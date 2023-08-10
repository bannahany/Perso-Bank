
namespace Perso_Bank.Models
{
    public class Trans
    {        
        public int? accountNumber { get; set; }
        public string? transType { get; set;}
        public decimal? amount { get; set; }
        public Trans() { }
                
    }
}

