using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryNotificationsPage.xaml
    /// </summary>
    public partial class SecretaryNotificationsPage : Page
    {
        public NotificationService NotificationService { get; set; }
        public ObservableCollection<Model.Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }
        public SecretaryNotificationsPage()
        {
            InitializeComponent();
            this.DataContext = this;
            NotificationService = new NotificationService();
            Notifications = new ObservableCollection<Notification>(NotificationService.GetAllNotifications());
        }

        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NewNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewNotificationPage());
        }

        private void EditNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedNotification != null)
            {
                NavigationService.Navigate(new EditNotificationPage(SelectedNotification));
            }
        }
        private void removePersonNotifications(int id)
        {
            Model.Resources.OpenPersonNotifications();
            Model.Resources.personNotifications.RemoveAll(elem => elem.NotificationId == id);
            Model.Resources.SavePersonNotifications();
        }

        private void DeleteNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedNotification != null)
            {
                NotificationService.RemoveNotification(SelectedNotification.NotificationId);
                Notifications.Remove(SelectedNotification);
            }
        }
    }
}
