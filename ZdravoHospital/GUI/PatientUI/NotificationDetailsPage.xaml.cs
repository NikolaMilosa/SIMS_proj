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
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for NotificationDetailsPage.xaml
    /// </summary>
    public partial class NotificationDetailsPage : Page
    {
        public NotificationView NotificationView { get; set; }

        public string PatientUsername { get; set; }
        public NotificationDetailsPage(NotificationView notificationView,string username)
        {
            InitializeComponent();
            PatientUsername = username;
            NotificationView = notificationView;
            NotificationView.Notification.UsernameRecievers[username] = true;
            Model.Resources.SaveNotifications();
            DataContext = this;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NotificationsPage(PatientUsername));
        }
    }
}
