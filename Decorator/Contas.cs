using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator
{
    class Conta
    {
        public string Nome { get; private set; }
        public double Saldo { get; private set; }

        public string Agencia { get; private set; }
        public string Numero { get; private set; }

        public DateTime Abertura { get; private set; }

        public Conta(string nome, double saldo, string agencia, string numero, DateTime abertura)
        {
            this.Nome = nome;
            this.Saldo = saldo;
            this.Agencia = agencia;
            this.Numero = numero;
            this.Abertura = abertura;
        }
    }

    abstract class Filtro
    {
        public Filtro ProximoFiltro { get; private set; }

        public Filtro(Filtro proximoFiltro)
        {
            this.ProximoFiltro = proximoFiltro;
        }
        public IList<Conta> FiltroComposto (IList<Conta> contas)
        {
            List<Conta> lista = (List<Conta>)this.Filtra(contas);

            if (this.ProximoFiltro != null)
                lista.AddRange(this.ProximoFiltro.Filtra(contas));

            return lista;
        }
        public abstract IList<Conta> Filtra(IList<Conta> contas);
    }

    class FiltraSaldoMenor100 : Filtro
    {
        public FiltraSaldoMenor100() : base (null)
        {

        }

        public FiltraSaldoMenor100(Filtro proximoFiltro) : base(proximoFiltro)
        {

        }


        public override IList<Conta> Filtra(IList<Conta> contas)
        {
            return contas.Where(conta => conta.Saldo < 100).ToList();
        }
    }

    class FiltraSaldoMaior500 : Filtro
    {
        public FiltraSaldoMaior500() : base(null)
        {

        }

        public FiltraSaldoMaior500(Filtro proximoFiltro) : base(proximoFiltro)
        {

        }

        public override IList<Conta> Filtra(IList<Conta> contas)
        {
            return contas.Where(conta => conta.Saldo > 500).ToList();
        }
    }

    class FiltraMesCorrente : Filtro
    {
        public FiltraMesCorrente() : base(null)
        {

        }

        public FiltraMesCorrente(Filtro proximoFiltro) : base(proximoFiltro)
        {

        }

        public override IList<Conta> Filtra(IList<Conta> contas)
        {
            return contas.Where(conta => conta.Abertura.Month == DateTime.Now.Month && conta.Abertura.Year == DateTime.Now.Year).ToList();
        }
    }

    class Contas
    {
        static void Main(string[] args)
        {
            var contas = new List<Conta>()
            {
                new Conta("Conta Saldo 100", 100, "1", "1", DateTime.Now),
                new Conta("Conta Saldo 600", 600, "1", "2", new DateTime(2019, 10, 3)),
                new Conta("Conta Saldo 200", 200, "1", "3", new DateTime(2019, 10, 3)),
                new Conta("Conta Saldo 300", 300, "1", "4", DateTime.Now),
            };

            var filtroMenor100 = new FiltraSaldoMenor100();
            var filtroMaior500 = new FiltraSaldoMaior500(filtroMenor100);
            var filtroMesCorrente = new FiltraMesCorrente(filtroMaior500);

            IList<Conta> contasFiltrada = filtroMesCorrente.FiltroComposto(contas);

            foreach(Conta conta in contasFiltrada)
            {
                Console.WriteLine("Conta: {0} - Saldo: {1} - Data Abertura: {2}", conta.Nome, conta.Saldo, conta.Abertura);
            }

        }
    }
}
