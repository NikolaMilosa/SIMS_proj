using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Text;
using ZdravoHospital.GUI.ManagerUI.Commands;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class InventoryFilteringDialogViewModel : ViewModel
    {
        #region Fields

        private string _id;
        private string _inventoryName;
        private string _supplier;
        private string _quantity;
        private string _type;

        private int _selectedInd;

        private bool _isDropDownOpen;

        #endregion

        #region Properties

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string InventoryName
        {
            get => _inventoryName;
            set
            {
                _inventoryName = value;
                OnPropertyChanged();
            }
        }

        public string Supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                OnPropertyChanged();
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public int SelectedInd
        {
            get => _selectedInd;
            set
            {
                _selectedInd = value;
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

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }
        public MyICommand<string> ComboBoxCommand { get; set; }

        #endregion

        #region Event things

        public delegate void FilteringEventHandler(object sender, FilteringEventArgs e);

        public event FilteringEventHandler FilteringRequest;

        public void OnFilteringRequested()
        {
            if (FilteringRequest != null)
            {
                var args = FormatInput();
                FilteringRequest(this, args);
            }
        }

        #endregion

        public InventoryFilteringDialogViewModel()
        {
            SelectedInd = 0;

            Id = "";
            InventoryName = "";
            Supplier = "";
            Quantity = "";
            Type = " BOTH";

            FilteringRequest += ManagerWindowViewModel.GetDashboard().OnFilteringRequested;

            ConfirmCommand = new MyICommand(OnConfirm);
            ComboBoxCommand = new MyICommand<string>(OnComboBox);
        }

        #region Button functions

        private void OnConfirm()
        {
            OnFilteringRequested();
        }

        private void OnComboBox(string key)
        {
            if (key.Equals("Enter"))
            {
                IsDropDownOpen = (IsDropDownOpen == false) ? true : false;
            }
            else if (key.Equals("Up"))
            {
                if (IsDropDownOpen && SelectedInd > 0)
                {
                    SelectedInd -= 1;
                }
            }
            else if (key.Equals("Down"))
            {
                if (IsDropDownOpen && SelectedInd < 2)
                {
                    SelectedInd += 1;
                }
            }
        }

        #endregion

        #region Private functions

        private FilteringEventArgs FormatInput()
        {
            Id = Id.Trim().ToUpper();
            InventoryName = InventoryName.Trim().ToLower();

            if (!Supplier.Trim().Equals(string.Empty))
            {
                Supplier = Supplier.Trim().Substring(0, 1).ToUpper() + Supplier.Trim().Substring(1).ToLower();
            }
            else
            {
                Supplier = Supplier.Trim();
            }

            var parts = Type.Split(" ");
            Type = parts[1];

            int enteredQuantity;
            if (Quantity.Trim().Equals(string.Empty))
            {
                enteredQuantity = int.MaxValue;
            }
            else
            {
                enteredQuantity = int.Parse(Quantity);
            }

            var ret = new FilteringEventArgs()
            {
                Id = Id,
                InventoryName = InventoryName,
                Quantity = enteredQuantity,
                Supplier = Supplier,
                Type = Type
            };

            return ret;
        }

        #endregion
    }

    public class FilteringEventArgs : EventArgs
    {
        public string Id { get; set; }
        public string InventoryName { get; set; }
        public string Supplier { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }

        public FilteringEventArgs() { }
    }
}
