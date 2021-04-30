using Model;
using System;
using System.Collections.Generic;
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

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for InventoryAdderSubtractor.xaml
    /// </summary>
    public partial class InventoryAdderSubtractor : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        //Fields 
        private string _selectedInventory;
        private List<Room> _rooms;
        private Room _selectedRoom;
        private string _roomInventoryQuantity;
        private int _enteredQuantity;
        private int _minInventory;

        private Logics.TransferRequestsFunctions _transferRequestsFunctions;
        private Logics.RoomInventoryFunctions _roomInventoryFunctions;
        private Logics.InventoryFunctions _inventoryFunctions;

        public string SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged("SelectedInventory");
            }
        }

        public List<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged("Rooms");
            }
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged("SelectedRoom");

                if (_selectedRoom != null)
                {
                    MinInventory = _transferRequestsFunctions.GetScheduledInventoryForRoom(PassedInventory, _selectedRoom);

                    var roomInventory = _roomInventoryFunctions.FindRoomInventoryByRoomAndInventory(_selectedRoom.Id, PassedInventory.Id);

                    if (PassedInventory.InventoryType == InventoryType.DYNAMIC_INVENTORY && roomInventory != null)
                    {
                        RoomInventoryQuantity = roomInventory.Quantity.ToString();
                    }
                    else if (PassedInventory.InventoryType == InventoryType.STATIC_INVENTORY && roomInventory != null)
                    {
                        if (MinInventory != 0)
                            RoomInventoryQuantity = ((new StringBuilder()).Append(roomInventory.Quantity.ToString()).Append(" (")
                                                                                                                .Append(MinInventory)
                                                                                                                .Append(")")).ToString();
                        else
                            RoomInventoryQuantity = roomInventory.Quantity.ToString();
                    }
                    else
                    {
                        RoomInventoryQuantity = "0";
                    }

                    EnteredQuantity = 0;
                }
            }
        }

        public string RoomInventoryQuantity
        {
            get => _roomInventoryQuantity;
            set
            {
                _roomInventoryQuantity = value;
                OnPropertyChanged("RoomInventoryQuantity");
            }
        }

        public int EnteredQuantity
        {
            get => _enteredQuantity;
            set
            {
                _enteredQuantity = value;
                OnPropertyChanged("EnteredQuantity");
            }
        }

        public int MinInventory
        {
            get => _minInventory;
            set
            {
                _minInventory = value;
                OnPropertyChanged("MinInventory");
            }
        }

        public Inventory PassedInventory { get; set; }

        public InventoryAdderSubtractor(Inventory inventory)
        {
            InitializeComponent();
            this.DataContext = this;

            this._transferRequestsFunctions = new Logics.TransferRequestsFunctions();
            this._roomInventoryFunctions = new Logics.RoomInventoryFunctions();
            this._inventoryFunctions = new Logics.InventoryFunctions();

            this.Rooms = new List<Room>(Model.Resources.rooms.Values);
            this.PassedInventory = inventory;

            this.SelectedInventory = ((new StringBuilder()).Append(PassedInventory.Id).Append(" - ").Append(PassedInventory.Name)).ToString();
        }

        private void RoomComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.Left)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                if (RoomComboBox.IsDropDownOpen == false)
                    RoomComboBox.IsDropDownOpen = true;
                else
                    RoomComboBox.IsDropDownOpen = false;
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                if (RoomComboBox.IsDropDownOpen == true)
                {
                    if (RoomComboBox.SelectedIndex + 1 < RoomComboBox.Items.Count)
                        RoomComboBox.SelectedIndex += 1;
                }
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                if (RoomComboBox.IsDropDownOpen == true)
                {
                    if (RoomComboBox.SelectedIndex - 1 >= 0)
                        RoomComboBox.SelectedIndex -= 1;
                }
                e.Handled = true;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            _inventoryFunctions.EditInventoryAmount(PassedInventory, EnteredQuantity, SelectedRoom);
            this.Close();
            }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
