using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Model
{
    public enum RoomType
    {
        APPOINTMENT_ROOM,
        OPERATING_ROOM,
        BREAK_ROOM,
        STORAGE_ROOM
    }

    public class RoomTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type ValueType = value.GetType();
            if (ValueType.Name == typeof(List<>).Name)
            {
                List<string> temp = new List<string>() { "APPOINTMENT", "BEDROOM", "OPERATING", "STORAGE" };
                return temp;
            }
            else
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
    }
}