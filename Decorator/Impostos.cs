using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Xml.Schema;

namespace ConsoleApp12
{
    class Item
    {
        public string Descricao { get; private set; }
        public double Valor { get; private set; }

        public Item(string descricao, double valor)
        {
            this.Descricao = descricao;
            this.Valor = valor;
        }
    }
    class Orcamento
    {
        public string Descricao { get; private set; }
        public List<Item> Items { get; private set; }
        public double Total => this.Items.Sum(item => item.Valor);

        public Orcamento(string descricao)
        {
            this.Descricao = descricao;
            this.Items = new List<Item>();
        }

        public void AdicionaItem(Item item)
        {
            this.Items.Add(item);
        }
    }
    interface IImposto
    {
        double CalculaImposto(Orcamento orcamento);
    }

    abstract class TemplateImposto : IImposto
    {
        public IImposto ProximoImposto { get; private set; }

        public TemplateImposto(IImposto proximoImposto)
        {
            this.ProximoImposto = proximoImposto;
        }
        public double CalculaImposto(Orcamento orcamento)
        {
            double imposto;

            if (UsarValorMaximo(orcamento))
                imposto = CalcularMaximoImposto(orcamento);
            else
                imposto = CalcularMenorImposto(orcamento);

            return imposto + (this.ProximoImposto == null ? 0 : this.ProximoImposto.CalculaImposto(orcamento));
        }

        protected abstract bool UsarValorMaximo(Orcamento orcamento);

        protected abstract double CalcularMaximoImposto(Orcamento orcamento);

        protected abstract double CalcularMenorImposto(Orcamento orcamento);
    }

    class ICPP : TemplateImposto
    {
        public ICPP() : base(null)
        {

        }

        public ICPP(IImposto proximoImposto) : base(proximoImposto)
        {

        }

        protected override double CalcularMaximoImposto(Orcamento orcamento)
        {
            return orcamento.Total * 0.07;
        }

        protected override double CalcularMenorImposto(Orcamento orcamento)
        {
            return orcamento.Total * 0.05;
        }

        protected override bool UsarValorMaximo(Orcamento orcamento)
        {
            return orcamento.Total > 500;
        }
    }

    class IKVC : TemplateImposto
    {
        public IKVC() : base(null)
        {

        }

        public IKVC(IImposto proximoImposto) : base(proximoImposto)
        {

        }

        protected override double CalcularMaximoImposto(Orcamento orcamento)
        {
            return orcamento.Total * 0.1;
        }

        protected override double CalcularMenorImposto(Orcamento orcamento)
        {
            return orcamento.Total * 0.06;
        }

        protected override bool UsarValorMaximo(Orcamento orcamento)
        {
            return orcamento.Total > 500 && orcamento.Items.Any(item => item.Valor > 100);
        }
    }

    class IHIT : TemplateImposto
    {
        public IHIT() : base(null)
        {

        }

        public IHIT(IImposto proximoImposto) : base(proximoImposto)
        {

        }

        protected override double CalcularMaximoImposto(Orcamento orcamento)
        {
            return (orcamento.Total * 0.13) + 100;
        }

        protected override double CalcularMenorImposto(Orcamento orcamento)
        {
            return orcamento.Total * 0.01;
        }

        protected override bool UsarValorMaximo(Orcamento orcamento)
        {
            return orcamento.Items.GroupBy(item => item.Descricao).Any(item => item.Count() > 1);
        }
    }

    class ImpostoMuitoAlto : TemplateImposto
    {
        public ImpostoMuitoAlto() : base(null)
        {

        }

        public ImpostoMuitoAlto(IImposto proximoImposto) : base(proximoImposto)
        {

        }

        protected override double CalcularMaximoImposto(Orcamento orcamento)
        {
            return orcamento.Total * 0.2;
        }

        protected override double CalcularMenorImposto(Orcamento orcamento)
        {
            return CalcularMaximoImposto(orcamento);
        }

        protected override bool UsarValorMaximo(Orcamento orcamento)
        {
            return true;
        }
    }

    class Impostos
    {
        static void Mainx(string[] args)
        {
            var orcamento = new Orcamento("Orcamento teste");
            orcamento.AdicionaItem(new Item("item1", 100));

            ExibiMensagens("ICPP, IKVC e IHIT no minimo", orcamento);

            orcamento.AdicionaItem(new Item("item1", 600));

            ExibiMensagens("ICPP, IKVC e IHIT no máximo", orcamento);

        }

        static void ExibiMensagens(string titulo, Orcamento orcamento)
        {
            var icpp = new ICPP();
            var ikvc = new IKVC();
            var ihit = new IHIT();
            var iccpCompostoIkvc = new ICPP(ikvc);
            var impostoMuitoAlto = new ImpostoMuitoAlto(ihit);

            Console.WriteLine(titulo);
            Console.WriteLine("Total Orcamento: {0}", orcamento.Total);
            Console.WriteLine("ICPP: {0}", icpp.CalculaImposto(orcamento));
            Console.WriteLine("IKVC: {0}", ikvc.CalculaImposto(orcamento));
            Console.WriteLine("IHIT: {0}", ihit.CalculaImposto(orcamento));
            Console.WriteLine("ICCP Composto com IKVC: {0}", iccpCompostoIkvc.CalculaImposto(orcamento));
            Console.WriteLine("Imposto muito alto com IHIT: {0}", impostoMuitoAlto.CalculaImposto(orcamento));

            Console.WriteLine();
        }

    }
}
