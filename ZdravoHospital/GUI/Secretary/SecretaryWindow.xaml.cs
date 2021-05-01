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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryWindow.xaml
    /// </summary>
    public partial class SecretaryWindow : Window
    {
        public static string SecretaryUsername;
        public SecretaryWindow(string username)
        {
            InitializeComponent();
            SecretaryUsername = username;
            this.SecretaryMainFrame.Content = new SecretaryHomePage();
        }

        private void AddPatientItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void SeePatientsItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void GuestItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void NotificationsItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void PeriodsItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
