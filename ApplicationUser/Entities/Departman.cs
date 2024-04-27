using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;

namespace HRTechProject.Entities
{
    public class Departman : BaseEntity
    {

        [Required(ErrorMessage = "Departman adı boş bırakılamaz.")]
        public string Ad { get; set; } = null!;
        public List<Meslek> Meslekler { get; set; } = [];
        public List<Sirket> Sirketler { get; set; } = [];
    }
}
