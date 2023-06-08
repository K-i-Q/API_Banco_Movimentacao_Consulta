using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands
{
    public class MovimentacaoContaCorrenteCommand
    {
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimento TipoMovimento { get; set; }
    }
}
