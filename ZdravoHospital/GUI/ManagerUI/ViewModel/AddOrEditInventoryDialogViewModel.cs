using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.Logics;
using ZdravoHospital.GUI.ManagerUI.View;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class AddOrEditInventoryDialogViewModel : ViewModel
    {
        #region Fields

        private string _title;
        private Inventory _inventory;
        private bool _isAdder;

        private Window _dialog;

        private InventoryFunctions _inventoryFunctions;

        #endregion

        #region Properties

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public Inventory Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdder
        {
            get => _isAdder;
            set
            {
                _isAdder = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public AddOrEditInventoryDialogViewModel(Inventory? inventory)
        {
            if (inventory == null)
            {
                Inventory = new Inventory();
                Title = "Inventory adding";
                IsAdder = true;
            }
            else
            {
                Inventory = new Inventory(inventory);
                Title = "Inventory editing";
                IsAdder = false;
            }

            ConfirmCommand = new MyICommand(OnConfirm);

            _inventoryFunctions = new InventoryFunctions();
        }

        private void OnConfirm()
        {
            if (IsAdder)
            {
                if (!_inventoryFunctions.AddInventory(Inventory))
                {
                    _dialog = new WarningDialog(this);
                    _dialog.ShowDialog();
                }
            }
            else
            {
                _inventoryFunctions.EditInventory(Inventory);
            }
        }
    }
}
