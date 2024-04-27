using ApplicationCore.Attributes.Personel;
using ApplicationCore.Enums;
using HRTechProject.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityDenemeHRTech.Models
{
    public class PersonelEkleViewModel
    {
        [GecerliResim]
        [Display(Name ="Fotoğraf Ekle")]
        public IFormFile? Fotograf { get; set; }

        [TcNo]
        public string TC { get; set; }
        [Sifre(ErrorMessage = "Şifre en az 8 karakter uzunluğunda olmalı, en az bir rakam ve en az bir büyük harf içermelidir.")]
        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [Display(Name ="Şifre")]
        public string Sifre { get; set; }
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Maaş alanı gereklidir.")]
        [Range(0, double.MaxValue, ErrorMessage = "Geçerli bir maaş değeri giriniz.")]
        public decimal Maas { get; set; }

        [Required(ErrorMessage = "Mail adresi gereklidir.")]
        [Email]
        public string Mail { get; set; }

        [Required(ErrorMessage = "İşe giriş tarihi gereklidir.")]
        public DateTime IseGirisTarihi { get; set; }
        public DateTime DogumTarihi { get; set; }

        [Required(ErrorMessage = "Cinsiyet belirtilmelidir.")]
        public Cinsiyet Cinsiyet { get; set; }

        [MaxLength(10, ErrorMessage = "Telefon numarası en fazla 10 karakter olmalıdır.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Telefon numarası sadece rakamlardan oluşmalıdır.")]
        [DataType(DataType.PhoneNumber)]
        public string Telefon { get; set; }
        public AdresViewModel Adres { get; set; }

        public int DepartmanId { get; set; } // Seçilen departmanın ID'sini tutacak özellik

        [NotMapped]
        public List<Departman>?  Departmanlar { get; set; }

        public string SirketAdi { get; set; } // Seçilen şirketin adını tutacak özellik

        public List<Sirket> Sirketler { get; set; } = new(); // Şirketler listesi
        public int MeslekId { get; set; }

        [NotMapped]
        public List<Meslek>? Meslekler { get; set; } 
    }
}
