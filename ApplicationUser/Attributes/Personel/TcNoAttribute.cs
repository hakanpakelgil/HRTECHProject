using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Attributes.Personel
{
    public class TcNoAttribute : ValidationAttribute
    {
        public TcNoAttribute()
        {
            ErrorMessage = "Geçersiz Tc kimlik numarası!";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;

            string? tcNumber = value.ToString();

            if (tcNumber?.Length != 11) return false;

            for (int i = 0; i < 11; i++)
            {
                if (!char.IsDigit(tcNumber[i]))
                {
                    return false;
                }
            }

            if (tcNumber[0] == '0') return false;

            int[] tcNumberArray = new int[11];

            for (int i = 0; i < 11; i++)
            {
                tcNumberArray[i] = int.Parse(tcNumber[i].ToString());
            }
            int sumFirstTenDigits = tcNumberArray[0] + tcNumberArray[1] + tcNumberArray[2] + tcNumberArray[3] + tcNumberArray[4] + tcNumberArray[5] + tcNumberArray[6] + tcNumberArray[7] + tcNumberArray[8] + tcNumberArray[9];
            int controlDigit = sumFirstTenDigits % 10;
            if (controlDigit != tcNumberArray[10])
            {
                return false;
            }

            return true;
        }
    }
}

