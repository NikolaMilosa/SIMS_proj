using System;
using System.Globalization;
using System.Windows.Data;

namespace ZdravoHospital.GUI.PatientUI.Converters
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
