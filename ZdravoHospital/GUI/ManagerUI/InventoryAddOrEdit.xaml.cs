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

            isAdder = true;
            this.Title = "Inventory adding dialog";
            TypeComboBox.SelectedIndex = 0;
        }

        public InventoryAddOrEdit(Inventory i)
        {
            InitializeComponent();
            this.DataContext = this;

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
            /* TODO : */
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
            Room someRoom = FindRoomByPrio();

            if (someRoom == null)
            {
                dialog = new WarningDialog(this);
                dialog.ShowDialog();
                return;
            }

            /* Has where to input inventory */
            Model.Resources.inventory[Id] = new Inventory(InventoryName, Supplier, Quantity, InventoryType, Id);
            Model.Resources.roomInventory.Add(new RoomInventory(Id,someRoom.Id,Quantity));

            Model.Resources.SerializeInventory();
            Model.Resources.SerializeRoomInventory();

            ManagerWindow.Inventory.Add(Model.Resources.inventory[Id]);
        }

        private void EditInventory()
        {
            int index = ManagerWindow.Inventory.IndexOf(Model.Resources.inventory[Id]);
            /* Visual changing */
            ManagerWindow.Inventory.Remove(Model.Resources.inventory[Id]);

            /* Basic fields */

            Model.Resources.inventory[Id].Name = InventoryName;
            Model.Resources.inventory[Id].Supplier = Supplier;
            Model.Resources.inventory[Id].InventoryType = InventoryType;

            /* Visual changing */
            ManagerWindow.Inventory.Insert(index,Model.Resources.inventory[Id]);

            Model.Resources.SerializeInventory();
            Model.Resources.SerializeRoomInventory();
        }

        private Room FindRoomByPrio()
        {
            return Logics.RoomFunctions.FindRoomByPrio();
        }

        private Room FindRoomByType(RoomType rt)
        {
            return Logics.RoomFunctions.FindRoomByType(rt);
        }

        private RoomInventory FindRoomInventoryByRoomAndInventory(int roomId, string inventoryId)
        {
            return Logics.RoomInventoryFunctions.FindRoomInventoryByRoomAndInventory(roomId, inventoryId);
        }

        private List<RoomInventory> FindAllRoomsWithInventory(string inventoryId)
        {
            return Logics.RoomInventoryFunctions.FindAllRoomsWithInventory(inventoryId);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
