using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using System.Data;

namespace Questao5.Domain.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IDbConnection _dbConnection;

        public ContaCorrenteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> MovimentarContaCorrenteAsync(string idContaCorrente, decimal valor, TipoMovimento tipoMovimento)
        {
            // Implemente a lógica para persistir a movimentação da conta corrente no banco de dados
            var query = @"INSERT INTO movimento (idcontacorrente, valor, tipomovimento) VALUES (@IdContaCorrente, @Valor, @TipoMovimento)";
            var parameters = new { IdContaCorrente = idContaCorrente, Valor = valor, TipoMovimento = tipoMovimento };
            var rowsAffected = await _dbConnection.ExecuteAsync(query, parameters);

            return rowsAffected > 0;
        }

        public async Task<decimal> ConsultarSaldoAsync(string idContaCorrente)
        {
            // Implemente a lógica para calcular o saldo da conta corrente no banco de dados
            var query = @"SELECT SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END)
                          FROM movimento
                          WHERE idcontacorrente = @IdContaCorrente";
            var parameters = new { IdContaCorrente = idContaCorrente };
            var saldo = await _dbConnection.ExecuteScalarAsync<decimal>(query, parameters);

            return saldo;
        }

        public async Task<Guid> PersistirMovimento(string idContaCorrente, Guid idRequisicao, string tipoMovimento, decimal valor)
        {
            // Implemente a lógica para persistir o movimento na tabela MOVIMENTO
            var movimentoId = Guid.NewGuid();
            var dataMovimento = DateTime.Now.ToString("dd/MM/yyyy");

            var query = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                          VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
            var parameters = new
            {
                IdMovimento = movimentoId.ToString(),
                IdContaCorrente = idContaCorrente,
                DataMovimento = dataMovimento,
                TipoMovimento = tipoMovimento,
                Valor = valor
            };
            await _dbConnection.ExecuteAsync(query, parameters);

            return movimentoId;
        }

        public async Task<ContaCorrente> ObterContaCorrentePorNumero(int numeroConta)
        {
            // Implemente a lógica para obter a conta corrente pelo número no banco de dados
            var query = "SELECT * FROM contacorrente WHERE numero = @NumeroConta";
            var parameters = new { NumeroConta = numeroConta };
            var contaCorrente = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(query, parameters);

            return contaCorrente;
        }

        public async Task<decimal> CalcularSaldoContaCorrente(string idContaCorrente)
        {
            // Implemente a lógica para calcular o saldo da conta corrente no banco de dados
            var query = @"SELECT SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END)
                          FROM movimento
                          WHERE idcontacorrente = @IdContaCorrente";
            var parameters = new { IdContaCorrente = idContaCorrente };
            var saldo = await _dbConnection.ExecuteScalarAsync<decimal>(query, parameters);

            return saldo;
        }
    }
}
