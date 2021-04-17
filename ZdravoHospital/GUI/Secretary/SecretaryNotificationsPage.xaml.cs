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
            Notifications = new ObservableCollection<Model.Notification>();
            Model.Resources.OpenNotifications();
            ListToObservableCollection(Model.Resources.notifications);
        }

        private void ListToObservableCollection(List<Model.Notification> list)
        {
            foreach(var item in list)
            {
                Notifications.Add(item);
            }
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

        private void DeleteNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            if(NotificationsListView.SelectedItem != null)
            {
                Notification notification = (Notification)NotificationsListView.SelectedItem;
                Notifications.Remove(notification);
                Model.Resources.notifications.Remove(notification);

                Model.Resources.SaveNotifications();
                CollectionViewSource.GetDefaultView(NotificationsListView.ItemsSource).Refresh();
            }
            
        }
    }
}
