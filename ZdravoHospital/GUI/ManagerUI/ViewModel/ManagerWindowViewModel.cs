using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class ManagerWindowViewModel : ViewModel
    {
        //Fields :
        private string activeManager;
        private Visibility _roomTableVisibility;
        private Visibility _inventoryTableVisibility;
        private Visibility _medicineTableVisibility;

        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<Inventory> Inventory { get; set; }
        public ObservableCollection<Medicine> Medicines { get; set; }

        private Logics.TransferRequestsFunctions transferRequestFunctions;
        private Logics.RoomScheduleFunctions roomScheduleFunctions;

        //Table visibility
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

        //Properties :
        public string ActiveManager
        {
            get => activeManager;
            set
            {
                activeManager = value;
                OnPropertyChanged();
            }
        }

        //Commands :
        public MyICommand ShowRoomCommand { get; set; }
        public MyICommand ShowInventoryCommand { get; set; }
        public MyICommand ShowMedicineCommand { get; set; }

        public ManagerWindowViewModel(string au)
        {
            Employee currManager = Model.Resources.findManager(au);
            ActiveManager = currManager.Name;

            OpenDataBase();
            SetObservables();
            TurnOffTables();

            ShowRoomCommand = new MyICommand(OnShowRooms);
            ShowInventoryCommand = new MyICommand(OnShowInventory);
            ShowMedicineCommand = new MyICommand(OnShowMedicine);
        }

        private void OpenDataBase()
        {
            Model.Resources.OpenRooms();
            Model.Resources.OpenInventory();
            Model.Resources.OpenMedicines();
        }

        private void SetObservables()
        {
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);
            Inventory = new ObservableCollection<Inventory>(Model.Resources.inventory.Values);
            Medicines = new ObservableCollection<Medicine>(Model.Resources.medicines);
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

        #endregion
    }
}
