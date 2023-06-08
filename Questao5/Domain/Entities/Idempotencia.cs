namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public Guid ChaveIdempotencia { get; set; } // Identificação da chave de idempotência
        public Guid Requisicao { get; set; } // Dados da requisição
        public Guid Resultado { get; set; } // Dados de retorno
    }
}
