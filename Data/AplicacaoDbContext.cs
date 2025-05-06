using Microsoft.EntityFrameworkCore;

using WebPDV.Models;

namespace WebPDV.Data
{
    public class AplicacaoDbContext : DbContext
    {

        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options)
            : base(options) { }

        public DbSet<Produto> produtos { get; set; }
        public DbSet<Vendedor> vendedores { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }

    }
}
