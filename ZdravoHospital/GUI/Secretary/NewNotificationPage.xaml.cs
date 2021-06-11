using Repository.CredentialsPersistance;
using Repository.NotificationsPersistance;
using Repository.PersonNotificationPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for NewNotificationPage.xaml
    /// </summary>
    public partial class NewNotificationPage : Page, INotifyPropertyChanged
    {
        private string _customRecipient;

        public string CustomRecipient
        {
            get => _customRecipient;
            set
            {
                _customRecipient = value;
                OnPropertyChanged("CustomRecipient");
            }
        }
        public NotificationDTO NotificationDTO { get; set; }
        public NotificationService NotificationService { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public NewNotificationPage()
        {
            InitializeComponent();
            NotificationDTO = new NotificationDTO();

            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            INotificationsRepository notificationsRepository = RepositoryFactory.CreateNotificationRepository();
            IPersonNotificationRepository personNotificationRepository = RepositoryFactory.CreatePersonNotificationRepository();
            NotificationService = new NotificationService(notificationsRepository, personNotificationRepository, credentialsRepository);
            
            this.DataContext = this;
        }


        private void SendNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotificationDTO.ManagerChecked == false && NotificationDTO.DoctorChecked == false && NotificationDTO.SecretaryChecked == false && NotificationDTO.PatientChecked == false && NotificationDTO.CustomRecipients.Count == 0)
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Warning", "There must be some recipients.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
            else
            {
                if(NotificationDTO.NotificationTitle == null || NotificationDTO.NotificationTitle == "" || NotificationDTO.NotificationText == null || NotificationDTO.NotificationText == "")
                {
                    SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Warning", "Title and text are required fields.");
                    SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                    SecretaryWindowVM.CustomMessageBox.Show();
                }
                else
                {
                    NotificationService.ProcessNotificationSend(NotificationDTO);
                    NavigationService.Navigate(new SecretaryNotificationsPage());
                }
                
            }
            
        }

        private void CustomRecipientTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                bool success = NotificationService.ProcessCustomRecipients(NotificationDTO, CustomRecipient.Trim());
                if (success)
                    CustomRecipient = "";
                else
                    new Thread(changeLabel).Start();

            }
        }

        private void changeLabel()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                HintLabel.Text = "Patient not registered. ";
                HintLabel.Foreground = Brushes.Red;
                
            }));
            Thread.Sleep(2000);
            this.Dispatcher.Invoke((Action)(() =>
            {
                HintLabel.Text = "Press enter to add new recipient. ";
                HintLabel.Foreground = Brushes.Black;

            }));
        }


        private void RemoveCustomRecipient_Click(object sender, RoutedEventArgs e)
        {
            string selectedRecipient = (sender as Button).DataContext as string;
            NotificationService.RemoveCustomRecipient(NotificationDTO, selectedRecipient);
        }
    }
}
