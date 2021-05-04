using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

using Model;

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for InventoryAddOrEdit.xaml
    /// </summary>
    public partial class InventoryAddOrEdit : Window, INotifyPropertyChanged
    {
        //Fields:
        private string _id;
        private string _name;
        private string _supplier;
        private int _quantity;
        private InventoryType _inventoryType;

        bool _isAdder;
        Window _dialog;

        private Logics.InventoryFunctions _inventoryFunctions;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

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
            get => _name;
            set
            {
                _name = value;
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

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public InventoryType InventoryType
        {
            get => _inventoryType;
            set
            {
                _inventoryType = value;
                OnPropertyChanged("InventoryType");
            }
        }

        public InventoryAddOrEdit()
        {
            InitializeComponent();
            this.DataContext = this;

            this._inventoryFunctions = new Logics.InventoryFunctions();

            _isAdder = true;
            this.Title = "Inventory adding dialog";
            TypeComboBox.SelectedIndex = 0;
        }

        public InventoryAddOrEdit(Inventory i)
        {
            InitializeComponent();
            this.DataContext = this;

            this._inventoryFunctions = new Logics.InventoryFunctions();

            this.Title = "Inventory editing dialog";
            _isAdder = false;

            Id = i.Id;
            InventoryName = i.Name;
            Supplier = i.Supplier;
            Quantity = i.Quantity;
            InventoryType = i.InventoryType;

            IdTextBox.IsEnabled = false;
            QuantityTextBox.IsEnabled = false;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdder)
            {
                AddInventory();
            }
            else
            {
                EditInventory();
            }

            this.Close();
        }

        private void AddInventory()
        {
            if (!_inventoryFunctions.AddInventory(new Inventory(InventoryName, Supplier, Quantity, InventoryType, Id)))
            {
                _dialog = new WarningDialog(this);
                _dialog.ShowDialog();
                return;
            }
        }

        private void EditInventory()
        {
            var newInventory = new Inventory(InventoryName, Supplier, Quantity, InventoryType, Id);
            _inventoryFunctions.EditInventory(Model.Resources.inventory[Id],newInventory);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
