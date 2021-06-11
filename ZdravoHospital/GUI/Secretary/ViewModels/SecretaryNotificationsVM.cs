using Model;
using Repository.CredentialsPersistance;
using Repository.NotificationsPersistance;
using Repository.PersonNotificationPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.Factory;
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
            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            INotificationsRepository notificationsRepository = RepositoryFactory.CreateNotificationRepository();
            IPersonNotificationRepository personNotificationRepository = RepositoryFactory.CreatePersonNotificationRepository();
            NotificationService = new NotificationService(notificationsRepository, personNotificationRepository, credentialsRepository);

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
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select a notification first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }

        private void deleteNotificationExecute(object parameter) 
        {
            if (SelectedNotification != null)
            {
                SecretaryWindowVM.CustomYesNoDialog = new CustomYesNoDialog("Are you sure?", "Action cannot be undone.");
                SecretaryWindowVM.CustomYesNoDialog.Owner = SecretaryWindowVM.SecretaryWindow;

                if ((bool)SecretaryWindowVM.CustomYesNoDialog.ShowDialog())
                {
                    NotificationService.RemoveNotification(SelectedNotification.NotificationId);
                    Notifications.Remove(SelectedNotification);
                }
                
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select a notification first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
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
