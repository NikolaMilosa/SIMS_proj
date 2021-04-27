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

        bool isAdder;
        Window dialog;

        private Logics.InventoryFunctions inventoryFunctions;

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
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string InventoryName
        {
            get { return _name; }
            set
            {
                _name = value;
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

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public InventoryType InventoryType
        {
            get { return _inventoryType; }
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

            this.inventoryFunctions = new Logics.InventoryFunctions();

            isAdder = true;
            this.Title = "Inventory adding dialog";
            TypeComboBox.SelectedIndex = 0;
        }

        public InventoryAddOrEdit(Inventory i)
        {
            InitializeComponent();
            this.DataContext = this;

            this.inventoryFunctions = new Logics.InventoryFunctions();

            this.Title = "Inventory editing dialog";
            isAdder = false;

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
            if (isAdder)
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
            if (!inventoryFunctions.AddInventory(new Inventory(InventoryName, Supplier, Quantity, InventoryType, Id)))
            {
                dialog = new WarningDialog(this);
                dialog.ShowDialog();
                return;
            }
        }

        private void EditInventory()
        {
            Inventory newInventory = new Inventory(InventoryName, Supplier, Quantity, InventoryType, Id);
            inventoryFunctions.EditInventory(Model.Resources.inventory[Id],newInventory);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
