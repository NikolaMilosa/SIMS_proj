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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for NewNotificationPage.xaml
    /// </summary>
    public partial class NewNotificationPage : Page, INotifyPropertyChanged
    {
        private string _notificationTitle;
        private string _notificationText;
        private string _customRecipient;
        private bool _managerChecked;
        private bool _secretaryChecked;
        private bool _doctorChecked;
        private bool _patientChecked;

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
        public bool ManagerChecked
        {
            get => _managerChecked;
            set
            {
                _managerChecked = value;
                OnPropertyChanged("ManagerChecked");
            }
        }
        public bool SecretaryChecked
        {
            get => _secretaryChecked;
            set
            {
                _secretaryChecked = value;
                OnPropertyChanged("SecretaryChecked");
            }
        }
        public bool DoctorChecked
        {
            get => _doctorChecked;
            set
            {
                _doctorChecked = value;
                OnPropertyChanged("DoctorChecked");
            }
        }
        public bool PatientChecked
        {
            get => _patientChecked;
            set
            {
                _patientChecked = value;
                OnPropertyChanged("PatientChecked");
            }
        }
        public List<string> Recipients { get; set; }
        public ObservableCollection<string> CustomRecipients { get; set; }

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
            this.DataContext = this;
            Recipients = new List<string>();
            CustomRecipients = new ObservableCollection<string>();
        }
        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void FillRecipients()
        {
            if(Model.Resources.accounts != null)
                Model.Resources.OpenAccounts();

            foreach (KeyValuePair<string, Model.Credentials> entry in Model.Resources.accounts)
            {
                if ((entry.Value.Role.Equals(Model.RoleType.MANAGER) && ManagerChecked)
                    || (entry.Value.Role.Equals(Model.RoleType.SECERATRY) && SecretaryChecked)
                    || (entry.Value.Role.Equals(Model.RoleType.DOCTOR) && DoctorChecked)
                    || (entry.Value.Role.Equals(Model.RoleType.PATIENT) && PatientChecked))
                {
                    Recipients.Add(entry.Key);
                }
            }
            addCustomRecipients();
        }

        private void addCustomRecipients()
        {
            foreach(string username in CustomRecipients)
            {
                if(!Recipients.Contains(username))
                    Recipients.Add(username);
            }
        }

        private int calculateNotificationId(List<Model.Notification> notifications)
        {
            if (notifications.Count == 0)
                return 1;
            else
            {
                return notifications[notifications.Count - 1].NotificationId + 1;
            }
        }
        private void SendNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            FillRecipients();
           
            Model.Resources.OpenNotifications();
            Model.Resources.OpenPersonNotifications();

            int notificationId = this.calculateNotificationId(Model.Resources.notifications);
            Model.Notification newNotification = new Model.Notification(NotificationText, DateTime.Now, SecretaryWindow.SecretaryUsername, NotificationTitle, notificationId);

            Model.Resources.notifications.Add(newNotification);
            Model.Resources.SaveNotifications();

            foreach(var recipient in Recipients)
            {
                Model.PersonNotification personNotification = new Model.PersonNotification(recipient, notificationId, false);
                Model.Resources.personNotifications.Add(personNotification);
            }
            Model.Resources.SavePersonNotifications();

            NavigationService.Navigate(new SecretaryNotificationsPage());
        }

        private void CustomRecipientTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CustomRecipient = CustomRecipient.Trim();
                if (isPatientRegistered(CustomRecipient))
                {
                    if (!CustomRecipients.Contains(CustomRecipient))
                    {
                        CustomRecipients.Add(CustomRecipient);
                        CustomRecipientTextBox.Text = "";
                    }
                }
                else
                {
                    new Thread(changeLabel).Start();

                }
                
                
            }
            // add feedback message
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

        private bool isPatientRegistered(string username)
        {
            Model.Resources.OpenAccounts();
            foreach(KeyValuePair<string, Model.Credentials> entry in Model.Resources.accounts)
            {
                if(entry.Key.Equals(username) && entry.Value.Role == Model.RoleType.PATIENT)
                {
                    return true;
                }
            }
            return false;
        }

        private void RemoveCustomRecipient_Click(object sender, RoutedEventArgs e)
        {
            string selectedRecipient = (sender as Button).DataContext as string;
            CustomRecipients.Remove(selectedRecipient);
        }
    }
}
