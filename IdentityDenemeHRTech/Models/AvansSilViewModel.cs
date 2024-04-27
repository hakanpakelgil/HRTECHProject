using ApplicationCore.Enums;

namespace IdentityDenemeHRTech.Models
{
    public class AvansSilViewModel
    {

        public int Id { get; set; }
        public decimal Tutar { get; set; }
        public string Aciklama { get; set; } = null!;
        public AvansTuru Tur { get; set; }
    }
}
