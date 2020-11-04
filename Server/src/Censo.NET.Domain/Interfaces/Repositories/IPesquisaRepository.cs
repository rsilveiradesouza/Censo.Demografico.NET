using Censo.NET.Domain.Common.Enums;
using Censo.NET.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.Domain.Interfaces.Data
{
    public interface IPesquisaRepository : IRepository<Pesquisa>
    {
        Task<Pesquisa> CriarPesquisa(Pesquisa model, IEnumerable<Pesquisa> pais, IEnumerable<Pesquisa> filhos);

        Task<(int quantidade, int total)> Filtrar(string nome, Regiao? regiao, Genero? genero, Etnia? etnia, Escolaridade? escolaridade);

        Task<List<List<Pesquisa>>> ArvoreGenealogica(Guid id, int nivelMaximo = 0);

        Task<IEnumerable<IEnumerable<Dashboard>>> ObterDashboard();
    }
}
