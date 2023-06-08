using System;

namespace Questao5.Application.Queries.Responses
{
    public class MovimentarContaCorrenteResponse
    {
        public Guid MovimentoId { get; set; } // ID do movimento gerado
        public string Mensagem { get; set; } // Mensagem descritiva da resposta
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }
}
