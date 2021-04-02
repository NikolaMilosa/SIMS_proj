using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace ZdravoHospital
{
    class DateTimeFormatter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime && (DateTime)value < new DateTime(2, 1, 1))
            {
                return "";
            }
            else
            {
                var date = (DateTime)value;
                return date.ToShortDateString();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as string;
            DateTime ret = DateTime.Today;
            if (DateTime.TryParse(v, out ret))
            {
                return ret;
            }
            else
            {
                return value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
