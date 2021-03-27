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
        Resources res;
        Dictionary<string,RoomType> roomTypes;
        bool isAdder;
        public RoomAddOrEditDialog(Resources r, bool ia)
        {
            InitializeComponent();
            this.res = r;
            this.isAdder = ia;
            roomTypes = new Dictionary<string, RoomType>();
            roomTypes["Sala za pregled"] = RoomType.APPOINTMENT_ROOM;
            roomTypes["Spavaća soba"] = RoomType.BREAK_ROOM;
            roomTypes["Skladište"] = RoomType.STORAGE_ROOM;
            roomTypes["Operaciona sala"] = RoomType.OPERATING_ROOM;
            this.roomTypeComboBox.ItemsSource = roomTypes.Keys;
            setOkButtonVisibility();
        }

        private void setOkButtonVisibility()
        {
            if((roomIdTextBox.Text != String.Empty) && (roomNameTextBox.Text != String.Empty) && (roomTypeComboBox.SelectedIndex > -1) && ((yesRadioButton.IsChecked == true) || (noRadioButton.IsChecked == true)))
            {
                okButton.IsEnabled = true;
            }
            else
            {
                okButton.IsEnabled = false;
            }
        }

        private void checkOkButton(object sender, TextChangedEventArgs e)
        {
            setOkButtonVisibility();
        }

        private void checkOkButton(object sender, RoutedEventArgs e)
        {
            setOkButtonVisibility();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdder)
            {
                switch (roomTypes[(string)roomTypeComboBox.SelectedItem])
                {
                    case RoomType.APPOINTMENT_ROOM:
                        if (!res.AppointmentRooms.ContainsKey(Int32.Parse(roomIdTextBox.Text)))
                        {

                        }
                        break;
                    case RoomType.OPERATING_ROOM:
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}
