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

        private bool _isDropDownOpen;
        private int _selectedIndex;

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

        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set
            {
                _isDropDownOpen = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }
        public MyICommand<string> ComboBoxCommand { get; set; }

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
                SelectedIndex = (int) Inventory.InventoryType;
                IsAdder = false;
            }

            _injector = injector;

            ConfirmCommand = new MyICommand(OnConfirm);
            ComboBoxCommand = new MyICommand<string>(OnComboBoxKeypress);

            _inventoryService = new InventoryService(injector);
        }

        #region Button functions

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

        private void OnComboBoxKeypress(string key)
        {
            if (key.Equals("Enter"))
            {
                IsDropDownOpen = (IsDropDownOpen == false) ? true : false;
            }
            else if (key.Equals("Down"))
            {
                if (SelectedIndex == 0 && IsDropDownOpen)
                {
                    SelectedIndex += 1;
                }
            }
            else if (key.Equals("Up"))
            {
                if (SelectedIndex == 1 && IsDropDownOpen)
                {
                    SelectedIndex -= 1;
                }
            }
        }

        #endregion

    }
}
