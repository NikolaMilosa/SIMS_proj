using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for InventoryManagementQuantitySelector.xaml
    /// </summary>
    public partial class InventoryManagementQuantitySelector : Window, INotifyPropertyChanged
    {
        #region fields
        //Fields:
        private int _maxInventory;
        private string _definitionText;
        private int _enteredQuantity;
        private DateTime _chosenDate;
        private string _inputTime;
        private bool _isStatic;

        private Logics.TransferRequestsFunctions _transferRequestsFunctions;

        public Room FirstRoom
        {
            get => _firstRoom;
            set 
            { 
                _firstRoom = value;
                OnPropertyChanged("FirstRoom");
            }
        }
        public Room SecondRoom
        {
            get => _secondRoom;
            set 
            { 
                _secondRoom = value;
                OnPropertyChanged("SecondRoom");
            }
        }

        public string InputTime
        {
            get => _inputTime;
            set
            {
                _inputTime = value;
                OnPropertyChanged("InputTime");
            }
        }

        public DateTime ChosenDate
        {
            get => _chosenDate;
            set
            {
                _chosenDate = value;
                OnPropertyChanged("ChosenDate");
            }
        }

        public bool IsStatic
        {
            get => _isStatic;
            set
            {
                _isStatic = value;
                OnPropertyChanged("IsStatic");
            }
        }

        public int EnteredQuantity
        {
            get => _enteredQuantity;
            set
            {
                _enteredQuantity = value;
                OnPropertyChanged("EnteredQuantity");
            }
        }

        public int MaxInventory
        {
            get => _maxInventory;
            set
            {
                _maxInventory = value;
                OnPropertyChanged("MaxInventory");
            }
        }

        public string DefinitionText
        {
            get => _definitionText;
            set
            {
                _definitionText = value;
                OnPropertyChanged("DefinitionText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        //Helpers:
        private Room _firstRoom;
        private Room _secondRoom;
        private string _id;
        private InventoryDTO _processedItem;
        #endregion end

        public InventoryManagementQuantitySelector(Room fr, Room sr, InventoryDTO invItem)
        {
            InitializeComponent();
            this.DataContext = this;

            this._transferRequestsFunctions = new Logics.TransferRequestsFunctions();

            FirstRoom = fr;
            SecondRoom = sr;
            this._id = invItem.Id;
            this.IsStatic = (invItem.InventoryType == InventoryType.STATIC_INVENTORY);
            this.ChosenDate = DateTime.Today;
            this._processedItem = invItem;

            /* Checking if there are any scheduled inventory changes for this room and that inventory */
            this.MaxInventory = invItem.Quantity;
            foreach (TransferRequest tr in Model.Resources.transferRequests)
            {
                if (tr.SenderRoom == _firstRoom.Id && tr.InventoryId.Equals(_processedItem.Id))
                    MaxInventory -= tr.Quantity;
            }

            DefinitionText = "Out of '" + MaxInventory + "' possible how many '" + Model.Resources.inventory[_id].Name + "' would you like to transfer?";
            
            if (MaxInventory != invItem.Quantity)
            {
                DefinitionText += "\nThis room has '" + (invItem.Quantity - MaxInventory) + "' units scheduled for transfer."; 
            }
            
            if (!IsStatic)
            {
                this.ChosenDate = DateTime.Today.AddDays(1);
                this.InputTime = "0:0";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (IsStatic)
            {
                MoveStaticInventory();
            }
            else
            {
                MoveDynamicInventory();
            }
            this.Close();
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!DatePicker.IsDropDownOpen)
            {
                if (e.Key == Key.Enter)
                {
                    DatePicker.IsDropDownOpen = true;
                    e.Handled = true;
                }
                else if (e.Key == Key.Tab) { }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    ChosenDate = (DateTime)DatePicker.SelectedDate;
                    DatePicker.IsDropDownOpen = false;
                    e.Handled = true;
                }
                else if (e.Key == Key.Left) { }
                else if (e.Key == Key.Right) { }
                else if (e.Key == Key.Up) { }
                else if (e.Key == Key.Down) { }
                else if (e.Key == Key.Tab)
                {
                    DatePicker.IsDropDownOpen = false;
                    e.Handled = true;
                    CancelButton.Focus();
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void MoveDynamicInventory()
        {
            _transferRequestsFunctions.ExecuteRequest(new TransferRequest(_firstRoom.Id, _secondRoom.Id, _processedItem.Id, EnteredQuantity, DateTime.Now));
        }
    
        private void MoveStaticInventory()
        {
            TimeSpan enteredTime = TimeSpan.ParseExact(InputTime, "c", null);
            ChosenDate = ChosenDate.Add(enteredTime);

            TransferRequest newRequest = new TransferRequest(_firstRoom.Id, _secondRoom.Id, _processedItem.Id, EnteredQuantity, ChosenDate);

            _transferRequestsFunctions.CreateAndStartTransfer(newRequest);
        }
    }
}
