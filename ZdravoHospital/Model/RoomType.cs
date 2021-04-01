using System;
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
            switch ((RoomType)value)
            {
                case RoomType.APPOINTMENT_ROOM:
                    return "APPOINTMENT ROOM";
                case RoomType.BREAK_ROOM:
                    return "BEDROOM";
                case RoomType.OPERATING_ROOM:
                    return "OPERATING ROOM";
                default:
                    return "STORAGE";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "APPOINTMENT ROOM":
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