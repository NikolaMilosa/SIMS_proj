using Model;
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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for EditNotificationPage.xaml
    /// </summary>
    public partial class EditNotificationPage : Page, INotifyPropertyChanged
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
        public AccountsGeneralService AccountsService { get; set; }

        public Model.Notification SelectedNotification { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void InitializeBindingFields()
        {
            NotificationDTO.NotificationTitle = SelectedNotification.Title;
            NotificationDTO.NotificationText = SelectedNotification.Text;
        }
        
        
        public void FindNotificationRoles()
        {
            int managerNum = 0;
            int secretaryNum = 0;
            int doctorNum = 0;
            int patientNum = 0;
            List<Credentials> accounts = AccountsService.GetAccounts();
            List<PersonNotification> personNotifications = NotificationService.GetPersonNotifications();
            foreach(var notification in personNotifications)
            {
                if(SelectedNotification.NotificationId == notification.NotificationId)
                {
                    switch (AccountsService.FindRoleByUsername(notification.Username))
                    {
                        case RoleType.SECERATRY:
                            secretaryNum++;
                            break;
                        case RoleType.PATIENT:
                            patientNum++;
                            break;
                        case RoleType.MANAGER:
                            managerNum++;
                            break;
                        case RoleType.DOCTOR:
                            doctorNum++;
                            break;
                        default:
                            break;
                    }
                }
                
            }
            if (managerNum == AccountsService.GetRoleCount(RoleType.MANAGER))
                NotificationDTO.ManagerChecked = true;
            if (secretaryNum == AccountsService.GetRoleCount(RoleType.SECERATRY))
                NotificationDTO.SecretaryChecked = true;
            if (doctorNum == AccountsService.GetRoleCount(RoleType.DOCTOR))
                NotificationDTO.DoctorChecked = true;
            if (patientNum == AccountsService.GetRoleCount(RoleType.PATIENT))
                NotificationDTO.PatientChecked = true;
        }
        public EditNotificationPage(Model.Notification selectedNotification)
        {
            InitializeComponent();
            this.DataContext = this;
            AccountsService = new AccountsGeneralService();
            NotificationService = new NotificationService();
            SelectedNotification = selectedNotification;
            NotificationDTO = new NotificationDTO();
            InitializeBindingFields();
            FindNotificationRoles();
        }
        

        private void SendNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            //NotificationService.ProcessNotificationUpdate(NotificationDTO, SelectedNotification.NotificationId);
            //NavigationService.Navigate(new SecretaryNotificationsPage());

            if (NotificationDTO.ManagerChecked == false && NotificationDTO.DoctorChecked == false && NotificationDTO.SecretaryChecked == false && NotificationDTO.PatientChecked == false && NotificationDTO.CustomRecipients.Count == 0)
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Warning", "There must be some recipients.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
            else
            {
                if (NotificationDTO.NotificationTitle == null || NotificationDTO.NotificationTitle == "" || NotificationDTO.NotificationText == null || NotificationDTO.NotificationText == "")
                {
                    SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Warning", "Title and text are required fields.");
                    SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                    SecretaryWindowVM.CustomMessageBox.Show();
                }
                else
                {
                    NotificationService.ProcessNotificationUpdate(NotificationDTO, SelectedNotification.NotificationId);
                    NavigationService.Navigate(new SecretaryNotificationsPage());
                }

            }
        }

        private void RemoveCustomRecipient_Click(object sender, RoutedEventArgs e)
        {
            string selectedRecipient = (sender as Button).DataContext as string;
            NotificationService.RemoveCustomRecipient(NotificationDTO, selectedRecipient);


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
    }
}
