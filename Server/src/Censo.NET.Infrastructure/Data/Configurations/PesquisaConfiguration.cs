using Censo.NET.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Censo.NET.Infrastructure.Data.Configurations
{
    public class PesquisaConfiguration : IEntityTypeConfiguration<Pesquisa>
    {
        public void Configure(EntityTypeBuilder<Pesquisa> builder)
        {
            builder.ToTable("TB_PESQUISAS");
            builder.HasKey(c => c.Id);
            builder.Property(a => a.Id).HasColumnName("idPesquisa").ValueGeneratedOnAdd();
            builder.Property(a => a.Escolaridade).HasColumnName("intEscolaridade");
            builder.Property(a => a.Genero).HasColumnName("intGenero");
            builder.Property(a => a.Etnia).HasColumnName("intEtnia");
            builder.Property(a => a.Regiao).HasColumnName("intRegiao");
            builder.Property(a => a.Nome).HasColumnName("chNome");
            builder.Property(a => a.Sobrenome).HasColumnName("chSobrenome");

            builder.HasIndex(c => new { c.Nome, c.Sobrenome }).IsUnique();
            builder.HasIndex(c => new { c.Nome, c.Sobrenome, c.Escolaridade, c.Regiao, c.Etnia, c.Genero });
        }
    }
}
