using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

using Model;

namespace ZdravoHospital.GUI.ManagerUI
{
    class RoomIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int id = int.Parse(value.ToString());

                if (Model.Resources.rooms.ContainsKey(id))
                    return new ValidationResult(false, "- Id exists...");

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "- Only digits...");
            }
        }
    }
}
