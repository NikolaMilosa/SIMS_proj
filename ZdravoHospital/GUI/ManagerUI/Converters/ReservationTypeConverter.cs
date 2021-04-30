using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using Model;

namespace ZdravoHospital.GUI.ManagerUI.Converters
{
    class ReservationTypeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ReservationType)value == ReservationType.RENOVATION)
                return "Type: RENOVATION  ";
            if ((ReservationType)value == ReservationType.APPOINTMENT)
                return "Type: APPOINTMENT ";
            if ((ReservationType)value == ReservationType.OPERATION)
                return "Type: OPERATION   ";
            if ((ReservationType)value == ReservationType.TRANSFER)
                return "Type: TRANSFER    ";

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
