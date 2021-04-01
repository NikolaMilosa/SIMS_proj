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
    }
}
