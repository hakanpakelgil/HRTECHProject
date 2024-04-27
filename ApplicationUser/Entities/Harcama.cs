using ApplicationCore.Enums;
using HRTechProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
	public class Harcama : BaseEntity
	{
		public HarcamaTuru Tur { get; set; }
        public decimal Tutar { get; set; }
        public ParaBirimi ParaBirimi { get; set; }
        public IzinDurumu Durum { get; set; }
        public DateTime TalepTarihi { get; set; }
        public DateTime? CevapTarihi { get; set; }
        public string? Dosya { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
