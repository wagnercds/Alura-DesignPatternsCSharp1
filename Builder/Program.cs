using System;
using System.Collections.Generic;

namespace Builder
{
    class ItemDaNota
    {
        public string Descricao { get; private set; }
        public double Valor { get; private set; }

        public ItemDaNota(string descricao, double valor)
        {
            this.Descricao = descricao;
            this.Valor = valor;
        }
    }

    class ItemDaNotaBuilder
    {
        private string _Descricao;
        private double _Valor;

        public ItemDaNotaBuilder ComDescricao(string descricao)
        {
            _Descricao = descricao;
            return this;
        }

        public ItemDaNotaBuilder ComValor(double valor)
        {
            _Valor = valor;
            return this;
        }

        public ItemDaNota Constroi()
        {
            return new ItemDaNota(_Descricao, _Valor);
        }
    }

    class NotaFiscal
    {
        public string RazaoSocial { get; private set; }
        public string CNPJ { get; private set; }
        public double ValorBruto { get; private set; }
        public double Impostos { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public string Observacoes { get; private set; }
        public List<ItemDaNota> Itens { get; private set; }

        public NotaFiscal(
            string razaoSocial,
            string CNPJ,
            double valorBruto,
            double impostos,
            DateTime dataEmissao,
            string observacoes,
            List<ItemDaNota> itens
            )
        {
            this.RazaoSocial = razaoSocial;
            this.CNPJ = CNPJ;
            this.ValorBruto = valorBruto;
            this.Impostos = impostos;
            this.DataEmissao = dataEmissao;
            this.Observacoes = observacoes;
            this.Itens = itens;
        }
    }

    class NotaFiscalBuilder
    {
        private string _RazaoSocial;
        private string _CNPJ;
        private double _ValorBruto = 0;
        private double _Impostos = 0;
        private DateTime _DataEmissao = DateTime.Now;
        private string _Observacoes;
        private List<ItemDaNota> _Itens = new List<ItemDaNota>();

        public NotaFiscalBuilder ParaEmpresa(string razaoSocial)
        {
            _RazaoSocial = razaoSocial;
            return this;
        }

        public NotaFiscalBuilder ComCNPJ(string CNPJ)
        {
            _CNPJ = CNPJ;
            return this;
        }

        public NotaFiscalBuilder ComItem(ItemDaNota itemDaNota)
        {
            _ValorBruto += itemDaNota.Valor;
            _Impostos += itemDaNota.Valor * 0.05;
            _Itens.Add(itemDaNota);
            return this;
        }

        public NotaFiscalBuilder ComDataEmissaoEspecifica(DateTime data)
        {
            _DataEmissao = data;
            return this;
        }

        public NotaFiscalBuilder ComObservacoes(string observacoes)
        {
            _Observacoes = observacoes;
            return this;
        }

        public NotaFiscal Constroi()
        {
            return new NotaFiscal(
                razaoSocial: _RazaoSocial,
                CNPJ: _CNPJ,
                valorBruto: _ValorBruto,
                impostos: _Impostos,
                dataEmissao: _DataEmissao,
                observacoes: _Observacoes,
                itens: _Itens
                );
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            NotaFiscal notaFiscalDataAtual = (new NotaFiscalBuilder())
                .ParaEmpresa("Teste com data atual")
                .ComCNPJ("2992929292")
                .ComItem(new ItemDaNota("prod1", 100))
                //.ComObservacoes("observacao")
                .Constroi();

            Show(notaFiscalDataAtual);

            NotaFiscal notaFiscalDataEspecifica = (new NotaFiscalBuilder())
                .ParaEmpresa("Teste com data especifica")
                .ComCNPJ("2992929292")
                .ComItem(new ItemDaNota("prod1", 300))
                .ComObservacoes("observacao")
                .ComDataEmissaoEspecifica(new DateTime(1978,5,3))
                .Constroi();

            Show(notaFiscalDataEspecifica);
        }

        static void Show(NotaFiscal notafiscal)
        {
            Console.WriteLine("RazaoSocial: {0}", notafiscal.RazaoSocial);
            Console.WriteLine("CNPJ: {0}", notafiscal.CNPJ);
            Console.WriteLine("Valor Bruto: {0}", notafiscal.ValorBruto);
            Console.WriteLine("Impostos: {0}", notafiscal.Impostos);
            Console.WriteLine("Observações: {0}", notafiscal.Observacoes);
            Console.WriteLine("Itens:");
            notafiscal.Itens.ForEach(item =>
            {
                Console.WriteLine("Descrição: {0} - Valor: {1}", item.Descricao, item.Valor);
            });
            Console.WriteLine("Data Emissão: {0}", notafiscal.DataEmissao);

            Console.WriteLine();
        }
    }
}
