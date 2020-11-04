using Censo.NET.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Censo.NET.Infrastructure.Data.Configurations
{
    public class PesquisaPaiFilhoConfiguration : IEntityTypeConfiguration<PesquisaPaiFilho>
    {
        public void Configure(EntityTypeBuilder<PesquisaPaiFilho> builder)
        {
            builder.ToTable("TB_PESQUISAS_PAISFILHOS");
            builder.HasKey(c => c.Id);
            builder.Property(a => a.Id).HasColumnName("idPesquisaPaiFilho").ValueGeneratedOnAdd();
            builder.Property(a => a.PesquisaId).HasColumnName("idPesquisa").IsRequired();
            builder.Property(a => a.GrauParentesco).HasColumnName("intGrauParentesco").IsRequired();
            builder.Property(a => a.ParenteId).HasColumnName("idPesquisaParente").IsRequired();

            builder.HasOne(c => c.Pesquisa)
                .WithMany(c => c.Parentes)
                .HasForeignKey(sc => sc.PesquisaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
