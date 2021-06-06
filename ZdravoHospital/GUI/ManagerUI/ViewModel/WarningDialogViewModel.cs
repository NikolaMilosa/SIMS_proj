using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class WarningDialogViewModel : ViewModel
    {
        #region Fields

        private string _warningTitle;
        private string _warningText;
        private string _warningElement;

        private RoomService _roomService;
        private InventoryService _inventoryService;
        private MedicineService _medicineService;

        private object _someObject;
        private object[] _otherParams;

        private bool _isConfirmable;

        private InjectorDTO _injector;

        #endregion

        #region Properties

        public string WarningTitle
        {
            get => _warningTitle;
            set
            {
                _warningTitle = value;
                OnPropertyChanged();
            }
        }

        public string WarningText
        {
            get => _warningText;
            set
            {
                _warningText = value;
                OnPropertyChanged();
            }
        }

        public string WarningElement
        {
            get => _warningElement;
            set
            {
                _warningElement = value;
                OnPropertyChanged();
            }
        }

        public bool IsConfirmable
        {
            get => _isConfirmable;
            set
            {
                _isConfirmable = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public WarningDialogViewModel(InjectorDTO injector,object someObject, params object[] otherParams)
        {
            _someObject = someObject;
            _otherParams = otherParams;

            _injector = injector;

            IsConfirmable = true;

            DisplayText();

            ConfirmCommand = new MyICommand(OnConfirm);
        }

        private void DisplayText()
        {
            switch (_someObject.GetType().Name)
            {
                case nameof(Room):
                    WarningTitle = "Warning! Deleting a room!";
                    WarningText = "You are about to delete a room! If you wish to continue press \"Confirm\"";
                    WarningElement = "Room Id : " + ((Room)_someObject).Id;
                    break;
                case nameof(Inventory):
                    WarningTitle = "Warning! Deleting inventory";
                    WarningText = "You are about to delete some inventory! If you wish to continue press \"Confirm\"";
                    WarningElement = "Inventory Id : " + ((Inventory)_someObject).Id;
                    break;
                case nameof(AddOrEditInventoryDialogViewModel):
                    WarningTitle = "Warning! No available rooms";
                    WarningText = "There are no rooms where inventory would be stored...";
                    IsConfirmable = false;
                    break;
                case nameof(Ingredient):
                    WarningTitle = "Warning! Deleting ingredient";
                    WarningText = "You are about to delete some ingredients from medicine, but it won't be permanent just yet!";
                    WarningElement = "Ingredient name : " + ((Ingredient)_someObject).IngredientName;
                    break;
                case nameof(Medicine):
                    WarningTitle = "Warning! Deleting medicine";
                    WarningText = "You are about to delete some medicine! If you wish to continue press \"Confirm\"";
                    WarningElement = "Medicine name : " + ((Medicine)_someObject).MedicineName;
                    break;
            }
        }

        private void OnConfirm()
        {
            switch (_someObject.GetType().Name)
            {
                case nameof(Room):
                    _roomService = new RoomService(_injector);
                    if (!_roomService.DeleteRoom((Room) _someObject))
                    {
                        MessageBox.Show(
                            "Cannot delete the room since there aren't any available rooms to store the inventory " +
                            "or because it has treatments");
                    }
                    break;
                case nameof(Inventory):
                    _inventoryService = new InventoryService(_injector);
                    if (!_inventoryService.DeleteInventory((Inventory) _someObject))
                    {
                        MessageBox.Show(
                            "Cannot delete beds since there are treatments");
                    }
                    break;
                case nameof(Ingredient):
                    _medicineService = new MedicineService((AddOrEditMedicineDialogViewModel) _otherParams[0], _injector);
                    _medicineService.DeleteIngredientFromMedicine((Ingredient) _someObject, (Medicine) _otherParams[1]);
                    break;
                case nameof(Medicine):
                    _medicineService = new MedicineService(null, _injector);
                    _medicineService.DeleteMedicine((Medicine) _someObject);
                    break;
            }
        }
    }
}
