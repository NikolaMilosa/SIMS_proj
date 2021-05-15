using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace ZdravoHospital.GUI.Secretary.Converters
{
    public class IconConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int recentActions = (int)value;
            if(recentActions < 5)
            {
                return MaterialDesignThemes.Wpf.PackIconKind.User;
            }
            else
            {
                return MaterialDesignThemes.Wpf.PackIconKind.UserBlock;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
