using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.View;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class ManagerWindowViewModel : ViewModel
    {
        /*Singleton*/
        private static ManagerWindowViewModel dashboard;

        public static ManagerWindowViewModel GetDashboard()
        {
            return dashboard;
        }

        #region Mutex

        private static Mutex _roomMutex;

        #endregion

        #region Fields

        private string activeManager;
        private Visibility _roomTableVisibility;
        private Visibility _inventoryTableVisibility;
        private Visibility _medicineTableVisibility;

        private Room _selectedRoom;
        private Inventory _selectedInventory;
        private Medicine _selectedMedicine;
        #endregion
        
        //Dialog
        private Window dialog;

        #region Observable collections
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<Inventory> Inventory { get; set; }
        public ObservableCollection<Medicine> Medicines { get; set; }

        #endregion

        #region Functions and services

        private Logics.TransferRequestsFunctions transferRequestFunctions;
        private Logics.RoomScheduleFunctions roomScheduleFunctions;

        #endregion

        #region Table visibility properties

        public Visibility RoomTableVisibility
        {
            get => _roomTableVisibility;
            set
            {
                _roomTableVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility InventoryTableVisibility
        {
            get => _inventoryTableVisibility;
            set
            {
                _inventoryTableVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility MedicineTableVisibility
        {
            get => _medicineTableVisibility;
            set
            {
                _medicineTableVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion
        
        #region Properties

        public string ActiveManager
        {
            get => activeManager;
            set
            {
                activeManager = value;
                OnPropertyChanged();
            }
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged();
            }
        }

        public Inventory SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged();
            }
        }

        public Medicine SelectedMedicine
        {
            get => _selectedMedicine;
            set
            {
                _selectedMedicine = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand ShowRoomCommand { get; set; }
        public MyICommand ShowInventoryCommand { get; set; }
        public MyICommand ShowMedicineCommand { get; set; }
        public MyICommand AddRoomCommand { get; set; }

        #endregion


        public ManagerWindowViewModel(string au)
        {
            dashboard = this;

            Employee currManager = Resources.findManager(au);
            ActiveManager = currManager.Name;

            OpenDataBase();
            SetObservables();
            TurnOffTables();

            ShowRoomCommand = new MyICommand(OnShowRooms);
            ShowInventoryCommand = new MyICommand(OnShowInventory);
            ShowMedicineCommand = new MyICommand(OnShowMedicine);
            AddRoomCommand = new MyICommand(OnAddRoom);

            _roomMutex = new Mutex();
        }

        #region Private functions

        private void OpenDataBase()
        {
            Resources.OpenRooms();
            Resources.OpenInventory();
            Resources.OpenMedicines();
        }

        private void SetObservables()
        {
            Rooms = new ObservableCollection<Room>(Resources.rooms.Values);
            Inventory = new ObservableCollection<Inventory>(Resources.inventory.Values);
            Medicines = new ObservableCollection<Medicine>(Resources.medicines);
        }

        private void TurnOffTables()
        {
            RoomTableVisibility = Visibility.Hidden;
            InventoryTableVisibility = Visibility.Hidden;
            MedicineTableVisibility = Visibility.Hidden;
        }

        public string FindVisibleDataGrid()
        {
            if (RoomTableVisibility == Visibility.Visible)
                return "roomTable";
            else if (InventoryTableVisibility == Visibility.Visible)
                return "inventoryTable";
            else if (MedicineTableVisibility == Visibility.Visible)
                return "medicineTable";
            else
                return "initialTable";
        }

        #endregion
        
        #region ButtonFunctions

        //Show rooms button
        private void OnShowRooms()
        {
            TurnOffTables();
            RoomTableVisibility = Visibility.Visible;
        }

        //Show inventory button
        private void OnShowInventory()
        {
            TurnOffTables();
            InventoryTableVisibility = Visibility.Visible;
        }

        //Show medicine button
        private void OnShowMedicine()
        {
            TurnOffTables();
            MedicineTableVisibility = Visibility.Visible;
        }

        //Add room button
        private void OnAddRoom()
        {
            dialog = new AddOrEditRoomDialog(null);
            dialog.ShowDialog();
        }

        #endregion

        #region DialogCreation

        public void HandleEnterClick()
        {
            if (RoomTableVisibility == Visibility.Visible)
            {
                if (SelectedRoom != null)
                    dialog = new AddOrEditRoomDialog(SelectedRoom);
            }

            dialog.ShowDialog();
        }

        #endregion

        #region Events

        public void OnRoomsChanged(object sender, ChangedRoomEventArgs e)
        {
            _roomMutex.WaitOne();

            Rooms = new ObservableCollection<Room>(Resources.rooms.Values);
            OnPropertyChanged("Rooms");

            _roomMutex.ReleaseMutex();
        }

        #endregion
    }
}
