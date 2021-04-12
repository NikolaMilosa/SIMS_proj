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
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdder)
            {
                if (Model.Resources.rooms.Count == 0)
                {
                    dialog = new WarningDialog(this);
                    dialog.ShowDialog();
                    return;
                }

                Model.Resources.inventory[Id] = new Inventory(InventoryName, Supplier, Quantity, InventoryType, Id);
                ManagerWindow.Inventory.Add(Model.Resources.inventory[Id]);

                //If adding there cannot be the same instance of Inventory in the system meaning :
                foreach (Room room in Model.Resources.rooms.Values)
                {
                    if (room.RoomType == RoomType.STORAGE_ROOM)
                    {
                        room.Inventory[Id] = Quantity;
                        Model.Resources.SerializeRooms();
                        Model.Resources.SerializeInventory();
                        this.Close();
                        return;
                    }
                }

                foreach (Room room in Model.Resources.rooms.Values)
                {
                    if (room.RoomType == RoomType.BREAK_ROOM)
                    {
                        room.Inventory[Id] = Quantity;
                        Model.Resources.SerializeRooms();
                        Model.Resources.SerializeInventory();
                        this.Close();
                        return;
                    }
                }

                foreach (Room room in Model.Resources.rooms.Values)
                {
                    if (room.RoomType == RoomType.APPOINTMENT_ROOM || room.RoomType == RoomType.OPERATING_ROOM)
                    {
                        room.Inventory[Id] = Quantity;
                        Model.Resources.SerializeRooms();
                        Model.Resources.SerializeInventory();
                        this.Close();
                        return;
                    }
                }
            }
            else
            {
                int index = ManagerWindow.Inventory.IndexOf(Model.Resources.inventory[Id]);

                Model.Resources.inventory[Id].Name = InventoryName;
                Model.Resources.inventory[Id].Supplier = Supplier;
                Model.Resources.inventory[Id].InventoryType = InventoryType;

                //Calculate the difference:
                int difference = Model.Resources.inventory[Id].Quantity - Quantity;

                if (difference > 0)
                {
                    foreach (Room room in Model.Resources.rooms.Values)
                    {
                        if (room.RoomType == RoomType.STORAGE_ROOM)
                        {
                            if (room.Inventory[Id] - difference > 0)
                            {
                                room.Inventory[Id] -= difference;

                                Model.Resources.inventory[Id].Quantity = Quantity;
                                ManagerWindow.Inventory.Remove(ManagerWindow.Inventory[index]);
                                ManagerWindow.Inventory.Insert(index, Model.Resources.inventory[Id]);

                                Model.Resources.SerializeRooms();
                                Model.Resources.SerializeInventory();
                                this.Close();
                                return;
                            }

                            difference -= room.Inventory[Id];
                            room.Inventory.Remove(Id);
                        }
                    }

                    foreach (Room room in Model.Resources.rooms.Values)
                    {
                        if (room.RoomType == RoomType.BREAK_ROOM)
                        {
                            if (room.Inventory[Id] - difference > 0)
                            {
                                room.Inventory[Id] -= difference;

                                Model.Resources.inventory[Id].Quantity = Quantity;
                                ManagerWindow.Inventory.Remove(ManagerWindow.Inventory[index]);
                                ManagerWindow.Inventory.Insert(index, Model.Resources.inventory[Id]);

                                Model.Resources.SerializeRooms();
                                Model.Resources.SerializeInventory();
                                this.Close();
                                return;
                            }

                            difference -= room.Inventory[Id];
                            room.Inventory.Remove(Id);
                        }
                    }

                    foreach (Room room in Model.Resources.rooms.Values)
                    {
                        if (room.RoomType == RoomType.APPOINTMENT_ROOM || room.RoomType == RoomType.OPERATING_ROOM)
                        {
                            if (room.Inventory[Id] - difference > 0)
                            {
                                room.Inventory[Id] -= difference;

                                Model.Resources.inventory[Id].Quantity = Quantity;
                                ManagerWindow.Inventory.Remove(ManagerWindow.Inventory[index]);
                                ManagerWindow.Inventory.Insert(index, Model.Resources.inventory[Id]);

                                Model.Resources.SerializeRooms();
                                Model.Resources.SerializeInventory();
                                this.Close();
                                return;
                            }

                            difference -= room.Inventory[Id];
                            room.Inventory.Remove(Id);
                        }
                    }
                }
                else if (difference < 0)
                {
                    difference += -2 * difference;
                    foreach (Room room in Model.Resources.rooms.Values)
                    {
                        if (room.RoomType == RoomType.STORAGE_ROOM)
                        {
                            if (room.Inventory.ContainsKey(Id))
                            {
                                room.Inventory[Id] += difference;

                                Model.Resources.inventory[Id].Quantity = Quantity;
                                ManagerWindow.Inventory.Remove(ManagerWindow.Inventory[index]);
                                ManagerWindow.Inventory.Insert(index, Model.Resources.inventory[Id]);

                                Model.Resources.SerializeRooms();
                                Model.Resources.SerializeInventory();

                                this.Close();
                                return;
                            }

                        }

                    }

                    foreach (Room room in Model.Resources.rooms.Values)
                    {
                        if (room.RoomType == RoomType.BREAK_ROOM)
                        {
                            if (room.Inventory.ContainsKey(Id))
                            {
                                room.Inventory[Id] += difference;

                                Model.Resources.inventory[Id].Quantity = Quantity;
                                ManagerWindow.Inventory.Remove(ManagerWindow.Inventory[index]);
                                ManagerWindow.Inventory.Insert(index, Model.Resources.inventory[Id]);

                                Model.Resources.SerializeRooms();
                                Model.Resources.SerializeInventory();

                                this.Close();
                                return;
                            }

                        }

                    }

                    foreach (Room room in Model.Resources.rooms.Values)
                    {
                        if (room.RoomType == RoomType.APPOINTMENT_ROOM || room.RoomType == RoomType.OPERATING_ROOM)
                        {
                            if (room.Inventory.ContainsKey(Id))
                            {
                                room.Inventory[Id] += difference;

                                Model.Resources.inventory[Id].Quantity = Quantity;
                                ManagerWindow.Inventory.Remove(ManagerWindow.Inventory[index]);
                                ManagerWindow.Inventory.Insert(index, Model.Resources.inventory[Id]);

                                Model.Resources.SerializeRooms();
                                Model.Resources.SerializeInventory();

                                this.Close();
                                return;
                            }

                        }

                    }
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
