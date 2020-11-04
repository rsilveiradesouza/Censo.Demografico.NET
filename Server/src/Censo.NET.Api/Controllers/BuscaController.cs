using Censo.NET.Application.Dtos;
using Censo.NET.Application.Interfaces;
using Censo.NET.Domain.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.API.Controllers
{
    [ApiController]
    [Route("/api/busca")]
    public class BuscaController : ControllerBase
    {
        private readonly IBuscaService _buscaService;

        public BuscaController(IBuscaService buscaService)
        {
            _buscaService = buscaService;
        }

        /// <summary>
        /// Obtém dados estatísticos
        /// </summary>
        /// <param name="nome">Nome do usuário</param>
        /// <param name="regiao">Região</param>
        /// <param name="genero">Gênero</param>
        /// <param name="etnia">Etnia</param>
        /// <param name="escolaridade">Escolaridade</param>
        /// <response code="200">Dados obtidos com sucesso</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("filtro")]
        public async Task<ActionResult<ResultadoBuscaDto>> Filtro(string nome, Regiao? regiao = null, Genero? genero = null, Etnia? etnia = null, Escolaridade? escolaridade = null)
        {
            try
            {
                return Ok(await _buscaService.Filtro(nome, regiao, genero, etnia, escolaridade));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Árvore genealógica de uma pesquisa
        /// </summary>
        /// <param name="id">Id da pesquisa</param>
        /// <param name="nivel">Nível máximo da árvore genealógica solicitada</param>
        /// <response code="200">Árvore genealógica obtida com sucesso</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("arvoreGenealogica")]
        public async Task<ActionResult<IEnumerable<IEnumerable<PesquisaDto>>>> ArvoreGenealogica(Guid id, int nivel = 0)
        {
            try
            {
                return Ok(await _buscaService.ArvoreGenealogica(id, nivel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
