using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Converters
{
    class ReservationTypeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ReservationType)value == ReservationType.RENOVATION)
                return "[ RENOVATION  ]";
            if ((ReservationType)value == ReservationType.APPOINTMENT)
                return "[ APPOINTMENT ]";
            if ((ReservationType)value == ReservationType.OPERATION)
                return "[ OPERATION   ]";

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
