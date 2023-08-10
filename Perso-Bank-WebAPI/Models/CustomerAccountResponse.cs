namespace Perso_Bank_WebAPI.Models
{
    public class CustomerAccountResponse
    {
        public Response response { get; set; }
        public List<CustomerAccount> customerAccount { get; set; }

        public CustomerAccountResponse()
        {
            response = new Response();
            customerAccount = new List<CustomerAccount>();
        }
    }
}
