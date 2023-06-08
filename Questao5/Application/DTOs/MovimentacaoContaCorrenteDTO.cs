namespace Questao5.Application.DTOs
{
    public class MovimentacaoContaCorrenteDTO
    {
        public Guid IdRequisicao { get; set; }
        public int NumeroConta { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
    }
}
