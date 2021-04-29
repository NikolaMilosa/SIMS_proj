using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    public class TimeAfterAnotherValidationRule : ValidationRule
    {
        public OtherPassedTimeWrapper Wrapper { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value.ToString().Trim();

            try
            {
                var timeOfDay = TimeSpan.ParseExact(input, "c", null);
                DateTime typedDate = Wrapper.ThisDate.Add(timeOfDay);
                if (Wrapper.OtherPassedTime > typedDate)
                {
                    return new ValidationResult(false, "'Start time' is ahead of 'End time'...");
                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, null);
            }
        }
    }

    public class OtherPassedTimeWrapper : DependencyObject
    {
        public static readonly DependencyProperty OtherPassedTimeProperty = DependencyProperty.Register("OtherPassedTime", typeof(DateTime), typeof(OtherPassedTimeWrapper), null);
    
        public DateTime OtherPassedTime
        {
            get { return (DateTime)GetValue(OtherPassedTimeProperty); }
            set { SetValue(OtherPassedTimeProperty, value); }
        }

        public DateTime ThisDate
        {
            get { return (DateTime)GetValue(ThisDateProperty); }
            set { SetValue(ThisDateProperty, value); }
        }

        public static readonly DependencyProperty ThisDateProperty = DependencyProperty.Register("ThisDate", typeof(DateTime), typeof(OtherPassedTimeWrapper), null);

    }

    public class OtherPassedTimeBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new OtherPassedTimeBindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(OtherPassedTimeBindingProxy), new PropertyMetadata(null));
    }
}
