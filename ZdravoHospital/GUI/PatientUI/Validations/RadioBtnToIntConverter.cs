using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ZdravoHospital.GUI.PatientUI.Validations
{
    class RadioBtnToIntConverter : IValueConverter
    {
        public int ReturnValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
          
            return int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                ReturnValue = int.Parse(parameter.ToString());
            }
               
            return ReturnValue;
        }
    }
}
