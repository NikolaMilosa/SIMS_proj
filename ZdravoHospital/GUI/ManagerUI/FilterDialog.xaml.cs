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
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string InventoryName
        {
            get { return _inventoryName; }
            set
            {
                _inventoryName = value;
                OnPropertyChanged("InventoryName");
            }
        }

        public string Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                OnPropertyChanged("Supplier");
            }
        }

        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
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
            Type = "BOTH";
            TypeComboBox.SelectedIndex = 0;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            /* Nothing is entered */
            if (Id.Trim().Equals(String.Empty) && InventoryName.Trim().Equals(String.Empty) && Supplier.Trim().Equals(String.Empty) &&
                Quantity.Trim().Equals(String.Empty) && Type.Equals("BOTH"))
            {
                if (ManagerWindow.Inventory.Count != Model.Resources.inventory.Count)
                {
                    ManagerWindow.Inventory.Clear();
                    foreach (Inventory i in Model.Resources.inventory.Values)
                    {
                        ManagerWindow.Inventory.Add(i);
                    }
                }
                this.Close();
            }

            ManagerWindow.Inventory.Clear();

            foreach (Inventory i in Model.Resources.inventory.Values)
            {
                if (!Id.Trim().Equals(String.Empty))
                {
                    if (i.Id.IndexOf(Id) == -1)
                    {
                        continue;
                    }
                }

                if (!InventoryName.Trim().Equals(String.Empty))
                {
                    if (i.Name.IndexOf(InventoryName) == -1)
                    {
                        continue;
                    }
                }

                if (!Supplier.Trim().Equals(String.Empty))
                {
                    if (i.Supplier.IndexOf(Supplier) == -1)
                    {
                        continue;
                    }
                }

                if (!Quantity.Trim().Equals(String.Empty))
                {
                    int enteredQuantity = int.Parse(Quantity);
                    if (i.Quantity > enteredQuantity)
                    {
                        continue;
                    }
                }

                if (!Type.Equals("BOTH"))
                {
                    if ((Type.Equals("STATIC") && i.InventoryType == InventoryType.DYNAMIC_INVENTORY) ||
                        (Type.Equals("DYNAMIC") && i.InventoryType == InventoryType.STATIC_INVENTORY))
                    {
                        continue;
                    }
                }

                ManagerWindow.Inventory.Add(i);
            }

            this.Close();
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
