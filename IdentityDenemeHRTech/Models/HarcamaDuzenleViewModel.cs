using ApplicationCore.Enums;

namespace IdentityDenemeHRTech.Models
{
	public class HarcamaDuzenleViewModel
	{
        public int Id { get; set; }
        public IFormFile? Dosya { get; set; }
        public IzinDurumu Durum { get; set; }
        public ParaBirimi ParaBirimi { get; set; }
        public DateTime TalepTarihi { get; set; }
      
        public HarcamaTuru Tur { get; set; }
        public decimal Tutar { get; set; }
    }
   
}
