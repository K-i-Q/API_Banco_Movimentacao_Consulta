namespace Questao5.Domain.Repositories
{
    using Questao5.Domain.Entities;
    using System.Threading.Tasks;

    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> ObterIdempotenciaPorChave(Guid chave);
        Task AdicionarIdempotencia(Idempotencia idempotencia);
    }
}
