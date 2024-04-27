using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Attributes
{
    public class IsTarihAraligiAttribute : ValidationAttribute
    {
        public readonly string _propertyName;

        public IsTarihAraligiAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_propertyName);

            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"'{_propertyName}' adında bir tarih bulunamadı.");
            }

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (value != null && otherPropertyValue != null)
            {
                DateTime iseGirisTarihi = (DateTime)value;
                DateTime istenCikisTarihi = (DateTime)otherPropertyValue;

                if (iseGirisTarihi > istenCikisTarihi)
                {
                    return new ValidationResult(ErrorMessage ?? "İşe giriş tarihi, çıkış tarihinden sonra olamaz.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
