using Microsoft.EntityFrameworkCore;

using WebPDV.Models;

namespace WebPDV.Data
{
    public class AplicacaoDbContext : DbContext
    {

        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options)
            : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Vendedor> vendedores { get; set; }

        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        public DbSet<Venda.ItemDaVenda> ItensDaVenda { get; set; } // Isso é crucial!

    }
}
