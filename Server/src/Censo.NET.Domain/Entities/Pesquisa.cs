using Censo.NET.Domain.Common.Enums;
using Censo.NET.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Censo.NET.Domain.Model
{
    public class Pesquisa : IEntity
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public Regiao? Regiao { get; set; }
        public Etnia? Etnia { get; set; }
        public Genero? Genero { get; set; }
        public Escolaridade? Escolaridade { get; set; }

        public IEnumerable<PesquisaPaiFilho> Parentes { get; set; }
    }
}
