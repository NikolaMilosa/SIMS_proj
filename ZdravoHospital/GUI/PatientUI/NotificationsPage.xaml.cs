using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : Page
    {
        public ObservableCollection<NotificationDTO> NotificationList { get; set; }
        
        public string PatientUsername { get; set; }
        public NotificationsPage(string username)
        {
            InitializeComponent();
            DataContext = this;
            PatientUsername = username;
            FillList();
        }

        private void FillList()
        {
            NotificationList = new ObservableCollection<NotificationDTO>();
            PersonNotificationRepository personNotificationRepository = new PersonNotificationRepository();
            List<PersonNotification> personNotifications = personNotificationRepository.GetValues();
            foreach(PersonNotification personNotification in personNotifications)
            {
                if (personNotification.Username.Equals(PatientUsername))
                {
                    NotificationList.Add(new NotificationDTO(personNotification, PatientUsername));
                }
            }
        }

        private void NotificationDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NotificationDetailsPage((NotificationDTO)notificationDataGrid.SelectedItem, PatientUsername));
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateNotePage(PatientUsername));
        }
    }
}
