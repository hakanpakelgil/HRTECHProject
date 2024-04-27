using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Attributes.Personel
{
    public class PersonelEskiTarihKontrolAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime tarih;

            DateTime.TryParse(value?.ToString(), out tarih);
            if (tarih < DateTime.Now.AddYears(-50))
            {
                ErrorMessage = "Eski tarih girdiniz!";
                return false;
            }

            return true;
        }
    }
}
