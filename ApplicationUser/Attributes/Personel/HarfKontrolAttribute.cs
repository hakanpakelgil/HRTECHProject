using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Attributes.Personel
{
    public class HarfKontrolAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string kelime = (string)value;
            if (kelime == null)
                return true;

            if (!kelime.All(Char.IsLetter))
            {
                ErrorMessage = "Lütfen sadece harf giriniz!";
                return false;
            }

            return true;
        }
    }
}
