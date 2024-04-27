using ApplicationCore.Attributes;
using ApplicationCore.Attributes.Personel;
using ApplicationCore.Entities;
using HRTechProject.Entities;
using Ornek.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityDenemeHRTech.Models
{
    public class PersonelGuncellemeViewModel : BaseEntity
    {
       // [Required(ErrorMessage = "Fotoğraf alanı boş olamaz..")]
        //[GecerliResim]
        public IFormFile? Fotograf { get; set; }

        public string? FotografYolu { get; set; }
        
        public string? Ad { get; set; } = null!;
        
        public string? Ad2 { get; set; }
        
        public string? Soyad { get; set; } = null!;
        
        public string? Soyad2 { get; set; }
        
        public DateTime? DogumTarihi { get; set; }
        
        public string? TcNo { get; set; } = null!;
        
        public DateTime? IseGirisTarihi { get; set; }
        
        public DateTime? IstenCikisTarihi { get; set; }        
        public int? MeslekId { get; set; }
        //public int? AdresId { get; set; }
        public Adres? Adres { get; set; }

        [Required(ErrorMessage = "İl alanı boş bırakılamaz.")]
        [HarfKontrol]
        [MaxLength(15,ErrorMessage = "Karakter sayısı 15'i geçemez")]
        public string Il { get; set; } = null!;

        [Required(ErrorMessage = "İlçe alanı boş bırakılamaz.")]
        [HarfKontrol]
        [MaxLength(25, ErrorMessage = "Karakter sayısı 25'i geçemez")]
        public string Ilce { get; set; } = null!;

        [Required(ErrorMessage = "Mahalle alanı boş bırakılamaz.")]
        [MaxLength(50)]
        public string Mahalle { get; set; } = null!;

        [Required(ErrorMessage = "Sokak alanı boş bırakılamaz.")]
        [MaxLength(50)]
        public string Sokak { get; set; } = null!;

        [Required(ErrorMessage = "Kapı no boş bırakılamaz.")]
        [MaxLength(6)]
        public string KapiNo { get; set; } = null!;


        [Required(ErrorMessage = "Daire no boş bırakılamaz.")]
        [Range(1, 1000, ErrorMessage = "Kapı no 1-1000 arası olmalıdır!")]
        public int DaireNo { get; set; } 


        public Meslek? Meslek { get; set; }
        public int? DepartmanId { get; set; }
        public Departman? Departman { get; set; }

        [Telefon]
        [MaxLength(10,ErrorMessage = "Telefon numarasını 10 hane olacak şekilde başında '0' olmadan giriniz")]
        [Required(ErrorMessage = "Telefon alanı boş olamaz..")]
        public string Telefon { get; set; } = null!;        
        public decimal? Maas { get; set; }
    }
}
