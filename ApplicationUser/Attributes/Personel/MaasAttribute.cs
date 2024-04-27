using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Attributes.Personel
{
    public class MaasAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            decimal maas;

            if (value == null) return true;

            if (!decimal.TryParse(value.ToString(), out maas))
            {
                ErrorMessage = "Geçersiz değer!";
                return false;
            }

            if (maas < 1) return false;

            return true;
        }
    }
}
