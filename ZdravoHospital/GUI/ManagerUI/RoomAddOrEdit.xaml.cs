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
    /// Interaction logic for RoomAddOrEdit.xaml
    /// </summary>
    public partial class RoomAddOrEdit : Window
    {
        List<RoomType> roomTypes = new List<RoomType>() { RoomType.APPOINTMENT_ROOM, RoomType.BREAK_ROOM, RoomType.STORAGE_ROOM, RoomType.OPERATING_ROOM };
        public RoomAddOrEdit()
        {
            InitializeComponent();
            Binding binding = new Binding() { Converter = new RoomTypeConverter() };
            binding.Source = roomTypes;
            RoomTypeComboBox.SetBinding(ComboBox.ItemsSourceProperty, binding);
            RoomTypeComboBox.SelectedIndex = 0;
            YesRadioButton.IsChecked = true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = RoomTypeComboBox.SelectedItem.ToString();
            RoomType temp;
            switch (selectedValue)
            {
                case "APPOINTMENT ROOM":
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

            Room newRoom = new Room(temp,Int32.Parse(IdTextBox.Text),NameTextBox.Text,(YesRadioButton.IsChecked == true) ? true : false);
            if (!Model.Resources.rooms.ContainsKey(newRoom.Id))
            {
                Model.Resources.rooms[newRoom.Id] = newRoom;
                ManagerWindow.oRooms.Add(newRoom);
                Model.Resources.SerializeRooms();
                this.Close();
            }
        
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
