using ApplicationCore.Enums;

namespace IdentityDenemeHRTech.Models
{
    public class YeniAvansViewModel
    {
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public AvansTuru Tur { get; set; }
        public AvansDurumu Durum { get; set; }
    }
}
