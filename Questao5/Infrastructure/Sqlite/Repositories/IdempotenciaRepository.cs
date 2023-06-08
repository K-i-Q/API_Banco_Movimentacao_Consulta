namespace Questao5.Infrastructure.Sqlite.Repositories
{
    using Questao5.Domain.Entities;
    using Questao5.Domain.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly List<Idempotencia> _idempotenciaEntries;

        public IdempotenciaRepository()
        {
            _idempotenciaEntries = new List<Idempotencia>();
        }

        public Task<Idempotencia> ObterIdempotenciaPorChave(string chave)
        {
            var idempotencia = _idempotenciaEntries.FirstOrDefault(e => e.ChaveIdempotencia == chave);
            return Task.FromResult(idempotencia);
        }

        public Task AdicionarIdempotencia(Idempotencia idempotencia)
        {
            _idempotenciaEntries.Add(idempotencia);
            return Task.CompletedTask;
        }
    }

}
