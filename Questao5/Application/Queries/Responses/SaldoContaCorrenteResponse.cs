using System;

namespace Questao5.Application.Queries.Responses
{
    public class SaldoContaCorrenteResponse
    {
        public int NumeroContaCorrente { get; set; } // Número da conta corrente
        public string TitularContaCorrente { get; set; } // Titular da conta corrente
        public DateTime DataConsulta { get; set; } // Data e hora da consulta
        public decimal SaldoAtual { get; set; } // Saldo atual da conta corrente
        public string Mensagem { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }
}
