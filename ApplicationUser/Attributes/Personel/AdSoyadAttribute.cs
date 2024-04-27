using System.ComponentModel.DataAnnotations;

namespace Ornek.Attributes
{
    public class AdSoyadAttribute : ValidationAttribute
    {
        public AdSoyadAttribute()
        {
            ErrorMessage = "Ad/Soyad sadece harf içermelidir";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;

            string? kelime = value.ToString();

            for (int i = 0; i < kelime?.Length-1; i++)
            {
                if (!char.IsLetter(kelime[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
