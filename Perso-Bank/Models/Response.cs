namespace Perso_Bank.Models
{
    public class Response
    {
        public int status { get; set; }
        public string message { get; set; }

        public Response() { }
        public Response(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}
