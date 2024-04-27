using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Attributes.Personel
{
    public class DogumTarihiAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime tarih;

            DateTime.TryParse(value.ToString(), out tarih);
            if (tarih < DateTime.Now.AddYears(-100))
            {
                ErrorMessage = "Eski tarih girdiniz!";
                return false;
            }

            if (tarih > DateTime.Now.AddYears(-18) && tarih < DateTime.Now)
            {
                ErrorMessage = "Personel 18 yaşından küçük!";
                return false;
            }
            if (tarih > DateTime.Now)
            {
                ErrorMessage = "İleri tarih girdiniz!";
                return false;
            }

            return true;
        }
    }
}
