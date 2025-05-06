namespace WebPDV.Models{

    public class Produto
    {
        public int CodigoID { get; set; }
        public string NomeProduto { get; set; }
        public string GrupoProdutos { get; set; }
        public string ValorDeCusto { get; set; }
        public float ValorDeVenda { get; set; }
    }
}