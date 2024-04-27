using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Enums
{
    public enum AvansTuru
    {
        [Display(Name = "Ücret Avansı")]
        UcretAvansi,
        [Display(Name = "İş Avansı")]
        IsAvansi,
        [Display(Name = "Yıllık Ücretli Avans")]
        YillikUcretliAvans
    }
}
