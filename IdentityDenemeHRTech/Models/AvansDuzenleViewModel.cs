using ApplicationCore.Enums;
using HRTechProject.Entities;

namespace IdentityDenemeHRTech.Models
{
    public class AvansDuzenleViewModel
    {
        public int Id { get; set; }
        public decimal Tutar { get; set; }        
        public string Aciklama { get; set; } = null!;        
        public AvansTuru Tur { get; set; }             
    }
}
