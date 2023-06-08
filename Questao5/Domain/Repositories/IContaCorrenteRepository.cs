using System.Threading.Tasks;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Repositories
{
    public interface IContaCorrenteRepository
    {
        Task<bool> MovimentarContaCorrenteAsync(string idContaCorrente, decimal valor, TipoMovimento tipoMovimento);
        Task<decimal> ConsultarSaldoAsync(string idContaCorrente);
        Task<Guid> PersistirMovimento(string idContaCorrente, Guid idRequisicao, string tipoMovimento, decimal valor);
        Task<ContaCorrente> ObterContaCorrentePorNumero(int numeroConta);
        Task<decimal> CalcularSaldoContaCorrente(string idContaCorrente);
    }
}
