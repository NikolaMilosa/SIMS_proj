using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Model
{
    public class Room
    {
        public RoomType RoomType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }

        public System.Collections.Generic.List<Inventory> Inventory { get; set; }
        public Room(RoomType rt, int id, string name, bool a)
        {
            this.RoomType = rt;
            this.Id = id;
            this.Name = name;
            this.Available = a;
            this.Inventory = new List<Inventory>();
        }

    }

    public class AvailabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "YES";
            return "NO";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals("YES"))
                return true;
            return false;
        }
    }
}