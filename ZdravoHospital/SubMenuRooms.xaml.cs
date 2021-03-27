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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for SubMenuRooms.xaml
    /// </summary>
    public partial class SubMenuRooms : Page
    {

        ManagerWindow managerWindow;
        public SubMenuRooms(ManagerWindow mw)
        {
            InitializeComponent();
            this.managerWindow = mw;
        }

        private void showButton_Click(object sender, RoutedEventArgs e)
        {
            managerWindow.showRooms();
        }

        private void addRoomButton_Click(object sender, RoutedEventArgs e)
        {
            RoomAddOrEditDialog addDialog = new RoomAddOrEditDialog(managerWindow, true);
            addDialog.roomIdTextBox.IsEnabled = true;
            addDialog.Show();
        }
    }
}
