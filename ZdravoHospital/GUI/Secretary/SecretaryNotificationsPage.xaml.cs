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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryNotificationsPage.xaml
    /// </summary>
    public partial class SecretaryNotificationsPage : Page
    {
        public ObservableCollection<Model.Notification> Notifications { get; set; }
        public SecretaryNotificationsPage()
        {
            InitializeComponent();
            this.DataContext = this;
            Model.Resources.OpenNotifications();
            Notifications = new ObservableCollection<Notification>(Model.Resources.notifications);
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
            if(NotificationsListView.SelectedItem != null)
            {
                NavigationService.Navigate(new EditNotificationPage((Model.Notification)NotificationsListView.SelectedItem));
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
            if(NotificationsListView.SelectedItem != null)
            {
                Notification notification = (Notification)NotificationsListView.SelectedItem;
                Notifications.Remove(notification);
                Model.Resources.notifications.Remove(notification);

                this.removePersonNotifications(notification.NotificationId);

                Model.Resources.SaveNotifications();
                CollectionViewSource.GetDefaultView(NotificationsListView.ItemsSource).Refresh();
            }
            
        }
    }
}
