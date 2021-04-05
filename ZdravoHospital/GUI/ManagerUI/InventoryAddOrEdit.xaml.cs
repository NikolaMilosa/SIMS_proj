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

        static readonly Regex reg = new Regex("[0-9]*$");

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
            if (NameTextBox.Text.Equals(String.Empty) || SuplierTextBox.Text.Equals(String.Empty) || WarningLabel.Visibility == Visibility.Visible)
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
                WarningLabel.Content = "Item exists!";
                WarningLabel.Visibility = Visibility.Visible;
            }
            else
            {
                WarningLabel.Visibility = Visibility.Hidden;
            }

            fieldChecker();
        }

        private void SuplierTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fieldChecker();
        }

        private void QuantityTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!reg.IsMatch(QuantityTextBox.Text))
                e.Handled = true;
            fieldChecker();
        }
    }
}
