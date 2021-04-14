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

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for InventoryManagementWindow.xaml
    /// </summary>
    public partial class InventoryManagementWindow : Window, INotifyPropertyChanged
    {
        //Feilds:
        private ObservableCollection<Room> _firstRooms;
        private ObservableCollection<Room> _secondRooms;
        private Room _firstRoom;
        private Room _secondRoom;

        private ObservableCollection<InventoryDTO> _firstRoomInventory;
        private ObservableCollection<InventoryDTO> _secondRoomInventory;

        public ObservableCollection<InventoryDTO> FirstRoomInventory
        {
            get { return _firstRoomInventory; }
            set
            {
                _firstRoomInventory = value;
                OnPropertyChanged("FirstRoomInventory");
            }
        }

        public ObservableCollection<InventoryDTO> SecondRoomInventory
        {
            get { return _secondRoomInventory; }
            set
            {
                _secondRoomInventory = value;
                OnPropertyChanged("SecondRoomInventory");
            }
        }

        public ObservableCollection<Room> FirstRooms
        {
            get { return _firstRooms; }
            set
            {
                _firstRooms = value;
                OnPropertyChanged("FirstRooms");
            }
        }

        public ObservableCollection<Room> SecondRooms
        {
            get { return _secondRooms; }
            set
            {
                _secondRooms = value;
                OnPropertyChanged("SecondRooms");
            }
        }

        public Room FirstRoom
        {
            get { return _firstRoom; }
            set
            {
                _firstRoom = value;

                SecondRooms = new ObservableCollection<Room>(FirstRooms);
                SecondRooms.Remove(_firstRoom);

                FirstRoomInventory = new ObservableCollection<InventoryDTO>();
                foreach (KeyValuePair<string, int> kvp in _firstRoom.Inventory)
                    FirstRoomInventory.Add(new InventoryDTO(Model.Resources.inventory[kvp.Key].Name, kvp.Value, kvp.Key));

                OnPropertyChanged("FirstRoomInventory");

                OnPropertyChanged("SecondRooms");
            }
        }

        public Room SecondRoom
        {
            get { return _secondRoom; }
            set
            {
                _secondRoom = value;

                SecondRoomInventory = new ObservableCollection<InventoryDTO>();
                if(_secondRoom != null)
                    foreach (KeyValuePair<string, int> kvp in _secondRoom.Inventory)
                        SecondRoomInventory.Add(new InventoryDTO(Model.Resources.inventory[kvp.Key].Name, kvp.Value, kvp.Key));

                OnPropertyChanged("SecondRoomInventory");
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

        public InventoryManagementWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            FirstRooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);
        }

        private void FirstRoomDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (FirstRoomDataGrid.SelectedIndex + 1 < FirstRoomDataGrid.Items.Count)
                    FirstRoomDataGrid.SelectedIndex += 1;
                else if (FirstRoomDataGrid.SelectedIndex + 1 == FirstRoomDataGrid.Items.Count)
                {
                    FirstRoomDataGrid.ScrollIntoView(FirstRoomDataGrid.Items[0]);
                    FirstRoomDataGrid.SelectedIndex = 0;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (FirstRoomDataGrid.SelectedIndex - 1 >= 0)
                    FirstRoomDataGrid.SelectedIndex -= 1;
                else if (FirstRoomDataGrid.SelectedIndex - 1 < 0)
                {
                    FirstRoomDataGrid.ScrollIntoView(FirstRoomDataGrid.Items[FirstRoomDataGrid.Items.Count - 1]);
                    FirstRoomDataGrid.SelectedIndex = FirstRoomDataGrid.Items.Count - 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                SecondRoomComboBox.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                FinishButton.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                FirstRoomComboBox.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (FirstRoom != null && SecondRoom != null)
                {
                    Window dialog = new InventoryManagementQuantitySelector(FirstRoom, SecondRoom, FirstRoomInventory, SecondRoomInventory, ((InventoryDTO)FirstRoomDataGrid.SelectedItem).Id);
                    dialog.ShowDialog();
                }
                e.Handled = true;
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FirstRoomDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FirstRoomDataGrid.Items.Count > 0)
            {
                FirstRoomDataGrid.SelectedIndex = 0;
                FirstRoomDataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void FirstRoomComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                SecondRoomComboBox.Focus();
                e.Handled = true;
            } 
            else if (e.Key == Key.Left)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (FirstRoomComboBox.IsDropDownOpen == false)
                    FirstRoomComboBox.IsDropDownOpen = true;
                else
                    FirstRoomComboBox.IsDropDownOpen = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (FirstRoomComboBox.IsDropDownOpen == false)
                    FirstRoomDataGrid.Focus();
                else
                {
                    if (FirstRoomComboBox.SelectedIndex + 1 < FirstRoomComboBox.Items.Count)
                        FirstRoomComboBox.SelectedIndex += 1;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (FirstRoomComboBox.IsDropDownOpen == true)
                {
                    if (FirstRoomComboBox.SelectedIndex - 1 >= 0)
                        FirstRoomComboBox.SelectedIndex -= 1;
                }
                e.Handled = true;
            }
        }

        private void SecondRoomComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                FirstRoomComboBox.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (SecondRoomComboBox.IsDropDownOpen == false)
                    SecondRoomComboBox.IsDropDownOpen = true;
                else
                    SecondRoomComboBox.IsDropDownOpen = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (SecondRoomComboBox.IsDropDownOpen == false)
                    FinishButton.Focus();
                else
                {
                    if (SecondRoomComboBox.SelectedIndex + 1 < SecondRoomComboBox.Items.Count)
                        SecondRoomComboBox.SelectedIndex += 1;

                }
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (SecondRoomComboBox.IsDropDownOpen == true)
                {
                    if (SecondRoomComboBox.SelectedIndex - 1 >= 0)
                        SecondRoomComboBox.SelectedIndex -= 1;
                }

                e.Handled = true;
            }
        }
    }
}
