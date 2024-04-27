using Microsoft.AspNetCore.Identity;
using Ornek.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ApplicationCore.Entities;
using ApplicationCore.Attributes.Personel;
using ApplicationCore.Attributes;
using ApplicationCore.Enums;

namespace HRTechProject.Entities
{
    public class ApplicationUser : IdentityUser
    {       

        public string? FotografYolu { get; set; } = null!;

        [Required(ErrorMessage = "Alan boş bırakılamaz!")]
        [AdSoyad]
        [MaxLength(40)]
        public string? Ad { get; set; } = null!;

        [AdSoyad]
        [MaxLength(40)]
        public string? Ad2 { get; set; }

        [Required(ErrorMessage = "Alan boş bırakılamaz!")]
        [AdSoyad]
        [MaxLength(60)]
        public string? Soyad { get; set; } = null!;

        [AdSoyad]
        [MaxLength(60)]
        public string? Soyad2 { get; set; }

        [DogumTarihi]
        public DateTime? DogumTarihi { get; set; }

        [Required(ErrorMessage = "Alan boş bırakılamaz!")]
        [TcNo]
        public string? TcNo { get; set; } = null!;

        [PersonelEskiTarihKontrol]
        [IsTarihAraligi("baslangicTarihi", ErrorMessage = "İşe giriş tarihi, çıkış tarihinden sonra olamaz.")]
        public DateTime? IseGirisTarihi { get; set; }

        [PersonelEskiTarihKontrol]
        public DateTime? IstenCikisTarihi { get; set; }
        public bool? Aktiflik { get; set; }
        public int? MeslekId { get; set; }
        public int? AdresId { get; set; }
        public Adres Adres { get; set; }
        public Meslek Meslek { get; set; } = null!;
        public int? DepartmanId { get; set; }
        public Departman Departman { get; set; } = null!;
        public int? SirketId { get; set; }
        public Sirket Sirket { get; set; } = null!;
        public List<Harcama> Harcamalar { get; set; } = [];

        public List<Izin> Izinler { get; set; } = [];

        public List<Avans> Avanslar { get; set; } = [];

        [Telefon]
        [MaxLength(10)]
        public string? Telefon { get; set; } = null!;

        [Required(ErrorMessage = "Alan boş bırakılamaz!")]
        [Maas]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Maas { get; set; }

        public Cinsiyet? Cinsiyet { get; set; }

        public int IzinHakki => IzinGunSayisiHesaplama();

        public int IzinGunSayisiHesaplama()
        {
            if (IseGirisTarihi == null || IstenCikisTarihi == null)
            {                
                return 0;
            }
            
            DateTime iseGirisTarihi = IseGirisTarihi.Value;
            DateTime istenCikisTarihi = IstenCikisTarihi.Value;
            
            int yilFarki = istenCikisTarihi.Year - iseGirisTarihi.Year;
            
            if (yilFarki == 0)
            {                
                int gunFarki = (int)(istenCikisTarihi - iseGirisTarihi).TotalDays;
                
                if (gunFarki < 365)
                {
                    return 0;
                }
                else
                {                    
                    return 14;
                }
            }
            else
            {                
                int izinGunSayisi = (yilFarki - 1) * 14;             
                
                return izinGunSayisi;
            }
        }
    }
}
