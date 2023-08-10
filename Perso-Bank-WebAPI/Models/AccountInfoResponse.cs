namespace Perso_Bank_WebAPI.Models
{
    public class AccountInfoResponse
    {
        public AccountInfo accountInfo { get; set; }
        public Response response { get; set; }
        public AccountInfoResponse() {
        this.accountInfo = new AccountInfo();
            this.response = new Response();

        }
    }
}
