using Censo.NET.Application.Dtos;
using Censo.NET.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.Application.Interfaces
{
    public interface IBuscaService
    {
        Task<ResultadoBuscaDto> Filtro(string nome, Regiao? regiao = null, Genero? genero = null, Etnia? etnia = null, Escolaridade? escolaridade = null);
        Task<IEnumerable<IEnumerable<PesquisaDto>>> ArvoreGenealogica(Guid id, int nivel = 0);
    }
}
