using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace ZdravoHospital.GUI.ManagerUI.Converters
{
    class BasicTimeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;

            StringBuilder str = new StringBuilder();
            str.Append(date.Day);
            str.Append("/");
            str.Append(date.Month);
            str.Append("/");
            str.Append(date.Year);
            str.Append("  ");
            str.Append(date.Hour);
            str.Append(":");
            str.Append(date.Minute);

            return str.ToString();
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
