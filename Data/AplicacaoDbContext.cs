
namespace WebPDV.Data
{
    public class AplicacaoDbContext : DbContext
    {

        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options)
            : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Atendente> Atendentes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }

    }
}
