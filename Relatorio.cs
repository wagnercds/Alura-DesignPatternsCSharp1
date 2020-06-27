using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace TempleteMethod
{
    class Conta
    {
        public string Nome { get; private set; }
        public double Saldo { get; private set; }

        public string Agencia { get; private set; }
        public string Numero { get; private set; }

        public Conta(string nome, double saldo, string agencia, string numero)
        {
            this.Nome = nome;
            this.Saldo = saldo;
            this.Agencia = agencia;
            this.Numero = numero;
        }
    }

    class Banco
    {
        public string Nome { get; private set; }

        public string Telefone { get; private set; }

        public string Email { get; private set; }

        public string Endereco { get; private set; }

        public Banco(string nome, string telefone, string email, string endereco)
        {
            this.Nome = nome;
            this.Telefone = telefone;
            this.Email = email;
            this.Endereco = endereco;
        }

    }

    abstract class RelatorioTemplate
    {
        protected abstract void Cabecalho(Banco banco);

        protected abstract void Detalhe(List<Conta> contas);

        protected abstract void Rodape(Banco banco);

        public void ImprimeRelatorio(Banco banco, List<Conta> contas)
        {
            Cabecalho(banco);
            Detalhe(contas);
            Rodape(banco);
        }
    }

    class RelatorioSimples : RelatorioTemplate
    {
        protected override void Cabecalho(Banco banco)
        {
            Console.WriteLine("Banco {0} Telefone: {1}", banco.Nome, banco.Telefone);
            Console.WriteLine();
        }

        protected override void Detalhe(List<Conta> contas)
        {
            contas.ForEach(conta =>
            {
                Console.WriteLine("Nome: {0} Saldo: {1}", conta.Nome, conta.Saldo);
            });
        }

        protected override void Rodape(Banco banco)
        {
            Console.WriteLine();
            Cabecalho(banco);
        }
    }

    class RelatorioComplexo : RelatorioTemplate
    {
        protected override void Cabecalho(Banco banco)
        {
            Console.WriteLine("Banco: {0} ", banco.Nome);
            Console.WriteLine("Endereço: {0} Telefone: {1}", banco.Endereco, banco.Telefone);
            Console.WriteLine();
        }

        protected override void Detalhe(List<Conta> contas)
        {
            contas.ForEach(conta =>
            {
                Console.WriteLine("Agêencia: {2} Conta: {3} Nome: {0} Saldo: {1}", conta.Nome, conta.Saldo, conta.Agencia, conta.Numero);
            });
        }

        protected override void Rodape(Banco banco)
        {
            Console.WriteLine();
            Console.WriteLine("Email: {0} Data e Hora: {1}", banco.Email, DateTime.Now);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var contas = new List<Conta>()
            {
                new Conta("Cliente 1", 100, "1", "1-1"),
                new Conta("Cliente 2", 200, "2", "2-2"),
                new Conta("Cliente 3", 300, "3", "3-3")
            };

            var banco = new Banco("Teste1", "0030-04390", "cc@teste1.com", "R.xxxx, 292");

            var relatorioSimples = new RelatorioSimples();
            relatorioSimples.ImprimeRelatorio(banco, contas);

            var relatorioComplexo = new RelatorioComplexo();
            relatorioComplexo.ImprimeRelatorio(banco, contas);
        }
    }
}
