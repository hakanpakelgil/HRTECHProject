using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Attributes.Personel
{
    public class IzinEskiTarihKontrolAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime tarih;

            DateTime.TryParse(value?.ToString(), out tarih);
            if (tarih <= DateTime.Now.AddDays(-1))
            {
                ErrorMessage = "İzin başlangıç tarihi bugünden önce olamaz!";
                return false;
            }

            return true;
        }
    }
}
