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

        public string InputTime
        {
            get { return _inputTime; }
            set
            {
                _inputTime = value;
                OnPropertyChanged("InputTime");
            }
        }

        public DateTime ChosenDate
        {
            get { return _chosenDate; }
            set
            {
                _chosenDate = value;
                OnPropertyChanged("ChosenDate");
            }
        }

        public bool IsStatic
        {
            get { return _isStatic; }
            set
            {
                _isStatic = value;
                OnPropertyChanged("IsStatic");
            }
        }

        public int EnteredQuantity
        {
            get { return _enteredQuantity; }
            set
            {
                _enteredQuantity = value;
                OnPropertyChanged("EnteredQuantity");
            }
        }

        public int MaxInventory
        {
            get { return _maxInventory; }
            set
            {
                _maxInventory = value;
                OnPropertyChanged("MaxInventory");
            }
        }

        public string DefinitionText
        {
            get { return _definitionText; }
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
        private Room firstRoom;
        private Room secondRoom;
        private ObservableCollection<InventoryDTO> firstRoomDTOs;
        private ObservableCollection<InventoryDTO> secondRoomDTOs;
        private string id;
        private InventoryDTO processedItem;
        #endregion end

        public InventoryManagementQuantitySelector(Room fr, Room sr, ObservableCollection<InventoryDTO> fri, ObservableCollection<InventoryDTO> sri, InventoryDTO invItem)
        {
            InitializeComponent();
            this.DataContext = this;

            this.firstRoom = fr;
            this.secondRoom = sr;
            this.firstRoomDTOs = fri;
            this.secondRoomDTOs = sri;
            this.MaxInventory = invItem.Quantity;
            this.id = invItem.Id;
            this.IsStatic = (invItem.InventoryType == InventoryType.STATIC_INVENTORY);
            this.ChosenDate = DateTime.Today;
            this.processedItem = invItem;

            DefinitionText = "Out of '" + MaxInventory + "' possible how many '" + Model.Resources.inventory[id].Name + "' would you like to transfer?";

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
            RoomInventory sender = Logics.RoomInventoryFunctions.FindRoomInventoryByRoomAndInventory(firstRoom.Id, processedItem.Id);
            RoomInventory reciever = Logics.RoomInventoryFunctions.FindRoomInventoryByRoomAndInventory(secondRoom.Id, processedItem.Id);

            if (sender.Quantity - EnteredQuantity == 0)
            {
                Logics.RoomInventoryFunctions.DeleteByReference(sender);
                firstRoomDTOs.Remove(processedItem);
            }
            else
            {
                int index = firstRoomDTOs.IndexOf(processedItem);
                firstRoomDTOs.Remove(processedItem);
                
                sender.Quantity -= EnteredQuantity;
                firstRoomDTOs.Insert(index, new InventoryDTO(processedItem.Name, sender.Quantity, processedItem.Id, processedItem.InventoryType));
            }

            if (reciever == null)
            {
                Logics.RoomInventoryFunctions.AddNewReference(new RoomInventory(processedItem.Id, secondRoom.Id, EnteredQuantity));
                secondRoomDTOs.Add(new InventoryDTO(processedItem.Name, EnteredQuantity, processedItem.Id, processedItem.InventoryType));
            }
            else
            {
                int index = 0;
                foreach(InventoryDTO inv in secondRoomDTOs)
                {
                    if (inv.Id.Equals(processedItem.Id))
                        break;
                    index++;
                }

                secondRoomDTOs.RemoveAt(index);
                reciever.Quantity += EnteredQuantity;

                secondRoomDTOs.Insert(index, new InventoryDTO(processedItem.Name, reciever.Quantity, processedItem.Id, processedItem.InventoryType));
            }

            Model.Resources.SerializeRoomInventory();
        }
    
        private void MoveStaticInventory()
        {
            TimeSpan enteredTime = TimeSpan.ParseExact(InputTime, "c", null);

            ChosenDate = ChosenDate.Add(enteredTime);

            MessageBox.Show(ChosenDate.ToString());
        }
    }
}
