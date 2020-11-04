using Censo.NET.Application.Dtos;
using Censo.NET.Domain.Common.Enums;
using Censo.NET.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace Censo.NET.Application
{
    public static class Mapping
    {
        public static Pesquisa ParaPesquisa(this PesquisaInfoDto pesquisaInfoDto) =>
            new Pesquisa
            {
                Nome = pesquisaInfoDto.Nome,
                Sobrenome = pesquisaInfoDto.Sobrenome,
                Regiao = pesquisaInfoDto.Regiao,
                Genero = pesquisaInfoDto.Genero,
                Escolaridade = pesquisaInfoDto.Escolaridade,
                Etnia = pesquisaInfoDto.Etnia,
                Parentes = new List<PesquisaPaiFilho>()
            };

        public static PesquisaDto ParaPesquisaDto(this Pesquisa pesquisa) =>
            new PesquisaDto
            {
                Pesquisa = new PesquisaInfoDto
                {
                    Id = pesquisa.Id,
                    Nome = pesquisa.Nome,
                    Sobrenome = pesquisa.Sobrenome,
                    Genero = pesquisa.Genero,
                    Escolaridade = pesquisa.Escolaridade,
                    Etnia = pesquisa.Etnia,
                    Regiao = pesquisa.Regiao
                },
                Pais = pesquisa.Parentes?.Where(c => c.GrauParentesco == GrauParentesco.Pai)?.Select(x => new PesquisaInfoDto
                {
                    Id = x.Parente.Id,
                    Nome = x.Parente.Nome,
                    Sobrenome = x.Parente.Sobrenome,
                    Genero = x.Parente.Genero,
                    Escolaridade = x.Parente.Escolaridade,
                    Etnia = x.Parente.Etnia,
                    Regiao = x.Parente.Regiao
                }),
                Filhos = pesquisa.Parentes?.Where(c => c.GrauParentesco == GrauParentesco.Filho)?.Select(x => new PesquisaInfoDto
                {
                    Id = x.Parente.Id,
                    Nome = x.Parente.Nome,
                    Sobrenome = x.Parente.Sobrenome,
                    Genero = x.Parente.Genero,
                    Escolaridade = x.Parente.Escolaridade,
                    Etnia = x.Parente.Etnia,
                    Regiao = x.Parente.Regiao
                })
            };

        public static Pesquisa ParaPesquisa(this PesquisaDto pesquisaDto) =>
            pesquisaDto.Pesquisa.ParaPesquisa();

        public static IEnumerable<Pesquisa> RecuperarFilhosPesquisa(this PesquisaDto pesquisaDto) =>
            pesquisaDto.Filhos?.Select(x => x.ParaPesquisa());

        public static IEnumerable<Pesquisa> RecuperarPaisPesquisa(this PesquisaDto pesquisaDto) =>
            pesquisaDto.Pais?.Select(x => x.ParaPesquisa());

        public static IEnumerable<PesquisaDto> ParaPesquisaDto(this IEnumerable<Pesquisa> pesquisas) =>
            pesquisas.Select(x => x.ParaPesquisaDto());

        public static IEnumerable<IEnumerable<PesquisaDto>> ParaPesquisaDto(this IEnumerable<IEnumerable<Pesquisa>> pesquisas) =>
             pesquisas.Select(x => x.ParaPesquisaDto());
    }
}
