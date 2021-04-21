using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for EditNotificationPage.xaml
    /// </summary>
    public partial class EditNotificationPage : Page, INotifyPropertyChanged
    {
        private string _notificationTitle;
        private string _notificationText;
        private string _customRecipient;
        public Dictionary<string, bool> Recipients;
        public Model.Notification SelectedNotification { get; set; }

        public string NotificationTitle
        {
            get => _notificationTitle;
            set
            {
                _notificationTitle = value;
                OnPropertyChanged("NotificationTitle");
            }
        }
        public string NotificationText
        {
            get => _notificationText;
            set
            {
                _notificationText = value;
                OnPropertyChanged("NotificationText");
            }
        }
        public string CustomRecipient
        {
            get => _customRecipient;
            set
            {
                _customRecipient = value;
                OnPropertyChanged("CustomRecipient");
            }
        }

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
            NotificationTitle = SelectedNotification.Title;
            NotificationText = SelectedNotification.Text;
        }
        public int GetRoleCount(Dictionary<string, Model.Credentials> accounts, Model.RoleType role)
        {
            int count = 0;
            foreach (KeyValuePair<string, Model.Credentials> entry in accounts)
            {
                if (entry.Value.Role.Equals(role))
                {
                    count++;
                }
            }
            return count;
        }
        public void FindNotificationRoles()
        {
            int managerNum = 0;
            int secretaryNum = 0;
            int doctorNum = 0;
            int patientNum = 0;
            Model.Resources.OpenAccounts();

            foreach (KeyValuePair<string, Model.Credentials> entry in Model.Resources.accounts)
            {
                /*if (SelectedNotification.UsernameRecievers.ContainsKey(entry.Key))
                {
                    switch (entry.Value.Role)
                    {
                        case Model.RoleType.SECERATRY:
                            secretaryNum++;
                            break;
                        case Model.RoleType.PATIENT:
                            patientNum++;
                            break;
                        case Model.RoleType.MANAGER:
                            managerNum++;
                            break;
                        case Model.RoleType.DOCTOR:
                            doctorNum++;
                            break;
                        default:
                            break;
                    }
                }   */
                if(managerNum == GetRoleCount(Model.Resources.accounts, Model.RoleType.MANAGER))
                    ManagerCheckBox.IsChecked = true;
                if (secretaryNum == GetRoleCount(Model.Resources.accounts, Model.RoleType.SECERATRY))
                    SecretaryCheckBox.IsChecked = true;
                if (doctorNum == GetRoleCount(Model.Resources.accounts, Model.RoleType.DOCTOR))
                    DoctorCheckBox.IsChecked = true;
                if (patientNum == GetRoleCount(Model.Resources.accounts, Model.RoleType.PATIENT))
                    PatientCheckBox.IsChecked = true;

            }

        }
        public EditNotificationPage(Model.Notification selectedNotification)
        {
            InitializeComponent();
            this.DataContext = this;
            Recipients = new Dictionary<string, bool>();
            SelectedNotification = selectedNotification;
            //InitializeBindingFields();
            //FindNotificationRoles();
        }
        private void FillRecipients()
        {
            if (Model.Resources.accounts != null)
                Model.Resources.OpenAccounts();

            bool customRecipientExists = false;

            foreach (KeyValuePair<string, Model.Credentials> entry in Model.Resources.accounts)
            {
                if ((entry.Value.Role.Equals(Model.RoleType.MANAGER) && (bool)ManagerCheckBox.IsChecked)
                    || (entry.Value.Role.Equals(Model.RoleType.SECERATRY) && (bool)SecretaryCheckBox.IsChecked)
                    || (entry.Value.Role.Equals(Model.RoleType.DOCTOR) && (bool)DoctorCheckBox.IsChecked)
                    || (entry.Value.Role.Equals(Model.RoleType.PATIENT) && (bool)PatientCheckBox.IsChecked))
                {
                    if (!entry.Key.Equals(CustomRecipient))
                        Recipients.Add(entry.Key, false);
                }
                if (entry.Key.Equals(CustomRecipient))
                    customRecipientExists = true;
            }
            if (CustomRecipient != null && customRecipientExists)
            {
                Recipients.Add(CustomRecipient, false);
            }

        }

        private void SendNotificationButton_Click(object sender, RoutedEventArgs e)
        {
           /* FillRecipients();
            Model.Notification newNotification = new Model.Notification(NotificationText, DateTime.Now, SecretaryWindow.SecretaryUsername, NotificationTitle + "(edit)", Recipients);

            Model.Resources.OpenNotifications();
            if (Model.Resources.notifications == null)
                Model.Resources.notifications = new List<Model.Notification>();

            Model.Resources.notifications.Add(newNotification);
            Model.Resources.SaveNotifications();

            NavigationService.Navigate(new SecretaryNotificationsPage());*/
        }
        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
