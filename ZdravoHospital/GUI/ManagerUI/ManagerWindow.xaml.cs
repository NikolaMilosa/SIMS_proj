using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public enum ActiveTable
        {
            ROOMS_TABLE,
            STAFF_TABLE,
            INVENTORY_TABLE,
            INITIAL_STATE
        }

        Manager activeManager;
        static ActiveTable activeTable = ActiveTable.INITIAL_STATE;
        Window dialog;

        //Observable collections:
        public static ObservableCollection<Room> oRooms;
        public static ObservableCollection<Person> oPersons;
        public static ObservableCollection<Inventory> oInventory;
        
        public ManagerWindow(string au)
        {
            InitializeComponent();
            activeManager = Model.Resources.findManager(au);
            WelcomeLabel.Content += activeManager.Name;
            RoomsButton.Focus();
        }

        private void RoomsButton_gotFocus(object sender, RoutedEventArgs e)
        {
            RoomsMenuArrow.Visibility = Visibility.Visible;
            StaffMenuArrow.Visibility = Visibility.Hidden;
            InventoryMenuArrow.Visibility = Visibility.Hidden;

            RoomsMenuGrid.Visibility = Visibility.Visible;
        }

        private void StaffButton_gotFocus(object sender, RoutedEventArgs e)
        {
            RoomsMenuArrow.Visibility = Visibility.Hidden;
            StaffMenuArrow.Visibility = Visibility.Visible;
            InventoryMenuArrow.Visibility = Visibility.Hidden;

            RoomsMenuGrid.Visibility = Visibility.Hidden;
        }

        private void InventoryButton_gotFocus(object sender, RoutedEventArgs e)
        {
            RoomsMenuArrow.Visibility = Visibility.Hidden;
            StaffMenuArrow.Visibility = Visibility.Hidden;
            InventoryMenuArrow.Visibility = Visibility.Visible;

            RoomsMenuGrid.Visibility = Visibility.Hidden;
        }

        private void ShowRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Resources.rooms == null)
            {
                Model.Resources.OpenRooms();
                oRooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);
            }

            if (activeTable != ActiveTable.ROOMS_TABLE)
            {
                activeTable = ActiveTable.ROOMS_TABLE;

                DataGridTextColumn roomNumber = new DataGridTextColumn();
                roomNumber.Header = "Number";
                roomNumber.Binding = new Binding("Id") { Mode = BindingMode.OneWay };
                MainDataGrid.Columns.Add(roomNumber);

                DataGridTextColumn roomType = new DataGridTextColumn();
                roomType.Header = "Room type";
                roomType.Binding = new Binding("RoomType") { Converter = new RoomTypeConverter(), Mode = BindingMode.OneWay };
                MainDataGrid.Columns.Add(roomType);

                DataGridTextColumn roomName = new DataGridTextColumn();
                roomName.Header = "Name";
                roomName.Binding = new Binding("Name") { Mode = BindingMode.OneWay };
                roomName.Width = new DataGridLength(20, DataGridLengthUnitType.Star);
                MainDataGrid.Columns.Add(roomName);

                DataGridTextColumn roomAvailable = new DataGridTextColumn();
                roomAvailable.Header = "Availability";
                roomAvailable.Binding = new Binding("Available") { Converter = new AvailabilityConverter(), Mode = BindingMode.OneWay };
                MainDataGrid.Columns.Add(roomAvailable);

                Binding binding = new Binding();
                binding.Source = oRooms;
                binding.Mode = BindingMode.OneWay;
                MainDataGrid.SetBinding(DataGrid.ItemsSourceProperty, binding);
            }
        }

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Resources.rooms == null)
                Model.Resources.OpenRooms();
            dialog = new RoomAddOrEdit();
            dialog.ShowDialog();
        }

        private void MainDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (MainDataGrid.SelectedIndex != -1)
                {
                    dialog = new RoomAddOrEdit((Room)MainDataGrid.SelectedItem);
                    dialog.ShowDialog();
                }
                e.Handled = true;
            }
            if (e.Key == Key.Right)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Delete)
            {
                if (MainDataGrid.SelectedIndex != -1)
                {
                    dialog = new WarningDialog((Room)MainDataGrid.SelectedItem);
                    dialog.ShowDialog();
                }
                e.Handled = true;
            }
            if (e.Key == Key.Left)
            {
                e.Handled = true;
                if (activeTable == ActiveTable.ROOMS_TABLE)
                    ShowRoomsButton.Focus();
                //Further code for focus changing
            }
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
            }
        }

        private void MainMenu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                e.Handled = true;
        }
    }
}
