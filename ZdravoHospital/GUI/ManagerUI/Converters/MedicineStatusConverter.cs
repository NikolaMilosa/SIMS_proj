using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.Converters
{
    class MedicineStatusConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((MedicineStatus)value)
            {
                case MedicineStatus.APPROVED:
                    return "APPROVED";
                case MedicineStatus.PENDING:
                    return "PENDING";
                case MedicineStatus.REJECTED:
                    return "REJECTED";
                default:
                    return "STAGED";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "APPROVED":
                    return MedicineStatus.APPROVED;
                case "PENDING":
                    return MedicineStatus.PENDING;
                case "REJECTED":
                    return MedicineStatus.REJECTED;
                default:
                    return MedicineStatus.STAGED;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
