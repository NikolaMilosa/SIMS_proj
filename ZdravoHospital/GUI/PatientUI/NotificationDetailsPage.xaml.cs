using Model;
using Model.Repository;
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
using ZdravoHospital.GUI.PatientUI.DTOs;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for NotificationDetailsPage.xaml
    /// </summary>
    public partial class NotificationDetailsPage : Page
    {
        public NotificationDTO NotificationView { get; set; }

        public string PatientUsername { get; set; }
        public NotificationDetailsPage(NotificationDTO notificationView, string username)
        {
            InitializeComponent();
            SetProperties(notificationView, username);
            SerializeReadNotification();
            DataContext = this;
        }

        private void SetProperties(NotificationDTO notificationView, string username)
        {
            PatientUsername = username;
            NotificationView = notificationView;
            NotificationView.Seen = true;
        }

        private void SerializeReadNotification()
        {

            PersonNotificationRepository personNotificationRepository = new PersonNotificationRepository();
            personNotificationRepository.Update(GetPersonNotification());
        }

        private PersonNotification GetPersonNotification()
        {
            PersonNotificationRepository personNotificationRepository = new PersonNotificationRepository();
            List<PersonNotification> personNotifications = personNotificationRepository.GetValues();
            foreach (PersonNotification personNotification in personNotifications)
            {
                if (personNotification.NotificationId.Equals(NotificationView.Notification.NotificationId))
                {
                    return personNotification; 
                }
            }
            return null;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NotificationsPage(PatientUsername));
        }
    }
}
