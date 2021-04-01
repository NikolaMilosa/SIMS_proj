using System;
using System.Collections.Generic;
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
        Manager activeManager;
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
                Model.Resources.OpenRooms();

            if (MainDataGrid.ItemsSource == null)
            {
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

                MainDataGrid.ItemsSource = Model.Resources.rooms.Values;
            }
        }

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            RoomAddOrEdit dialog = new RoomAddOrEdit();
            dialog.Show();
        }
    }
}
