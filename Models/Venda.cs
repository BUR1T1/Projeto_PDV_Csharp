namespace WebPDV.Models{

    public class Venda
    {
        public int NumeroDeSequencai { get; set; }
        public string NomeCliente { get; set; }
        public string NomeVendedor { get; set; }
        public string ItensDaVenda { get; set; }
        public DateOnly DataDaVenda {get; set;}
        public DateTime DataDaVendaDate { get; set;}
        public string FormaDePagamento { get; set; }
    }
}