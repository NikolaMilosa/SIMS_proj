using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Model;


namespace ZdravoHospital.GUI.ManagerUI.Converters
{
    class FeedbackTypeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch ((FeedbackType)value)
            {
                case FeedbackType.FAULT:
                    return "Unexpected behaviour";
                case FeedbackType.QUESTION:
                    return "Question";
                default:
                    return "Improvement";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "Unexpected behaviour":
                    return FeedbackType.FAULT;
                case "Question":
                    return FeedbackType.QUESTION;
                default:
                    return FeedbackType.IMPROVEMENT;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}