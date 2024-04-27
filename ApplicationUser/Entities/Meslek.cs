using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace HRTechProject.Entities
{
    public class Meslek : BaseEntity
    {
        [Required(ErrorMessage = "Meslek adı boş bırakılamaz.")]
        public string Ad { get; set; } = null!;
        public List<ApplicationUser> ApplicationUsers { get; set; } = [];
        public List<Departman> Departmanlar { get; set; } = [];
    }
}