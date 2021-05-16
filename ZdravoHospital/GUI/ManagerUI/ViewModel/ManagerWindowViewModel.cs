using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.Logics;
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
        private static Mutex _inventoryMutex;
        private static Mutex _transferMutex;

        #endregion

        #region Fields

        private string activeManager;
        private Visibility _roomTableVisibility;
        private Visibility _inventoryTableVisibility;
        private Visibility _medicineTableVisibility;

        private Room _selectedRoom;
        private Inventory _selectedInventory;
        private Medicine _selectedMedicine;

        private InventoryManagementDialogViewModel _inventoryManagementDialogViewModel;

        private Window dialog;
        #endregion

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
        public MyICommand AddInventoryCommand { get; set; }
        public MyICommand AddMedicineCommand { get; set; }
        public MyICommand ManageInventoryCommand { get; set; }
        public MyICommand PlanRenovationCommand { get; set; }

        #endregion


        public ManagerWindowViewModel(string au)
        {
            dashboard = this;

            Employee currManager = Resources.findManager(au);
            ActiveManager = "Welcome, " + currManager.Name;

            OpenDataBase();
            SetObservables();
            TurnOffTables();

            ShowRoomCommand = new MyICommand(OnShowRooms);
            ShowInventoryCommand = new MyICommand(OnShowInventory);
            ShowMedicineCommand = new MyICommand(OnShowMedicine);
            AddRoomCommand = new MyICommand(OnAddRoom);
            AddInventoryCommand = new MyICommand(OnAddInventory);
            AddMedicineCommand = new MyICommand(OnAddMedicine);
            ManageInventoryCommand = new MyICommand(OnManageInventory);
            PlanRenovationCommand = new MyICommand(OnPlanRenovation);

            _roomMutex = new Mutex();
            _inventoryMutex = new Mutex();
            _transferMutex = new Mutex();

            _inventoryManagementDialogViewModel = new InventoryManagementDialogViewModel();

            RunAllTasks();
        }

        #region Private functions

        private void OpenDataBase()
        {
            Resources.OpenRooms();
            Resources.OpenInventory();
            Resources.OpenMedicines();
            Resources.OpenRoomInventory();
            Resources.OpenRoomSchedule();
            Resources.OpenTransferRequests();
            Resources.OpenPeriods();
            Resources.OpenMedicineRecensions();
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
        
        private void RunAllTasks()
        {
            var transferFunctions = new TransferRequestsFunctions();
            transferFunctions.RunOrExecute();

            var roomScheduleFunctions = new RoomScheduleFunctions();
            roomScheduleFunctions.RunOrExecute();
        }

        #endregion

        #region Public functions

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

        #region Button Functions

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

        //Add inventory button
        private void OnAddInventory()
        {
            dialog = new AddOrEditInventoryDialog(null);
            dialog.ShowDialog();
        }

        //Add medicine button
        private void OnAddMedicine()
        {
            dialog = new AddOrEditMedicineDialog(null);
            dialog.ShowDialog();
        }

        //Manage inventory button
        private void OnManageInventory()
        {
            _inventoryManagementDialogViewModel.SenderRooms = new ObservableCollection<Room>(Resources.rooms.Values);
            _inventoryManagementDialogViewModel.SenderRoom = null;
            _inventoryManagementDialogViewModel.ReceiverRoom = null;
            dialog = new InventoryManagementDialog(_inventoryManagementDialogViewModel);
            dialog.ShowDialog();
        }

        //Plan renovation button
        private void OnPlanRenovation()
        {
            dialog = new RenovationPlaningDialog();
            dialog.ShowDialog();
        }

        #endregion

        #region Complex Key Handling

        public void HandleEnterClick()
        {
            if (RoomTableVisibility == Visibility.Visible)
            {
                if (SelectedRoom != null)
                    dialog = new AddOrEditRoomDialog(SelectedRoom);
            }
            else if (InventoryTableVisibility == Visibility.Visible)
            {
                if (SelectedInventory != null)
                    dialog = new AddOrEditInventoryDialog(SelectedInventory);
            }
            else if (MedicineTableVisibility == Visibility.Visible)
            {
                if (SelectedMedicine != null)
                    dialog = new AddOrEditMedicineDialog(SelectedMedicine);
            }

            if (dialog != null)
                dialog.ShowDialog();
        }

        public void HandleDeleteClick()
        {
            if (RoomTableVisibility == Visibility.Visible)
            {
                if (SelectedRoom != null)
                    dialog = new WarningDialog(SelectedRoom);
            }
            else if (InventoryTableVisibility == Visibility.Visible)
            {
                if (SelectedInventory != null)
                    dialog = new WarningDialog(SelectedInventory);
                
            }
            else if (MedicineTableVisibility == Visibility.Visible)
            {
                if (SelectedMedicine != null)
                    dialog = new WarningDialog(SelectedMedicine);
            }

            if (dialog != null)
                dialog.ShowDialog();
        }
        #endregion

        #region Events

        public void OnRoomsChanged(object sender, EventArgs e)
        {
            _roomMutex.WaitOne();

            Rooms = new ObservableCollection<Room>(Resources.rooms.Values);
            OnPropertyChanged("Rooms");

            _roomMutex.ReleaseMutex();

            OnRefreshRenovationDialog(sender, e);
        }

        public void OnInventoryChanged(object sender, EventArgs e)
        {
            _inventoryMutex.WaitOne();

            Inventory = new ObservableCollection<Inventory>(Resources.inventory.Values);
            OnPropertyChanged("Inventory");

            _inventoryMutex.ReleaseMutex();
        }

        public void OnMedicineChanged(object sender, EventArgs e)
        {
            Medicines = new ObservableCollection<Medicine>(Resources.medicines);
            OnPropertyChanged("Medicines");
        }

        public void OnRefreshRenovationDialog(object sender, EventArgs e)
        {
            _transferMutex.WaitOne();
            _inventoryManagementDialogViewModel.OnShoudRefresh();
            _transferMutex.ReleaseMutex();
        }

        #endregion
    }
}
