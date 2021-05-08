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
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                SecretaryMainFrame.Content = new PatientRegistrationPage();
                CloseMenu_BeginStoryboard.Storyboard.Begin();
            }

        }

        private void SeePatientsItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                SecretaryMainFrame.Content = new PatientsView();
                CloseMenu_BeginStoryboard.Storyboard.Begin();
            }
        }

        private void GuestItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                SecretaryMainFrame.Content = new GuestAccountPage(false);
                CloseMenu_BeginStoryboard.Storyboard.Begin();
            }

        }

        private void NotificationsItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                SecretaryMainFrame.Content = new SecretaryNotificationsPage();
                CloseMenu_BeginStoryboard.Storyboard.Begin();
            }
        }

        private void PeriodsItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                SecretaryMainFrame.Content = new SecretaryPeriodsPage();
                CloseMenu_BeginStoryboard.Storyboard.Begin();
            }
        }

        private void UrgentPeriodItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                SecretaryMainFrame.Content = new SecretaryUrgentPeriodPage();
                CloseMenu_BeginStoryboard.Storyboard.Begin();
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            SecretaryMainFrame.Content = new SecretaryHomePage();
        }

        private void PatientsViewButton_Click(object sender, RoutedEventArgs e)
        {
            SecretaryMainFrame.Content = new PatientsView();
        }

        private void PeriodsButton_Click(object sender, RoutedEventArgs e)
        {
            SecretaryMainFrame.Content = new SecretaryPeriodsPage();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
