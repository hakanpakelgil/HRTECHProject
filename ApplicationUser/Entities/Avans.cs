using ApplicationCore.Enums;
using HRTechProject.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Avans : BaseEntity
    {
        [Range(0,double.MaxValue,ErrorMessage = "Tutar 0'dan küçük olamaz.")]
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }

        [MaxLength(300)]
        public string Aciklama { get; set; } = null!;
        public DateTime? OnaylanmaTarihi { get; set; }
        public AvansTuru Tur { get; set; }
        public AvansDurumu Durum { get; set; } = AvansDurumu.Bekleme;
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        
    }
}
