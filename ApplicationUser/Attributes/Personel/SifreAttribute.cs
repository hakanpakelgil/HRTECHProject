using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationCore.Attributes.Personel
{
    public class SifreAttribute :ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
                return false;

            // Şifre en az 8 karakter olmalıdır
            if (password.Length < 8)
                return false;

            // Şifre en az 1 rakam içermelidir
            if (!Regex.IsMatch(password, @"\d"))
                return false;

            // Şifre en az 1 büyük harf içermelidir
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // Diğer kriterleri sağlayan bir şifre geçerlidir
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return "Şifre en az 8 karakter uzunluğunda olmalı, en az bir rakam ve en az bir büyük harf içermelidir.";
        }
    }
}
