using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class TimeInputValidationRule : ValidationRule
    {

        public PassedTimeWrapper Wrapper { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value.ToString().Trim();

            if (input.Equals(String.Empty))
                return new ValidationResult(false, "'Time' cannot be left empty...");

            if(input.IndexOf("-") != -1 || input.IndexOf(".") != -1 || input.IndexOf(":") == -1)
                return new ValidationResult(false, "The correct format is 'HH:mm(:ss)' \n eg 19:45:13");

            try
            {
                var timeOfDay = TimeSpan.ParseExact(input, "c", null);
                if(Wrapper.PassedTime == DateTime.Today)
                {
                    if(timeOfDay <= DateTime.Now.TimeOfDay)
                    {
                        return new ValidationResult(false, "Time you typed has already passed...");
                    }
                }
            }
            catch
            {
                return new ValidationResult(false, "The correct format is 'HH:mm(:ss)' \n eg 19:45:13");
            }

            return new ValidationResult(true, null);
        }
    }

    class PassedTimeWrapper : DependencyObject
    {
        public static readonly DependencyProperty PassedTimeProeprty = DependencyProperty.Register("PassedTime", typeof(DateTime), typeof(PassedTimeWrapper), null);
        
        public DateTime PassedTime
        {
            get { return (DateTime)GetValue(PassedTimeProeprty); }
            set { SetValue(PassedTimeProeprty, value); }
        }
    }

    public class PassedTimeBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new PassedTimeBindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(PassedTimeBindingProxy), new PropertyMetadata(null));
    }
}
