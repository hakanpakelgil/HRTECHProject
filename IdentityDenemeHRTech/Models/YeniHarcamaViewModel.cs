using ApplicationCore.Attributes.Personel;
using ApplicationCore.Enums;
using System.ComponentModel.DataAnnotations;

namespace IdentityDenemeHRTech.Models
{
    public class YeniHarcamaViewModel
    {

        public HarcamaTuru Tur { get; set; }

        [Required(ErrorMessage = "Tutar alanı boş bırakılamaz.")]
        public decimal Tutar { get; set; }

        public ParaBirimi ParaBirimi { get; set; }

        [GecerliDosya]
        public IFormFile Dosya { get; set; } = null!;
    }
}
