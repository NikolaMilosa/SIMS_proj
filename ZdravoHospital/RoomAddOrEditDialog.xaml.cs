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

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for RoomAddOrEditDialog.xaml
    /// </summary>
    public partial class RoomAddOrEditDialog : Window
    {
        ManagerWindow managerWindow;
        bool isAdder;
        Dictionary<RoomType, string> roomTypes;
        Dictionary<string, RoomType> invertRoomTypes;

        public RoomAddOrEditDialog(ManagerWindow mw, bool ia)
        {
            InitializeComponent();
            this.managerWindow = mw;
            this.isAdder = ia;

            roomTypes = new Dictionary<RoomType, string>();
            roomTypes[RoomType.APPOINTMENT_ROOM] = "Appointment Room";
            roomTypes[RoomType.BREAK_ROOM] = "Bed Room";
            roomTypes[RoomType.STORAGE_ROOM] = "Storage";
            roomTypes[RoomType.OPERATING_ROOM] = "Operation room";

            invertRoomTypes = new Dictionary<string, RoomType>();
            invertRoomTypes["Appointment Room"] = RoomType.APPOINTMENT_ROOM;
            invertRoomTypes["Bed Room"] = RoomType.BREAK_ROOM;
            invertRoomTypes["Storage"] = RoomType.STORAGE_ROOM;
            invertRoomTypes["Operation room"] = RoomType.OPERATING_ROOM;

            this.roomTypeComboBox.ItemsSource = roomTypes.Values;

            if (ia == true)
            {
                roomTypeComboBox.SelectedIndex = 0;
                roomIdTextBox.IsEnabled = true;
            }
            else
            {
                roomIdTextBox.IsEnabled = false;
                roomTypeComboBox.IsEnabled = false;
                int key;

                if (managerWindow.managerMainTable.SelectedItem != null)
                {
                    key = ((Room)managerWindow.managerMainTable.SelectedItem).Id;
                    if (((Room)managerWindow.managerMainTable.SelectedItem).RoomType == RoomType.APPOINTMENT_ROOM)
                    {
                        AppointmentRoom room = managerWindow.Res.AppointmentRooms[key];
                        roomIdTextBox.Text = room.Id.ToString();
                        roomNameTextBox.Text = room.Name;
                        roomTypeComboBox.SelectedItem = roomTypes[RoomType.APPOINTMENT_ROOM];
                        if (room.Avaliabe == true)
                            yesRadioButton.IsChecked = true;
                        else
                            noRadioButton.IsChecked = true;
                    }
                    else if (((Room)managerWindow.managerMainTable.SelectedItem).RoomType == RoomType.OPERATING_ROOM)
                    {
                        OperatingRoom room = managerWindow.Res.OperatingRooms[key];
                        roomIdTextBox.Text = room.Id.ToString();
                        roomNameTextBox.Text = room.Name;
                        roomTypeComboBox.SelectedItem = roomTypes[RoomType.OPERATING_ROOM];
                        if (room.Avaliabe == true)
                            yesRadioButton.IsChecked = true;
                        else
                            noRadioButton.IsChecked = true;
                    }
                    else
                    {
                        Room room = managerWindow.Res.StorageAndBedRooms[key];
                        roomIdTextBox.Text = room.Id.ToString();
                        roomNameTextBox.Text = room.Name;
                        roomTypeComboBox.SelectedItem = roomTypes[room.RoomType];
                        if (room.Avaliabe == true)
                            yesRadioButton.IsChecked = true;
                        else
                            noRadioButton.IsChecked = true;
                    }
                    this.Show();
                }
            }

            setOkButtonVisibility();
        }

        private void setOkButtonVisibility()
        {
            if ((roomIdTextBox.Text != String.Empty) && (roomNameTextBox.Text != String.Empty) && (roomTypeComboBox.SelectedIndex > -1) && ((yesRadioButton.IsChecked == true) || (noRadioButton.IsChecked == true)))
            {
                okButton.IsEnabled = true;
            }
            else
            {
                okButton.IsEnabled = false;
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            int key = Int32.Parse(roomIdTextBox.Text);
            if (this.isAdder)
            {
                if (managerWindow.Res.AppointmentRooms.ContainsKey(key) || managerWindow.Res.OperatingRooms.ContainsKey(key) || managerWindow.Res.StorageAndBedRooms.ContainsKey(key))
                {
                    roomIdTextBox.Foreground = new SolidColorBrush(Colors.Red);
                    warningLabel.Visibility = Visibility.Visible;
                    warningLabel.Content = "- Id already exists!";
                    warningLabel.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    switch (invertRoomTypes[(string)roomTypeComboBox.SelectedItem])
                    {
                        case RoomType.APPOINTMENT_ROOM:
                            managerWindow.Res.AppointmentRooms[key] = new AppointmentRoom(RoomType.APPOINTMENT_ROOM, key, roomNameTextBox.Text, (yesRadioButton.IsChecked == true) ? true : false);
                            break;
                        case RoomType.OPERATING_ROOM:
                            managerWindow.Res.OperatingRooms[key] = new OperatingRoom(RoomType.OPERATING_ROOM, key, roomNameTextBox.Text, (yesRadioButton.IsChecked == true) ? true : false);
                            break;
                        case RoomType.BREAK_ROOM:
                            managerWindow.Res.StorageAndBedRooms[key] = new Room(RoomType.BREAK_ROOM, key, roomNameTextBox.Text, (yesRadioButton.IsChecked == true) ? true : false);
                            break;
                        default:
                            managerWindow.Res.StorageAndBedRooms[key] = new Room(RoomType.STORAGE_ROOM, key, roomNameTextBox.Text, (yesRadioButton.IsChecked == true) ? true : false);
                            break;
                    }
                }
            }
            else
            {
                switch (invertRoomTypes[(string)roomTypeComboBox.SelectedItem])
                {
                    case RoomType.APPOINTMENT_ROOM:
                        managerWindow.Res.AppointmentRooms[key].Name = roomNameTextBox.Text;
                        managerWindow.Res.AppointmentRooms[key].Avaliabe = (yesRadioButton.IsChecked == true) ? true : false;
                        break;
                    case RoomType.OPERATING_ROOM:
                        managerWindow.Res.OperatingRooms[key].Name = roomNameTextBox.Text;
                        managerWindow.Res.OperatingRooms[key].Avaliabe = (yesRadioButton.IsChecked == true) ? true : false;
                        break;
                    default:
                        managerWindow.Res.StorageAndBedRooms[key].Name = roomNameTextBox.Text;
                        managerWindow.Res.StorageAndBedRooms[key].Avaliabe = (yesRadioButton.IsChecked == true) ? true : false;
                        break;
                }
            }
            managerWindow.drawRooms();
            this.Close();
        }

        private void checkOkButtonText(object sender, RoutedEventArgs e)
        {
            setOkButtonVisibility();
        }

        private void checkOkButtonRadio(object sender, RoutedEventArgs e)
        {
            setOkButtonVisibility();
        }

        private void keyPress(object sender, KeyEventArgs e)
        {
            char parsedCharacter = ' ';
            if (Char.TryParse(e.Key.ToString(), out parsedCharacter))
            {
                if (!char.IsControl(parsedCharacter) && !char.IsDigit(parsedCharacter))
                    e.Handled = true;
            }
        }

        private void changedText(object sender, TextChangedEventArgs e)
        {
            warningLabel.Visibility = Visibility.Hidden;
            roomIdTextBox.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
