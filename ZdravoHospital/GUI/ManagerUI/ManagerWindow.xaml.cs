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
                roomNumber.Binding = new Binding("Id");
                MainDataGrid.Columns.Add(roomNumber);

                DataGridTextColumn roomType = new DataGridTextColumn();
                roomType.Header = "Room type";
                roomType.Binding = new Binding("RoomType") { Converter = new RoomTypeConverter() };
                MainDataGrid.Columns.Add(roomType);

                DataGridTextColumn roomName = new DataGridTextColumn();
                roomName.Header = "Name";
                roomName.Binding = new Binding("Name");
                roomName.Width = new DataGridLength(20, DataGridLengthUnitType.Star);
                MainDataGrid.Columns.Add(roomName);

                DataGridTextColumn roomAvailable = new DataGridTextColumn();
                roomAvailable.Header = "Availability";
                roomAvailable.Binding = new Binding("Available") { Converter = new AvailabilityConverter() };
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
            RoomAddOrEdit dialog = new RoomAddOrEdit();
            dialog.Show();
        }
    }
}
