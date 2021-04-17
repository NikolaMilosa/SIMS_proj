using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.Secretary.Validation
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var email = value as string;
                if(email.Length == 0)
                    return new ValidationResult(true, null);
                
                MailAddress m = new MailAddress(email);
                return new ValidationResult(true, null);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Invalid email format.");
            }
        }
    }
}
