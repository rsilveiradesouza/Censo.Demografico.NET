using Censo.NET.Domain.Common.Enums;
using System;

namespace Censo.NET.Application.Dtos
{
    public class PesquisaInfoDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public Genero? Genero { get; set; }

        public Regiao? Regiao { get; set; }

        public Etnia? Etnia { get; set; }

        public Escolaridade? Escolaridade { get; set; }
    }
}
