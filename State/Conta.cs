using System;
using System.Collections.Generic;
using System.Text;

namespace State
{
    class ContaConsole
    {

        interface IStatusConta
        {
            (IStatusConta statuAtual, double saldo) Deposita(Conta conta, double valor);
            (IStatusConta statuAtual, double saldo) Saque(Conta conta, double valor);
        }

        class StatusNegativo : IStatusConta
        {
            public (IStatusConta statuAtual, double saldo) Deposita(Conta conta, double valor)
            {
                var saldo = conta.Saldo + (valor * 0.95);
                var statusAtual = saldo < 0 ? (IStatusConta) this : (IStatusConta) new StatusPositivo();

                return (statusAtual, saldo);
            }

            public (IStatusConta statuAtual, double saldo) Saque(Conta conta, double valor)
            {
                throw new Exception("Saldo da conta negativo");
            }

            public override string ToString()
            {
                return "Negativo";
            }
        }

        class StatusPositivo : IStatusConta
        {
            public (IStatusConta statuAtual, double saldo) Deposita(Conta conta, double valor)
            {
                var saldo = conta.Saldo + (valor * 0.98);

                return (StatusAtual(saldo), saldo);
            }

            public (IStatusConta statuAtual, double saldo) Saque(Conta conta, double valor)
            {
                var saldo = conta.Saldo - valor;

                return (StatusAtual(saldo), saldo);
            }

            private IStatusConta StatusAtual(double saldo)
            {
                return saldo < 0 ? (IStatusConta)new StatusNegativo() : (IStatusConta)this;
            }

            public override string ToString()
            {
                return "Positivo";
            }
        }

        class Conta
        {
            public string Nome { get; private set; }
            public double Saldo { get; private set; }

            public string Agencia { get; private set; }
            public string Numero { get; private set; }

            public DateTime Abertura { get; private set; }

            public IStatusConta Status { get; private set; }

            public Conta(string nome, double saldo, string agencia, string numero, DateTime abertura)
            {
                this.Nome = nome;
                this.Saldo = saldo;
                this.Agencia = agencia;
                this.Numero = numero;
                this.Abertura = abertura;

                if (this.Saldo < 0)
                    this.Status = new StatusNegativo();
                else
                    this.Status = new StatusPositivo();
            }

            public void Deposita(double valor)
            {
                (this.Status, this.Saldo) = this.Status.Deposita(this, valor);
            }

            public void Saque(double valor)
            {
                (this.Status, this.Saldo) = this.Status.Saque(this, valor);
            }
        }
        static void Main(string[] args)
        {
            var conta = new Conta("conta", 100, "1", "2002", DateTime.Now);
            Console.WriteLine("Saldo: {0} - Status: {1}", conta.Saldo, conta.Status);

            conta.Saque(200);
            Console.WriteLine("Saque 200");
            Console.WriteLine("Saldo: {0} - Status: {1}", conta.Saldo, conta.Status);

            conta.Deposita(100);
            Console.WriteLine("Deposita 100");
            Console.WriteLine("Saldo: {0} - Status: {1}", conta.Saldo, conta.Status);

            conta.Deposita(100);
            Console.WriteLine("Deposita 100");
            Console.WriteLine("Saldo: {0} - Status: {1}", conta.Saldo, conta.Status);

            conta.Saque(50);
            Console.WriteLine("Saque 50");
            Console.WriteLine("Saldo: {0} - Status: {1}", conta.Saldo, conta.Status);
        }
    }
}
