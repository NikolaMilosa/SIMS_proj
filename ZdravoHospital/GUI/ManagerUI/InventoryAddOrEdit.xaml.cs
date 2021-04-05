using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Model;

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for InventoryAddOrEdit.xaml
    /// </summary>
    public partial class InventoryAddOrEdit : Window
    {
        List<InventoryType> inventoryTypes = new List<InventoryType>() { InventoryType.DYNAMIC_INVENTORY, InventoryType.STATIC_INVENTORY };
        bool isAdder;
        Inventory newInventory;

        public InventoryAddOrEdit()
        {
            InitializeComponent();
            Binding binding = new Binding() { Converter = new InventoryTypeConverter() };
            binding.Source = inventoryTypes;
            TypeComboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);

            TypeComboBox.SelectedIndex = 0;
            isAdder = true;
            this.Title = "Inventory adding";
        }

        private void fieldChecker()
        {
            if (NameTextBox.Text.Equals(String.Empty) || SuplierTextBox.Text.Equals(String.Empty) || NameWarningLabel.Visibility == Visibility.Visible || QuantityWarningLabel.Visibility == Visibility.Visible)
                ConfirmButton.IsEnabled = false;
            else
                ConfirmButton.IsEnabled = true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = TypeComboBox.SelectedItem.ToString();
            InventoryType temp;
            if (selectedValue.Equals("STATIC"))
                temp = InventoryType.STATIC_INVENTORY;
            else
                temp = InventoryType.DYNAMIC_INVENTORY;

            if (isAdder)
            {
                newInventory = new Inventory(NameTextBox.Text, SuplierTextBox.Text, Int32.Parse(QuantityTextBox.Text), temp);
                if (!Model.Resources.inventory.ContainsKey(newInventory.Name))
                {
                    Model.Resources.inventory[newInventory.Name] = newInventory;
                    ManagerWindow.oInventory.Add(newInventory);
                    Model.Resources.SerializeInventory();
                    this.Close();
                }
            }
            else
            {
                //Code for edit
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Model.Resources.inventory.ContainsKey(NameTextBox.Text))
            {
                NameWarningLabel.Content = "Item exists!";
                NameWarningLabel.Visibility = Visibility.Visible;
            }
            else
            {
                NameWarningLabel.Visibility = Visibility.Hidden;
            }

            fieldChecker();
        }

        private void SuplierTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fieldChecker();
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int temp;
            if (Int32.TryParse(QuantityTextBox.Text, out temp))
            {
                QuantityWarningLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                if (QuantityTextBox.Text.Length != 0)
                {
                    QuantityTextBox.Text = QuantityTextBox.Text.Substring(0, QuantityTextBox.Text.Length - 1);
                    QuantityWarningLabel.Visibility = Visibility.Visible;
                    QuantityWarningLabel.Content = "- Only digits!";
                }
            }
            fieldChecker();
        }
    }
}
