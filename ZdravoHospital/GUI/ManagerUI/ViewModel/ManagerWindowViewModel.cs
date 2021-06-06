using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Model;
using Newtonsoft.Json;
using Repository.CredentialsPersistance;
using Repository.DoctorPersistance;
using Repository.EmployeePersistance;
using Repository.FeedbackPersistance;
using Repository.InventoryPersistance;
using Repository.MedicinePersistance;
using Repository.MedicineRecensionPersistance;
using Repository.NotificationsPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.PersonNotificationPersistance;
using Repository.ReferralPersistance;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using Repository.RoomSchedulePersistance;
using Repository.SpecializationPersistance;
using Repository.SurveyPersistance;
using Repository.TransferRequestPersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
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
            if (dashboard == null)
                dashboard = new ManagerWindowViewModel();
            return dashboard;
        }

        #region Mutex

        private static Mutex _roomMutex;
        private static Mutex _inventoryMutex;
        private static Mutex _transferMutex;

        #endregion

        #region Fields

        private string activeManager;
        private Employee currentEmployee;
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

        private InjectorDTO _injector;

        private string _tableName;

        private int _selectedRoomIndex;
        private int _selectedInventoryIndex;
        private int _selectedMedicineIndex;

        private bool _shouldFocusTable;

        private static Help _help;

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

        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                OnPropertyChanged();
            }
        }

        public int SelectedRoomIndex
        {
            get => _selectedRoomIndex;
            set
            {
                _selectedRoomIndex = value;
                OnPropertyChanged();
            }
        }

        public int SelectedInventoryIndex
        {
            get => _selectedInventoryIndex;
            set
            {
                _selectedInventoryIndex = value;
                OnPropertyChanged();
            }
        }

        public int SelectedMedicineIndex
        {
            get => _selectedMedicineIndex;
            set
            {
                _selectedMedicineIndex = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldFocusTable
        {
            get => _shouldFocusTable;
            set
            {
                _shouldFocusTable = value;
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
        public MyICommand<KeyEventArgs> TableCommand { get; set; }
        public MyICommand<KeyEventArgs> SubMenuCommand { get; set; }
        public MyICommand ShowHelpCommand { get; set; }
        public MyICommand DoctorReportCommand { get; set; }
        public MyICommand<object> LogoutCommand { get; set; }
        public MyICommand FeedbackCommand { get; set; }

        #endregion


        private ManagerWindowViewModel()
        {
            
        }

        #region Private functions

        private void OpenDataBase()
        {
            _roomRepository = _injector.RoomRepository;
            _inventoryRepository = _injector.InventoryRepository;
            _medicineRepository = _injector.MedicineRepository;
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
            var transferFunctions = new TransferRequestService(_injector);
            transferFunctions.RunOrExecute();

            var roomScheduleFunctions = new RoomScheduleService(_injector);
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

        private void InstantiateInjector()
        {
            _injector = new InjectorDTO()
            {
                CredentialsRepository = new CredentialsRepository(),
                DoctorRepository = new DoctorRepository(),
                EmployeeRepository = new EmployeeRepository(),
                InventoryRepository = new InventoryRepository(),
                MedicineRecensionRepository = new MedicineRecensionRepository(),
                MedicineRepository = new MedicineRepository(),
                NotificationsRepository = new NotificationRepository(),
                PatientRepository = new PatientRepository(),
                PeriodRepository = new PeriodRepository(),
                PersonNotificationRepository = new PersonNotificationRepository(),
                ReferralRepository = new ReferralRepository(),
                RoomInventoryRepository = new RoomInventoryRepository(),
                RoomRepository = new RoomRepository(),
                RoomScheduleRepository = new RoomScheduleRepository(),
                SpecializationRepository = new SpecializationRepository(),
                SurveyRepository = new SurveyRepository(),
                TransferRepository = new TransferRequestRepository(),
                FeedbackRepository = new FeedbackRepository()
            };
        }

        #endregion

        #region Public functions

        public void Initialize(string activeUser)
        {
            _employeeRepository = new EmployeeRepository();
            currentEmployee = _employeeRepository.GetById(activeUser);
            ActiveManager = "Welcome, " + currentEmployee.Name;

            InstantiateInjector();
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
            TableCommand = new MyICommand<KeyEventArgs>(OnTableKey);
            SubMenuCommand = new MyICommand<KeyEventArgs>(OnSubMenuKey);
            ShowHelpCommand = new MyICommand(OnShowHelp);
            DoctorReportCommand = new MyICommand(OnDoctorReport);
            LogoutCommand = new MyICommand<object>(OnLogout);
            FeedbackCommand = new MyICommand(OnFeedback);

            _roomMutex = new Mutex();
            _inventoryMutex = new Mutex();
            _transferMutex = new Mutex();

            _inventoryManagementDialogViewModel = new InventoryManagementDialogViewModel(_injector);

            RunAllTasks();
        }

        #endregion

        #region Button Functions

        //Show rooms button
        private void OnShowRooms()
        {
            TurnOffTables();
            RoomTableVisibility = Visibility.Visible;
            SelectedRoomIndex = -1;
            TableName = "Room table >";
        }

        //Show inventory button
        private void OnShowInventory()
        {
            TurnOffTables();
            InventoryTableVisibility = Visibility.Visible;
            SelectedInventoryIndex = -1;
            TableName = "Inventory table >";
        }

        //Show medicine button
        private void OnShowMedicine()
        {
            TurnOffTables();
            MedicineTableVisibility = Visibility.Visible;
            SelectedMedicineIndex = -1;
            TableName = "Medicine table >";
        }

        //Add room button
        private void OnAddRoom()
        {
            dialog = new AddOrEditRoomDialog(null, _injector, false);
            dialog.ShowDialog();
        }

        //Add inventory button
        private void OnAddInventory()
        {
            dialog = new AddOrEditInventoryDialog(null, _injector);
            dialog.ShowDialog();
        }

        //Add medicine button
        private void OnAddMedicine()
        {
            dialog = new AddOrEditMedicineDialog(null, _injector);
            dialog.ShowDialog();
        }

        //Manage inventory button
        private void OnManageInventory()
        {
            _inventoryManagementDialogViewModel.SenderRooms = new ObservableCollection<Room>(_roomRepository.GetValues());
            _inventoryManagementDialogViewModel.SenderIndex = -1;
            _inventoryManagementDialogViewModel.ReceiverIndex = -1;
            dialog = new InventoryManagementDialog(_inventoryManagementDialogViewModel);
            dialog.ShowDialog();
        }

        //Plan renovation button
        private void OnPlanRenovation()
        {
            dialog = new RenovationPlaningDialog(_injector);
            dialog.ShowDialog();
        }

        private void OnTableKey(KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (RoomTableVisibility == Visibility.Visible)
                {
                    if (SelectedRoomIndex < Rooms.Count - 1)
                    {
                        SelectedRoomIndex += 1;
                    }
                }
                else if (InventoryTableVisibility == Visibility.Visible)
                {
                    if (SelectedInventoryIndex < Inventory.Count - 1)
                    {
                        SelectedInventoryIndex += 1;
                    }
                }
                else if (MedicineTableVisibility == Visibility.Visible)
                {
                    if (SelectedMedicineIndex < Medicines.Count - 1)
                    {
                        SelectedMedicineIndex += 1;
                    }
                }
            }
            else if (e.Key == Key.Up)
            {
                if (RoomTableVisibility == Visibility.Visible)
                {
                    if (SelectedRoomIndex > 0)
                    {
                        SelectedRoomIndex -= 1;
                    }
                }
                else if (InventoryTableVisibility == Visibility.Visible)
                {
                    if (SelectedInventoryIndex > 0)
                    {
                        SelectedInventoryIndex -= 1;
                    }
                }
                else if (MedicineTableVisibility == Visibility.Visible)
                {
                    if (SelectedMedicineIndex > 0)
                    {
                        SelectedMedicineIndex -= 1;
                    }
                }
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                HandleEnterClick();
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                HandleDeleteClick();
                e.Handled = true;
            }
            else if (e.Key == Key.F)
            {
                HandleFClick();
                e.Handled = true;
            }
            else if (e.Key == Key.Add)
            {
                HandleAddClick();
                e.Handled = true;
            }
            else if (e.Key == Key.S)
            {
                HandleSClick();
                e.Handled = true;
            }
            else if (e.Key == Key.R)
            {
                HandleRClick();
                e.Handled = true;
            }
        }

        private void OnSubMenuKey(KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                ShouldFocusTable = true;
                e.Handled = true;
            }

            ShouldFocusTable = false;
        }

        private void OnShowHelp()
        {
            if (_help == null || !_help.IsVisible)
            {
                _help = new Help();
                _help.Show();
            }
        }

        private void OnDoctorReport()
        {
            dialog = new DoctorReportDialog(_injector);
            dialog.ShowDialog();
        }

        private void OnLogout(object window)
        {
            Window newWindow = new MainWindow();
            newWindow.Show();
            (window as Window).Close();
        }

        private void OnFeedback()
        {
            dialog = new FeedbackDialog(_injector, currentEmployee.Username);
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
                    dialog = new AddOrEditRoomDialog(room, _injector,false);
                    dialog.ShowDialog();
                }
            }
            else if (InventoryTableVisibility == Visibility.Visible)
            {
                if (SelectedInventory != null)
                {
                    var inventory = _inventoryRepository.GetById(SelectedInventory.Id);
                    dialog = new AddOrEditInventoryDialog(inventory, _injector);
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
                    dialog = new AddOrEditMedicineDialog(medicine, _injector);
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
                    dialog = new WarningDialog(_injector, SelectedRoom);
                }
            }
            else if (InventoryTableVisibility == Visibility.Visible)
            {
                if (SelectedInventory != null)
                    dialog = new WarningDialog(_injector, SelectedInventory);
                
            }
            else if (MedicineTableVisibility == Visibility.Visible)
            {
                if (SelectedMedicine != null)
                    dialog = new WarningDialog(_injector, SelectedMedicine);
            }

            if (dialog != null)
                dialog.ShowDialog();
        }

        public void HandleFClick()
        {
            if (InventoryTableVisibility == Visibility.Visible)
            {
                var itemsVisual = CollectionViewSource.GetDefaultView(Inventory);
                itemsVisual.Filter = o => true;
                dialog = new InventoryFilteringDialog();
                dialog.ShowDialog();
            }
        }

        public void HandleAddClick()
        {
            if (InventoryTableVisibility == Visibility.Visible)
            {
                dialog = new InventoryAdderSubtractor(SelectedInventory, _injector);
                dialog.ShowDialog();
            }
        }

        public void HandleSClick()
        {
            if (MedicineTableVisibility == Visibility.Visible &&
                (SelectedMedicine.Status != MedicineStatus.PENDING &&
                 SelectedMedicine.Status != MedicineStatus.APPROVED))
            {
                dialog = new ValidationRequestDialog(SelectedMedicine, _injector);
                dialog.ShowDialog();
            }
        }

        public void HandleRClick()
        {
            if (MedicineTableVisibility == Visibility.Visible && SelectedMedicine.Status == MedicineStatus.REJECTED)
            {
                dialog = new RejectionNoteDialog(SelectedMedicine, _injector);
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
