using ApplicationCore.Attributes.Personel;
using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace HRTechProject.Entities
{
    public class Adres : BaseEntity
    {
        public List<ApplicationUser> ApplicationUsers { get; set; } = new();

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
        [MaxLength(5)]
        public int DaireNo { get; set; }

    }
}
