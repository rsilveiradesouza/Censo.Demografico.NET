using System.Collections.Generic;

namespace Censo.NET.Application.Dtos
{
    public class PesquisaDto
    {
        public PesquisaInfoDto Pesquisa { get; set; }

        public IEnumerable<PesquisaInfoDto> Pais { get; set; }

        public IEnumerable<PesquisaInfoDto> Filhos { get; set; }
    }
}
