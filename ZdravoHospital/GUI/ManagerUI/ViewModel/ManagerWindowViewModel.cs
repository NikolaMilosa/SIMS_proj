using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Model;
using Newtonsoft.Json;
using Repository.EmployeePersistance;
using Repository.InventoryPersistance;
using Repository.MedicinePersistance;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.ValidationRules;
using ZdravoHospital.GUI.ManagerUI.View;
using ZdravoHospital.Services.Manager;
using EmployeeRepository = Repository.EmployeePersistance.EmployeeRepository;
using InventoryRepository = Repository.InventoryPersistance.InventoryRepository;
using MedicineRepository = Repository.MedicinePersistance.MedicineRepository;
using RoomRepository = Repository.RoomPersistance.RoomRepository;

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

        private FilteringEventArgs _passedArgs;

        private IRoomRepository _roomRepository;
        private IInventoryRepository _inventoryRepository;
        private IMedicineRepository _medicineRepository;
        private IEmployeeRepository _employeeRepository;

        #endregion

        #region Observable collections
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<Inventory> Inventory { get; set; }
        public ObservableCollection<Medicine> Medicines { get; set; }

        #endregion

        #region Functions and services

        private TransferRequestService transferRequestService;
        private RoomScheduleService roomScheduleService;

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


        public ManagerWindowViewModel(string activeUser)
        {
            dashboard = this;
            _employeeRepository = new EmployeeRepository();
            var currManager = _employeeRepository.GetById(activeUser);
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
            _roomRepository = new RoomRepository();
            _inventoryRepository = new InventoryRepository();
            _medicineRepository = new MedicineRepository();
        }

        private void SetObservables()
        {
            Rooms = new ObservableCollection<Room>(_roomRepository.GetValues());
            Inventory = new ObservableCollection<Inventory>(_inventoryRepository.GetValues());
            Medicines = new ObservableCollection<Medicine>(_medicineRepository.GetValues());
        }

        private void TurnOffTables()
        {
            RoomTableVisibility = Visibility.Hidden;
            InventoryTableVisibility = Visibility.Hidden;
            MedicineTableVisibility = Visibility.Hidden;
        }
        
        private void RunAllTasks()
        {
            var transferFunctions = new TransferRequestService();
            transferFunctions.RunOrExecute();

            var roomScheduleFunctions = new RoomScheduleService();
            roomScheduleFunctions.RunOrExecute();
        }

        private bool InventoryFilter(object item)
        {
            var inventory = item as Inventory;

            if (inventory.Id.Contains(_passedArgs.Id) &&
                inventory.Name.Contains(_passedArgs.InventoryName) &&
                inventory.Supplier.Contains(_passedArgs.Supplier) &&
                inventory.Quantity <= _passedArgs.Quantity &&
                ((_passedArgs.Type.Equals("STATIC") && inventory.InventoryType == InventoryType.STATIC_INVENTORY) ||
                 (_passedArgs.Type.Equals("DYNAMIC") && inventory.InventoryType == InventoryType.DYNAMIC_INVENTORY) ||
                 (_passedArgs.Type.Equals("BOTH"))))
            {
                return true;
            }
            else
            {
                return false;
            }

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
            _inventoryManagementDialogViewModel.SenderRooms = new ObservableCollection<Room>(_roomRepository.GetValues());
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
                {
                    var room = _roomRepository.GetById(SelectedRoom.Id);
                    dialog = new AddOrEditRoomDialog(room);
                    dialog.ShowDialog();
                }
            }
            else if (InventoryTableVisibility == Visibility.Visible)
            {
                if (SelectedInventory != null)
                {
                    var inventory = _inventoryRepository.GetById(SelectedInventory.Id);
                    dialog = new AddOrEditInventoryDialog(inventory);
                    dialog.ShowDialog();
                }
            }
            else if (MedicineTableVisibility == Visibility.Visible && 
                     (SelectedMedicine.Status != MedicineStatus.PENDING &&
                      SelectedMedicine.Status != MedicineStatus.APPROVED))
            {
                if (SelectedMedicine != null)
                {
                    var medicine = _medicineRepository.GetById(SelectedMedicine.MedicineName);
                    dialog = new AddOrEditMedicineDialog(medicine);
                    dialog.ShowDialog();
                }
            }
        }

        public void HandleDeleteClick()
        {
            if (RoomTableVisibility == Visibility.Visible)
            {
                if (SelectedRoom != null)
                {
                    var room = _roomRepository.GetById(SelectedRoom.Id);
                    dialog = new WarningDialog(SelectedRoom);
                }
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

        public void HandleFClick()
        {
            if (InventoryTableVisibility == Visibility.Visible)
            {
                dialog = new InventoryFilteringDialog();
                dialog.ShowDialog();
            }
        }

        public void HandleAddClick()
        {
            if (InventoryTableVisibility == Visibility.Visible)
            {
                dialog = new InventoryAdderSubtractor(SelectedInventory);
                dialog.ShowDialog();
            }
        }

        public void HandleSClick()
        {
            if (MedicineTableVisibility == Visibility.Visible &&
                (SelectedMedicine.Status != MedicineStatus.PENDING &&
                 SelectedMedicine.Status != MedicineStatus.APPROVED))
            {
                dialog = new ValidationRequestDialog(SelectedMedicine);
                dialog.ShowDialog();
            }
        }

        public void HandleRClick()
        {
            if (MedicineTableVisibility == Visibility.Visible && SelectedMedicine.Status == MedicineStatus.REJECTED)
            {
                dialog = new RejectionNoteDialog(SelectedMedicine);
                dialog.ShowDialog();
            }
        }

        #endregion

        #region Events

        public void OnRoomsChanged(object sender, EventArgs e)
        {
            _roomMutex.WaitOne();

            Rooms = new ObservableCollection<Room>(_roomRepository.GetValues());
            OnPropertyChanged("Rooms");

            _roomMutex.ReleaseMutex();

            OnRefreshTransferDialog(sender, e);
        }

        public void OnInventoryChanged(object sender, EventArgs e)
        {
            _inventoryMutex.WaitOne();

            Inventory = new ObservableCollection<Inventory>(_inventoryRepository.GetValues());
            OnPropertyChanged("Inventory");

            _inventoryMutex.ReleaseMutex();
        }

        public void OnMedicineChanged(object sender, EventArgs e)
        {
            Medicines = new ObservableCollection<Medicine>(_medicineRepository.GetValues());
            OnPropertyChanged("Medicines");
        }

        public void OnRefreshTransferDialog(object sender, EventArgs e)
        {
            _transferMutex.WaitOne();
            _inventoryManagementDialogViewModel.OnShoudRefresh();
            _transferMutex.ReleaseMutex();
        }

        public void OnFilteringRequested(object sender, FilteringEventArgs e)
        {
            _inventoryMutex.WaitOne();
            var itemsVisual = CollectionViewSource.GetDefaultView(Inventory);

            _passedArgs = e;

            itemsVisual.Filter = InventoryFilter;
            _inventoryMutex.ReleaseMutex();
        }
        #endregion
    }
}
