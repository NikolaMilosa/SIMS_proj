using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Model;


namespace ZdravoHospital.GUI.ManagerUI.Converters
{
    class RoomTypeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch ((RoomType)value)
            {
                case RoomType.APPOINTMENT_ROOM:
                    return "APPOINTMENT";
                case RoomType.BREAK_ROOM:
                    return "BEDROOM";
                case RoomType.OPERATING_ROOM:
                    return "OPERATING";
                default:
                    return "STORAGE";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "APPOINTMENT":
                    return RoomType.APPOINTMENT_ROOM;
                case "BEDROOM":
                    return RoomType.BREAK_ROOM;
                case "STORAGE":
                    return RoomType.STORAGE_ROOM;
                default:
                    return RoomType.OPERATING_ROOM;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
