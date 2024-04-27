namespace IdentityDenemeHRTech.Models
{
    public class MailViewModel
    {
        public string Sender { get; set; } = null!;
        public string Reciever { get; set; } = null!;        
        public string Subject { get; set; } = null!;        
        public string Body { get; set; } = null!;
    }
}
