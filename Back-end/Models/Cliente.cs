using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPDV.Models{

    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        public string? NomeCliente { get; set; }
        public string email {get; set;}
        public string? Telefone{ get; set; }
        public string Endereco {get; set;}
        
    }
}