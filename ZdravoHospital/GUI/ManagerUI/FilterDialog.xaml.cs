using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
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
    /// Interaction logic for FilterDialog.xaml
    /// </summary>
    public partial class FilterDialog : Window, INotifyPropertyChanged
    {
        #region fields
        //Fields:
        private string _id;
        private string _inventoryName;
        private string _supplier;
        private string _quantity;
        private string _type;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string InventoryName
        {
            get => _inventoryName;
            set
            {
                _inventoryName = value;
                OnPropertyChanged("InventoryName");
            }
        }

        public string Supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                OnPropertyChanged("Supplier");
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                string[] parts = value.ToString().Split(" ");
                _type = parts[1];
                OnPropertyChanged("Type");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion fields

        public FilterDialog()
        {
            InitializeComponent();
            this.DataContext = this;

            Id = "";
            InventoryName = "";
            Supplier = "";
            Quantity = "";
            Type = " BOTH";
            TypeComboBox.SelectedIndex = 0;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsVisual = CollectionViewSource.GetDefaultView(ManagerWindow.Inventory);

            itemsVisual.Filter = InventoryFilter;

            this.Close();
        }

        private bool InventoryFilter(object item)
        {
            var inventory = item as Inventory;

            if (!Supplier.Trim().Equals(string.Empty))
                Supplier = Supplier.Trim().Substring(0, 1).ToUpper() + Supplier.Trim().Substring(1).ToLower();
            else
                Supplier = Supplier.Trim();

            int enteredInv;

            if (Quantity.Trim().Equals(string.Empty))
                enteredInv = int.MaxValue;
            else
                enteredInv = int.Parse(Quantity);

            if (inventory.Id.Contains(Id.Trim().ToUpper()) && 
                inventory.Name.Contains(InventoryName.Trim().ToLower()) && 
                inventory.Supplier.Contains(Supplier) &&
                inventory.Quantity <= enteredInv && 
                ((Type.Equals("STATIC") && inventory.InventoryType == InventoryType.STATIC_INVENTORY) || 
                (Type.Equals("DYNAMIC") && inventory.InventoryType == InventoryType.DYNAMIC_INVENTORY) ||
                (Type.Equals("BOTH"))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TypeComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TypeComboBox.IsDropDownOpen == false)
                    TypeComboBox.IsDropDownOpen = true;
                else
                    TypeComboBox.IsDropDownOpen = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (TypeComboBox.IsDropDownOpen == true)
                {
                    if (TypeComboBox.SelectedIndex + 1 < TypeComboBox.Items.Count)
                        TypeComboBox.SelectedIndex += 1;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (TypeComboBox.IsDropDownOpen == true)
                {
                    if (TypeComboBox.SelectedIndex - 1 >= 0)
                        TypeComboBox.SelectedIndex -= 1;
                }
                e.Handled = true;
            }
        }
    }
}
