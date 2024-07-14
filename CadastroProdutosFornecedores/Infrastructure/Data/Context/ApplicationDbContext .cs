using CadastroProdutosFornecedores.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CadastroProdutosFornecedores.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<ProdutoFornecedor> ProdutosFornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProdutoFornecedor>()
                .Property(p => p.FornecedorId)
                .IsRequired(false);
        }

    }
}
