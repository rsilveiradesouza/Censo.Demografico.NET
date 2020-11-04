using Censo.NET.Application.Dtos;
using Censo.NET.Application.Interfaces;
using Censo.NET.Domain.Common.Enums;
using Censo.NET.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.Application
{
    public class BuscaService : IBuscaService
    {
        private readonly IPesquisaRepository _pesquisaRepository;

        public BuscaService(IPesquisaRepository pesquisaRepository)
        {
            _pesquisaRepository = pesquisaRepository;
        }

        public async Task<ResultadoBuscaDto> Filtro(string nome, Regiao? regiao = null, Genero? genero = null, Etnia? etnia = null, Escolaridade? escolaridade = null)
        {
            var result = await _pesquisaRepository.Filtrar(nome, regiao, genero, etnia, escolaridade);
            return new ResultadoBuscaDto { Fraction = result.quantidade, Total = result.total };
        }

        public async Task<IEnumerable<IEnumerable<PesquisaDto>>> ArvoreGenealogica(Guid id, int nivel = 0) =>
            (await _pesquisaRepository.ArvoreGenealogica(id, nivel)).ParaPesquisaDto();
    }
}
