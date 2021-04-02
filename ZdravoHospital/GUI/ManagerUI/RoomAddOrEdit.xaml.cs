using System;
using System.Collections.Generic;
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
    /// Interaction logic for RoomAddOrEdit.xaml
    /// </summary>
    public partial class RoomAddOrEdit : Window
    {
        List<RoomType> roomTypes = new List<RoomType>() { RoomType.APPOINTMENT_ROOM, RoomType.BREAK_ROOM, RoomType.STORAGE_ROOM, RoomType.OPERATING_ROOM };
        bool isAdder;
        Room newRoom;

        public RoomAddOrEdit()
        {
            InitializeComponent();
            Binding binding = new Binding() { Converter = new RoomTypeConverter() };
            binding.Source = roomTypes;
            RoomTypeComboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);
            RoomTypeComboBox.SelectedIndex = 0;
            YesRadioButton.IsChecked = true;
            isAdder = true;
            this.Title = "Room adding";
        }

        public RoomAddOrEdit(Room r)
        {
            InitializeComponent();
            Binding binding = new Binding() { Converter = new RoomTypeConverter() };
            binding.Source = roomTypes;
            RoomTypeComboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);

            IdTextBox.Text = r.Id.ToString();
            IdTextBox.IsEnabled = false;
            NameTextBox.Text = r.Name;
            if (r.Available)
                YesRadioButton.IsChecked = true;
            else
                NoRadioButton.IsChecked = true;
            RoomTypeComboBox.SelectedIndex = roomTypes.IndexOf(r.RoomType);
            
            newRoom = r;
            isAdder = false;

            WarningLabel.Visibility = Visibility.Hidden;
            this.Title = "Room editing";

            fieldChecker();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = RoomTypeComboBox.SelectedItem.ToString();
            RoomType temp;
            switch (selectedValue)
            {
                case "APPOINTMENT":
                    temp = RoomType.APPOINTMENT_ROOM;
                    break;
                case "BEDROOM":
                    temp = RoomType.BREAK_ROOM;
                    break;
                case "STORAGE":
                    temp = RoomType.STORAGE_ROOM;
                    break;
                default:
                    temp = RoomType.OPERATING_ROOM;
                    break;
            }

            if (isAdder)
            {
                newRoom = new Room(temp, Int32.Parse(IdTextBox.Text), NameTextBox.Text, (YesRadioButton.IsChecked == true) ? true : false);
                if (!Model.Resources.rooms.ContainsKey(newRoom.Id))
                {
                    Model.Resources.rooms[newRoom.Id] = newRoom;
                    ManagerWindow.oRooms.Add(newRoom);
                    Model.Resources.SerializeRooms();
                    this.Close();
                }
            }
            else
            {
                int index = ManagerWindow.oRooms.IndexOf(newRoom);
                newRoom.Name = NameTextBox.Text;
                newRoom.Available = (YesRadioButton.IsChecked == true) ? true : false;
                newRoom.RoomType = temp;
                ManagerWindow.oRooms.Remove(ManagerWindow.oRooms[index]);
                ManagerWindow.oRooms.Insert(index, newRoom);
                Model.Resources.SerializeRooms();
                this.Close();
            }
        
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void YesRadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                YesRadioButton.IsChecked = true;
        }

        private void NoRadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                NoRadioButton.IsChecked = true;
        }

        private void fieldChecker()
        {
            if (NameTextBox.Text.Equals(String.Empty) || IdTextBox.Text.Equals(String.Empty) || WarningLabel.Visibility == Visibility.Visible)
                ConfirmButton.IsEnabled = false;
            else
                ConfirmButton.IsEnabled = true;
        }

        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int id = Int32.Parse(IdTextBox.Text);
            if (Model.Resources.rooms.ContainsKey(id))
            {
                WarningLabel.Content = "- Id exists";
                WarningLabel.Visibility = Visibility.Visible;
            }
            else
            {
                WarningLabel.Visibility = Visibility.Hidden;
            }

            fieldChecker();
        }

        private void IdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            char parsedCharacter = ' ';
            if (Char.TryParse(e.Key.ToString(), out parsedCharacter))
                if (!char.IsControl(parsedCharacter) && !char.IsDigit(parsedCharacter))
                    e.Handled = true;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fieldChecker();
        }

        private void RoomTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fieldChecker();
        }
    }
}
