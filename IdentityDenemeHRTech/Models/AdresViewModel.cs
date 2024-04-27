using ApplicationCore.Attributes.Personel;
using System.ComponentModel.DataAnnotations;

namespace IdentityDenemeHRTech.Models
{
    public class AdresViewModel
    {
        [Required(ErrorMessage = "İl alanı boş bırakılamaz.")]
        [HarfKontrol]
        [MaxLength(15)]
        public string Il { get; set; } = null!;

        [Required(ErrorMessage = "İlçe alanı boş bırakılamaz.")]
        [HarfKontrol]
        [MaxLength(25)]
        public string Ilce { get; set; } = null!;

        [Required(ErrorMessage = "Mahalle alanı boş bırakılamaz.")]
        [MaxLength(50)]
        public string Mahalle { get; set; } = null!;

        [Required(ErrorMessage = "Sokak alanı boş bırakılamaz.")]
        [MaxLength(50)]
        public string Sokak { get; set; } = null!;

        [Required(ErrorMessage = "Kapı no boş bırakılamaz.")]
        [MaxLength(5)]
        public string KapiNo { get; set; } = null!;

        [Required(ErrorMessage = "Daire no boş bırakılamaz.")]
        [Range(0,100)]
        public int DaireNo { get; set; }
    }
}
