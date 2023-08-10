namespace Perso_Bank_WebAPI.Models
{
    public class CustomerAccount
    {
        public string accountNumber { get; set; }
        public string accountType { get; set; }
        public decimal accountBalance { get; set; }
        public bool isActive { get; set; }   

        public string displayName { get; set; }
        
        public CustomerAccount()
        {
         }
    }
}
