using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class SecretaryNotificationsVM
    {
        public NotificationService NotificationService { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }
        public SecretaryNotificationsVM()
        {
            NotificationService = new NotificationService();
            Notifications = new ObservableCollection<Notification>(NotificationService.GetAllNotifications());
            initializeCommands();
        }

        public ICommand NewNotificationCommand { get; set; }
        public ICommand EditNotificationCommand { get; set; }
        public ICommand DeleteNotificationCommand { get; set; }

        private void newNotificationExecute(object parameter)
        {
            SecretaryWindowVM.NavigationService.Navigate(new NewNotificationPage());
        }

        private void editNotificationExecute(object parameter)
        {
            if (SelectedNotification != null)
            {
                SecretaryWindowVM.NavigationService.Navigate(new EditNotificationPage(SelectedNotification));
            }
        }

        private void deleteNotificationExecute(object parameter) 
        {
            if (SelectedNotification != null)
            {
                NotificationService.RemoveNotification(SelectedNotification.NotificationId);
                Notifications.Remove(SelectedNotification);
            }
        }

        private void initializeCommands()
        {
            NewNotificationCommand = new RelayCommand(newNotificationExecute);
            EditNotificationCommand = new RelayCommand(editNotificationExecute);
            DeleteNotificationCommand = new RelayCommand(deleteNotificationExecute);
        }
    }
}
