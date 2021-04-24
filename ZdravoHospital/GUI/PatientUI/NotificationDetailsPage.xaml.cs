using Model;
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
           foreach(PersonNotification personNotification in Model.Resources.personNotifications)
            {
                if (personNotification.NotificationId.Equals(NotificationView.Notification.NotificationId) && personNotification.Username.Equals(username))
                {
                    personNotification.IsRead = true;
                    break;
                }
            }
            Model.Resources.SavePersonNotifications();
            DataContext = this;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NotificationsPage(PatientUsername));
        }
    }
}
