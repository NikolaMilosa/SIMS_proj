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
    /// Interaction logic for Warning.xaml
    /// </summary>
    public partial class Warning : Window
    {
        ManagerWindow managerWindow;

        public Warning(ManagerWindow mw)
        {
            InitializeComponent();
            this.managerWindow = mw;

            if (managerWindow.Active == ManagerWindow.ActiveTable.ROOMS)
                questionLabel.Content = "If you delete this item it will be permanently lost. Proceed?";
        }

        private void yesClick(object sender, RoutedEventArgs e)
        {
            int key;
            if (managerWindow.managerMainTable.SelectedItem != null)
            {
                key = ((Room)managerWindow.managerMainTable.SelectedItem).Id;
                if (managerWindow.Res.AppointmentRooms.ContainsKey(key))
                    managerWindow.Res.AppointmentRooms.Remove(key);
                else if (managerWindow.Res.OperatingRooms.ContainsKey(key))
                    managerWindow.Res.OperatingRooms.Remove(key);
                else
                    managerWindow.Res.OperatingRooms.Remove(key);

                managerWindow.drawRooms();
                this.Close();
            }
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
