using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class BasicValidation
    {
        public static bool IsTimeFromTextFormatValid(string text)
        {
            Regex regex = new Regex(@"^\d{2}:\d{2}$");
            return regex.IsMatch(text);
        }

        public static bool IsTimeFromTextValueValid(string text)
        {
            string[] parts = text.Split(':');
            int hours = Int32.Parse(parts[0]);
            int minutes = Int32.Parse(parts[1]);
            return hours <= 24 && minutes <= 59;
        }

        public static bool IsIntegerFromTextValid(string text)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(text);
        }
    }
}
