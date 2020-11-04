using Censo.NET.Application.Dtos;
using Censo.NET.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.API.Controllers
{
    [ApiController]
    [Route("/api/pesquisa")]
    public class PesquisaController : ControllerBase
    {
        private readonly IPesquisaService _pesquisaService;

        public PesquisaController(IPesquisaService pesquisaService)
        {
            _pesquisaService = pesquisaService;
        }

        /// <summary>
        /// Obtém uma pesquisa do censo
        /// </summary>
        /// <param name="id">Id da pesquisa a ser obtida</param>
        /// <response code="200">Pesquisa obtida com sucesso</response>
        /// <response code="404">Pesquisa não foi encontrada</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<PesquisaDto>> Obter(Guid id)
        {
            try
            {
                var result = await _pesquisaService.Obter(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Obtém todas as pesquisas
        /// </summary>
        /// <response code="200">Pesquisas obtidas com sucesso</response>
        /// <response code="500">Erro interno</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PesquisaDto>>> ObterTudo()
        {
            try
            {
                return Ok(await _pesquisaService.ObterTudo());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Recebe nova pesquisa do censo
        /// </summary>
        /// <param name="pesquisa">Info da pesquisa</param>
        /// <response code="201">Pesquisa cadastrada com sucesso</response>
        /// <response code="500">Erro interno</response>
        [HttpPost]
        public async Task<ActionResult<PesquisaDto>> CadastrarPesquisa([FromBody] PesquisaDto pesquisa)
        {
            try
            {
                var result = await _pesquisaService.CadastrarPesquisa(pesquisa);
                return CreatedAtAction(nameof(Obter), new { id = result.Pesquisa.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetBaseException().Message);
            }
        }
    }
}