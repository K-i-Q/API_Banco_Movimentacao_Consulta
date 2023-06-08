using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questao5.Application.Services
{
    public interface IContaCorrenteService
    {
        Task<MovimentarContaCorrenteResponse> MovimentarContaCorrenteAsync(Guid idRequisicao, int numeroConta, decimal valor, string tipoMovimento);
        Task<SaldoContaCorrenteResponse> ConsultarSaldoContaCorrenteAsync(int numeroConta);
    }
}
