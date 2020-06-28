using System;
using System.Collections.Generic;
using System.Linq;

namespace State
{
    interface IStatusOrcamento
    {
        double CalculaDesconto(Orcamento orcamento);
        IStatusOrcamento Aprova();
        IStatusOrcamento Reprova();
        IStatusOrcamento Finaliza();
    }

    class StatusAprovado : IStatusOrcamento
    {
        public IStatusOrcamento Aprova()
        {
            throw new Exception("Orçamento já está aprovado !");
        }

        public double CalculaDesconto(Orcamento orcamento)
        {
            return orcamento.Total * 0.02;
        }

        public IStatusOrcamento Finaliza()
        {
            return new StatusFinalizado();
        }

        public IStatusOrcamento Reprova()
        {
            throw new Exception("Orçamento não pode ser mais reprovado !");
        }

        public override string ToString()
        {
            return "Aprovado";
        }
    }

    class StatusEmAprovacao : IStatusOrcamento
    {
        public IStatusOrcamento Aprova()
        {
            return new StatusAprovado();
        }

        public double CalculaDesconto(Orcamento orcamento)
        {
            return orcamento.Total * 0.05;
        }

        public IStatusOrcamento Finaliza()
        {
            throw new Exception("Orçamento não pode ser finalizado !");
        }

        public IStatusOrcamento Reprova()
        {
            return new StatusReprovado();
        }
        public override string ToString()
        {
            return "Em Aprovação";
        }
    }

    class StatusReprovado : IStatusOrcamento
    {
        public IStatusOrcamento Aprova()
        {
            throw new Exception("Orçamento não pode ser mais aprovado !");
        }

        public double CalculaDesconto(Orcamento orcamento)
        {
            return 0;
        }

        public IStatusOrcamento Finaliza()
        {
            return new StatusFinalizado();
        }

        public IStatusOrcamento Reprova()
        {
            throw new Exception("Orçamento já foi reprovado !");
        }

        public override string ToString()
        {
            return "Reprovado";
        }
    }

    class StatusFinalizado : IStatusOrcamento
    {
        public IStatusOrcamento Aprova()
        {
            throw new Exception("Orçamento já está finalizado !");
        }

        public double CalculaDesconto(Orcamento orcamento)
        {
            return 0;
        }

        public IStatusOrcamento Finaliza()
        {
            throw new Exception("Orçamento já está finalizado !");
        }

        public IStatusOrcamento Reprova()
        {
            throw new Exception("Orçamento já está finalizado !");
        }

        public override string ToString()
        {
            return "Finalizado";
        }
    }

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
        public double Desconto => StatusOrcamento.CalculaDesconto(this);
        public double TotalComDesconto => this.Total - this.Desconto;

        public IStatusOrcamento StatusOrcamento { get; private set; }

        public Orcamento(string descricao)
        {
            this.Descricao = descricao;
            this.Items = new List<Item>();
            this.StatusOrcamento = new StatusEmAprovacao();
        }

        public void AdicionaItem(Item item)
        {
            this.Items.Add(item);
        }

        public void Aprova()
        {
            this.StatusOrcamento = this.StatusOrcamento.Aprova();
        }

        public void Reprova()
        {
            this.StatusOrcamento = this.StatusOrcamento.Reprova();
        }

        public void Finaliza()
        {
            this.StatusOrcamento = this.StatusOrcamento.Finaliza();
        }

        
    }

    class OrcamentoConsole
    {
        static void Mainx(string[] args)
        {
            var orcamento = new Orcamento("Orcamento teste");
            orcamento.AdicionaItem(new Item("item1", 100));

            Console.WriteLine("Status Orcamento: {0} - Total: {2} - Desconto: {1} - Total com Desconto {3}", orcamento.StatusOrcamento, orcamento.Desconto, orcamento.Total, orcamento.TotalComDesconto);

            orcamento.Aprova();

            Console.WriteLine("Status Orcamento: {0} - Total: {2} - Desconto: {1} - Total com Desconto {3}", orcamento.StatusOrcamento, orcamento.Desconto, orcamento.Total, orcamento.TotalComDesconto);

            orcamento.Finaliza();

            Console.WriteLine("Status Orcamento: {0} - Total: {2} - Desconto: {1} - Total com Desconto {3}", orcamento.StatusOrcamento, orcamento.Desconto, orcamento.Total, orcamento.TotalComDesconto);


        }
    }
}
