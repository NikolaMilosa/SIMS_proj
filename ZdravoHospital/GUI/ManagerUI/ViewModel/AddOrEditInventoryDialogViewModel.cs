using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.View;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class AddOrEditInventoryDialogViewModel : ViewModel
    {
        #region Fields

        private string _title;
        private Inventory _inventory;
        private bool _isAdder;

        private Window _dialog;

        private InventoryService _inventoryService;

        private InjectorDTO _injector;

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

        public AddOrEditInventoryDialogViewModel(Inventory? inventory, InjectorDTO injector)
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

            _injector = injector;

            ConfirmCommand = new MyICommand(OnConfirm);

            _inventoryService = new InventoryService(injector);
        }

        private void OnConfirm()
        {
            if (IsAdder)
            {
                if (!_inventoryService.AddInventory(Inventory))
                {
                    _dialog = new WarningDialog(_injector, this);
                    _dialog.ShowDialog();
                }
            }
            else
            {
                _inventoryService.EditInventory(Inventory);
            }
        }
    }
}
