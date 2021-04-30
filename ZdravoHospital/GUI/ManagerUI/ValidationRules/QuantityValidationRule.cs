using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    public class QuantityValidationRule : ValidationRule
    {
        public MaxInventoryWrapper Wrapper { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            try
            {
                var quantity = int.Parse(value.ToString());

                if (quantity < 1)
                    return new ValidationResult(false, "'Quantity' needs to be at least 1 or more...");

                if (Wrapper != null)
                {
                    if (quantity > Wrapper.Max)
                        return new ValidationResult(false, "There isn't that much inventory of that type in the room...");
                }

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "'Quantity' accepts only digits...");
            }
        }
    }

    public class MaxInventoryWrapper : DependencyObject
    {
        public static readonly DependencyProperty MaxInventoryProperty = DependencyProperty.Register("Max", typeof(int), typeof(MaxInventoryWrapper), new FrameworkPropertyMetadata(0));

        public int Max
        {
            get { return (int)GetValue(MaxInventoryProperty); }
            set
            {
                SetValue(MaxInventoryProperty, value);
            }
        }
    }

    public class MaxInventoryBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new MaxInventoryBindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(MaxInventoryBindingProxy), new PropertyMetadata(null));
    }
}
