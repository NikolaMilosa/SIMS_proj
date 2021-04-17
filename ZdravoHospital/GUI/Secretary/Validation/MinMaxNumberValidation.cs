using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.Secretary.Validation
{
    public class MinMaxNumberValidation : ValidationRule
    {
        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string text = (string)value;
            if (text.Length == 0)
                return new ValidationResult(false, "");
            int num;
            if (int.TryParse(text, out num))
            {
                if (num < Min) return new ValidationResult(false, "Value too small.");
                if (num > Max) return new ValidationResult(false, "Value too large.");
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, string.Format("Enter valid number in minutes between {0} and {1}.", Min, Max));
            }
        }
    }
}
