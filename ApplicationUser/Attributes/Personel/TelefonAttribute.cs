using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Attributes.Personel
{
    public class TelefonAttribute : ValidationAttribute
    {
        public TelefonAttribute()
        {
            ErrorMessage = "Geçersiz numara!";
        }
        public override bool IsValid(object? value)
        {
            int sayac = 0;

            if (value == null) return true;

            string? telefon = value.ToString();

            for (int i = 0; i < telefon?.Length; i++)
            {
                if (char.IsDigit(telefon[i]) || telefon[i] == '+' || telefon[i] == ' ' && telefon[i - 1] != ' ')
                {
                    sayac += 1;
                }
            }

            if (sayac < telefon?.Length)
            {
                ErrorMessage = "Karakter türüne veya boşluklara dikkat edin!";
                return false;
            }

            return true;
        }
    }
}
