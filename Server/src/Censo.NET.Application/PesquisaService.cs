using Censo.NET.Application.Dtos;
using Censo.NET.Application.Interfaces;
using Censo.NET.Domain.Interfaces.API;
using Censo.NET.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Censo.NET.Application
{
    public class PesquisaService : IPesquisaService
    {
        private readonly IPesquisaRepository _pesquisaRepository;
        private readonly ICensoHub _dashboardHub;

        public PesquisaService(IPesquisaRepository pesquisaRepository,
            ICensoHub dashboardHub)
        {
            _pesquisaRepository = pesquisaRepository;
            _dashboardHub = dashboardHub;
        }

        public async Task<PesquisaDto> CadastrarPesquisa(PesquisaDto pesquisa)
        {
            var filhos = pesquisa.RecuperarFilhosPesquisa();
            var pais = pesquisa.RecuperarPaisPesquisa();
            var pessoa = pesquisa.ParaPesquisa();

            var result = await _pesquisaRepository.CriarPesquisa(pessoa, pais, filhos);

            var resultsDash = (await _pesquisaRepository.ObterDashboard()).ToList();

            await _dashboardHub.SendMessage(new { regioes = resultsDash[0], generos = resultsDash[1], escolaridades = resultsDash[2], etnias = resultsDash[3] });

            return result.ParaPesquisaDto();
        }

        public async Task<IEnumerable<PesquisaDto>> ObterTudo() =>
            (await _pesquisaRepository.ObterTodos()).ParaPesquisaDto();

        public async Task<PesquisaDto> Obter(Guid id) =>
            (await _pesquisaRepository.Obter(id)).ParaPesquisaDto();
    }
}
