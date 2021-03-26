using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        Resources res;

        public ManagerWindow(Resources r)
        {
            InitializeComponent();
            this.res = r;
        }

        private void roomButton_Click(object sender, RoutedEventArgs e)
        {
            SubMenu.Content = new SubMenuRooms(this.res,managerMainTable);
        }

    }
}
