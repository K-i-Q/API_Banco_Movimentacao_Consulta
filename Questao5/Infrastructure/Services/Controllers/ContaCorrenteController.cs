using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.DTOs;
using Questao5.Application.Services;
using Questao5.Domain.Entities;

namespace Questao5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteService _contaCorrenteService;
        private readonly IMapper _mapper;
        public ContaCorrenteController(IContaCorrenteService contaCorrenteService, IMapper mapper)
        {
            _contaCorrenteService = contaCorrenteService;
            _mapper = mapper;
        }

        [HttpPost("movimentacao")]
        public async Task<ActionResult<Guid>> MovimentarContaCorrente(MovimentacaoContaCorrenteDTO movimentacaoDto)
        {
            try
            {
                var idMovimentacao = await _contaCorrenteService.MovimentarContaCorrenteAsync(movimentacaoDto.IdRequisicao, movimentacaoDto.NumeroConta, movimentacaoDto.Valor, movimentacaoDto.TipoMovimento);
                return Ok(idMovimentacao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao movimentar a conta corrente.");
            }
        }


        [HttpGet("{id}/saldo")]
        public async Task<ActionResult<SaldoContaCorrente>> ConsultarSaldoContaCorrente(string id)
        {
            try
            {
                int numeroConta;
                if (!int.TryParse(id, out numeroConta))
                {
                    return BadRequest("ID da conta corrente inválido.");
                }

                var saldo = await _contaCorrenteService.ConsultarSaldoContaCorrenteAsync(numeroConta);
                return Ok(saldo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao consultar o saldo da conta corrente.");
            }
        }

    }
}
