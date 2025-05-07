using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebPDV.Models
{
    public class Venda
    {
        public int NumeroDeSequencia { get; set; } // Nome corrigido
        public string NomeCliente { get; set; }
        public string NomeVendedor { get; set; }   

       /* public DateOnly DataDaVenda { get; set; }
        public DateTime DataDaVendaDate { get; set; }*/
        public string FormaDePagamento { get; set; }
        public decimal ValorDaVenda => ItensDaVenda.Sum(item => item.Subtotal); 

        
        // Componete de Itens da venda, e função de calculo de valor de produtos para a soma do valor fina da venda.
        public List<ItemDaVenda> ItensDaVenda { get; set; } = new List<ItemDaVenda>();
        public class ItemDaVenda{
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario => Produto.ValorDeVenda;
        public decimal Subtotal => ValorUnitario * Quantidade;
}
   
    }

}
