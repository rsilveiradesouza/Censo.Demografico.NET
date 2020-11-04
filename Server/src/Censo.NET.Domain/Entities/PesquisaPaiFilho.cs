using Censo.NET.Domain.Common.Enums;
using System;

namespace Censo.NET.Domain.Model
{
    public class PesquisaPaiFilho
    {
        public Guid Id { get; set; }

        public Guid PesquisaId { get; set; }
        public Pesquisa Pesquisa { get; set; }

        public GrauParentesco GrauParentesco { get; set; }

        public Guid ParenteId { get; set; }
        public Pesquisa Parente { get; set; }
    }
}
