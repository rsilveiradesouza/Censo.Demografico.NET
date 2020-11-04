using Censo.NET.Domain.Model;
using Censo.NET.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Censo.NET.Infrastructure.Data
{
    public class CensoContext : DbContext
    {
        public CensoContext(DbContextOptions<CensoContext> options) : base(options) { }

        public DbSet<Pesquisa> Pesquisas { get; set; }
        public DbSet<PesquisaPaiFilho> PesquisasPaisFilhos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PesquisaPaiFilhoConfiguration());
            modelBuilder.ApplyConfiguration(new PesquisaConfiguration());
        }
    }
}
