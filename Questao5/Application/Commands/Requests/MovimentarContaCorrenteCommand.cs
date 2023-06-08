using MediatR;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCorrenteCommand : IRequest<string>
    {
        public string ContaCorrenteId { get; set; } // Identificação da conta corrente
        public decimal Valor { get; set; } // Valor a ser movimentado
        public TipoMovimento TipoMovimento { get; set; } // Tipo de movimento (C = Crédito, D = Débito)
    }
}
