using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class QuantityAddSubtractValidationRule : ValidationRule
    {
        public MinInventoryWrapper Wrapper { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var quantity = int.Parse(value.ToString());

                if (quantity < 0)
                {
                    return new ValidationResult(false, "'Quantity' cannot be less than 0...");
                }

                if (quantity < Wrapper.Min)
                {
                    return new ValidationResult(false, "Cannot be less than '" + Wrapper.Min + "' since it's reserved for transferring...");
                }

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "'Quantity' accepts only digits...");
            }
        }
    }

    public class MinInventoryWrapper : DependencyObject
    {
        public static readonly DependencyProperty MinInventoryProperty = DependencyProperty.Register("Min", typeof(int), typeof(MinInventoryWrapper), new FrameworkPropertyMetadata(0));

        public int Min
        {
            get => (int)GetValue(MinInventoryProperty);
            set => SetValue(MinInventoryProperty, value);
        }
    }

    public class MinInventoryBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new MinInventoryBindingProxy();
        }

        public object Data
        {
            get => (object)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(MinInventoryBindingProxy), new PropertyMetadata(null));
    }
}
