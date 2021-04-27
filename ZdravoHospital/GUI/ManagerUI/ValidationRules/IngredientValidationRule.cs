using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class IngredientValidationRule : ValidationRule
    {
        public IngredientNameWrapper Wrapper { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim().ToLower();

            if (input.Equals(String.Empty))
            {
                return new ValidationResult(false, "Ingredient name cannot be empty...");
            }

            if (!Regex.IsMatch(input, @"^([a-z]+(\s[a-z]+)*)$"))
            {
                return new ValidationResult(false, "You have typed an unsupported character...");
            }

            Ingredient checker = Wrapper.ExistingNames.Find(i => i.IngredientName.Equals(input));
            if (checker == null)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Ingredient with that name already exists...");
            }
        }
    }

    public class IngredientNameWrapper : DependencyObject
    {
        public static readonly DependencyProperty IngredientsProperty = DependencyProperty.Register("ExistingNames", typeof(List<Ingredient>), typeof(IngredientNameWrapper), new FrameworkPropertyMetadata(null));
    
        public List<Ingredient> ExistingNames
        {
            get { return (List<Ingredient>)GetValue(IngredientsProperty); }
            set
            {
                SetValue(IngredientsProperty, value);
            }
        }
    }

    public class IngredientNameBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new IngredientNameBindingProxy();
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(IngredientNameBindingProxy), new PropertyMetadata(null));

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }
}
