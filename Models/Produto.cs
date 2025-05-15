namespace WebPDV.Models{

    public class Produto
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public string GrupoProdutos { get; set; }
        public int QuantidedeDeEstoque { get; set; }
        public Decimal ValorDeCusto { get; set; }
        public Decimal ValorDeVenda { get; set; }
    }
}