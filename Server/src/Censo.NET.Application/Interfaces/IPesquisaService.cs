using Censo.NET.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Censo.NET.Application.Interfaces
{
    public interface IPesquisaService
    {
        public Task<PesquisaDto> CadastrarPesquisa(PesquisaDto pesquisa);
        public Task<IEnumerable<PesquisaDto>> ObterTudo();
        public Task<PesquisaDto> Obter(Guid id);
    }
}
