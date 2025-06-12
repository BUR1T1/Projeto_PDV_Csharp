using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPDV.Models{

    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        public string? NomeProduto { get; set; }
        public string GrupoProdutos { get; set; }
        public int QuantidedeDeEstoque { get; set; }
       
        [Column(TypeName = "decimal(10,2)")]
        public Decimal ValorDeCusto { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public Decimal? ValorDeVenda { get; set; }
    }
}