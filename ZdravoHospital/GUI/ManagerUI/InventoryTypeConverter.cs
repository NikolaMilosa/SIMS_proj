using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Model;

namespace ZdravoHospital.GUI.ManagerUI
{
    class InventoryTypeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
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

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

