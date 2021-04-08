using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Model
{
    public enum InventoryType
    {
        STATIC_INVENTORY,
        DYNAMIC_INVENTORY
    }

    public class InventoryTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type valueType = value.GetType();
            if(valueType.Name == typeof(List<>).Name)
            {
                List<string> ret = new List<string>() { "DINAMIC", "STATIC" };
                return ret;
            }
            if ((InventoryType)value == InventoryType.STATIC_INVENTORY)
                return "STATIC";
            return "DYNAMIC";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals("STATIC"))
                return InventoryType.STATIC_INVENTORY;
            return InventoryType.DYNAMIC_INVENTORY;
        }
    }
}