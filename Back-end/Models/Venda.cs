using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;

namespace WebPDV.Models
{
    public class Venda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? NomeCliente { get; set; } 
        public string? NomeVendedor { get; set; }
        public string? FormaDePagamento { get; set; } 

        [Column(TypeName = "decimal(10,2)")] 
        public decimal ValorDaVenda { get; set; } 

        public List<ItemDaVenda> ItensDaVenda { get; set; } = new List<ItemDaVenda>();

     
        public class ItemDaVenda
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            public int ProdutoId { get; set; }
            public Produto? Produto { get; set; }

            public string? NomeProduto { get; set; } 
            
            [Column(TypeName = "decimal(10,2)")]
            public decimal? ValorUnitario { get; set; } 

            public int Quantidade { get; set; }

            [NotMapped] 
            public decimal Subtotal
            {
                get
                {
                    return Quantidade * (ValorUnitario ?? 0m);
                }
            }

            public int VendaId { get; set; }
            public Venda? Venda { get; set; }
        }
    }
}