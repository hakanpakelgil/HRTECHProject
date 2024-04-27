using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace HRTechProject.Entities
{
    public class Sirket : BaseEntity
    {
        [Required(ErrorMessage = "Şirket adı boş bırakılamaz.")]
        public string Name { get; set; } = null!;
        public List<Departman> Departmanlar { get; set; } = [];
    }
}
