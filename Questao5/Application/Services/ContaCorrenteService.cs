using Questao5.Application.Queries.Responses;
using Questao5.Application.Services;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.Json;


public class ContaCorrenteService : IContaCorrenteService
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;

    public ContaCorrenteService(IContaCorrenteRepository contaCorrenteRepository, 
                                IIdempotenciaRepository idempotenciaRepository)
    {
        _contaCorrenteRepository = contaCorrenteRepository;
        _idempotenciaRepository = idempotenciaRepository;
    }

    public async Task<MovimentarContaCorrenteResponse> MovimentarContaCorrenteAsync(Guid idRequisicao, int numeroConta, decimal valor, string tipoMovimento)
    {

        var response = new MovimentarContaCorrenteResponse();
        // Realizar validações e processamento da movimentação da conta corrente
        if (string.IsNullOrEmpty(idRequisicao.ToString()))
        {
            response.HttpResponseMessage = CreateBadRequestResponse();
            response.Mensagem = "ID da requisição inválido.";
            return response;
        }

        // Verificar se a conta corrente existe e está ativa
        var contaCorrente = await _contaCorrenteRepository.ObterContaCorrentePorNumero(numeroConta);
        if (contaCorrente == null)
        {
            response.HttpResponseMessage = CreateBadRequestResponse();
            response.Mensagem = "Conta corrente não encontrada.";
            return response;
        }

        if (!contaCorrente.Ativo)
        {
            response.HttpResponseMessage = CreateBadRequestResponse();
            response.Mensagem = "A conta corrente está inativa.";
            return response;
        }

        // Validar o tipo de movimento (crédito ou débito)
        if (tipoMovimento != "C" && tipoMovimento != "D")
        {
            response.HttpResponseMessage = CreateBadRequestResponse();
            response.Mensagem = "Tipo de movimento inválido.";
            return response;
        }

        // Verificar se o valor é válido (positivo)
        if (valor <= 0)
        {
            response.HttpResponseMessage = CreateBadRequestResponse();
            response.Mensagem = "Valor inválido. O valor deve ser positivo.";
            return response;
        }

        // Verificar se a requisição já foi processada anteriormente
        var requisicaoExistente = await _idempotenciaRepository.ObterIdempotenciaPorChave(idRequisicao);
        if (requisicaoExistente != null)
        {
            // Retornar a resposta anterior ao invés de processar novamente
            response.MovimentoId = requisicaoExistente.Resultado;
            response.HttpResponseMessage = CreateOkResponse();
            return response;
        }

        // Persistir a movimentação na tabela MOVIMENTO
        var movimentoId = await _contaCorrenteRepository.PersistirMovimento(contaCorrente.IdContaCorrente, idRequisicao, tipoMovimento, valor);

        // Armazenar o resultado da requisição na tabela de idempotência
        await _idempotenciaRepository.AdicionarIdempotencia(new Idempotencia { ChaveIdempotencia = Guid.NewGuid(),
                                                                                Requisicao = idRequisicao, 
                                                                                Resultado = movimentoId });

        // Retornar HTTP 200 OK com o ID do movimento gerado no body
        response.MovimentoId = movimentoId;
        response.HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        return response;
    }

    public async Task<SaldoContaCorrenteResponse> ConsultarSaldoContaCorrenteAsync(int numeroConta)
    {
        var responseNotOk = new SaldoContaCorrenteResponse();
        // Verificar se a conta corrente existe e está ativa
        var contaCorrente = await _contaCorrenteRepository.ObterContaCorrentePorNumero(numeroConta);
        if (contaCorrente == null)
        {
            responseNotOk.HttpResponseMessage = CreateBadRequestResponse("INVALID_ACCOUNT");
            responseNotOk.Mensagem = "Conta corrente não encontrada.";
            return responseNotOk;
        }

        // Verificar se a conta corrente está ativa
        if (!contaCorrente.Ativo)
        {
            responseNotOk.HttpResponseMessage = CreateBadRequestResponse("INACTIVE_ACCOUNT");
            responseNotOk.Mensagem = "A conta corrente está inativa.";
            return responseNotOk;
        }

        // Calcular o saldo da conta corrente com base nos movimentos persistidos
        var saldo = await _contaCorrenteRepository.CalcularSaldoContaCorrente(contaCorrente.IdContaCorrente);

        // Retornar HTTP 200 OK com os dados do saldo no body
        var response = new SaldoContaCorrenteResponse
        {
            NumeroContaCorrente = contaCorrente.Numero,
            TitularContaCorrente = contaCorrente.Nome,
            DataConsulta = DateTime.Now,
            SaldoAtual = saldo,
            HttpResponseMessage = CreateOkResponse()
        };
        return response;
    }

    private HttpResponseMessage CreateBadRequestResponse()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        return response;
    }

    private HttpResponseMessage CreateBadRequestResponse(string codigoErro)
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Headers.Add("Codigo-Erro", codigoErro);
        return response;
    }

    private HttpResponseMessage CreateOkResponse()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        return response;
    }
}
