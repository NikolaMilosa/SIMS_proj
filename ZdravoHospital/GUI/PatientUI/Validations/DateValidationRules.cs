using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.PatientUI.Validations
{
    public class DateValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var date = (DateTime)value;
                if (date < DateTime.Now)
                    return new ValidationResult(false, "Please enter an upcoming Date!");

                return new ValidationResult(true, null);
            }
            catch
            {

            }

            return new ValidationResult(false, "Please enter Date in form:");
        }
    }
}
