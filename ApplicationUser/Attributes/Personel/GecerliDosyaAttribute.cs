using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Attributes.Personel
{
	public class GecerliDosyaAttribute : ValidationAttribute
	{
		public double MaxDosyaBoyutuMB { get; set; } = 2;

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var file = (IFormFile?)value;

			if (file == null)
				return ValidationResult.Success;

			if (!file.ContentType.StartsWith("image/") && !file.ContentType.Equals("application/pdf"))
			{
				return new ValidationResult("Geçersiz  dosya yüklediniz.");
			}
			else if (file.Length > MaxDosyaBoyutuMB * 1024 * 1024)
			{
				return new ValidationResult($"Maksimum Dosya Boyutu : {MaxDosyaBoyutuMB} MB");
			}


			return ValidationResult.Success;
		}
	}
}
