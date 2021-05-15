using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.Logics;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class WarningDialogViewModel : ViewModel
    {
        #region Fields

        private string _warningTitle;
        private string _warningText;
        private string _warningElement;

        private RoomFunctions _roomFunctions;
        private InventoryFunctions _inventoryFunctions;

        private object _someObject;
        private object[] _otherParams;

        private bool _isConfirmable;

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

        public WarningDialogViewModel(object someObject, params object[] otherParams)
        {
            _someObject = someObject;
            _otherParams = otherParams;

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
            }
        }

        private void OnConfirm()
        {
            switch (_someObject.GetType().Name)
            {
                case nameof(Room):
                    _roomFunctions = new RoomFunctions();
                    if (!_roomFunctions.DeleteRoom((Room) _someObject))
                    {
                        MessageBox.Show(
                            "Cannot delete the room since there aren't any available rooms to store the inventory");
                    }
                    break;
                case nameof(Inventory):
                    _inventoryFunctions = new InventoryFunctions();
                    _inventoryFunctions.DeleteInventory((Inventory) _someObject);
                    break;
            }
        }
    }
}
