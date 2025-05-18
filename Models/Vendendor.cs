namespace WebPDV.Models{

    public class Vendedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string nomeVendedor { get; set; }
        public double ValorDaComissao { get; set; }
    }
}