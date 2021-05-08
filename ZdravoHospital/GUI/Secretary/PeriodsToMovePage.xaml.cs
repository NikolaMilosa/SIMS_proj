using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    /// Interaction logic for PeriodsToMovePage.xaml
    /// </summary>
    public partial class PeriodsToMovePage : Page
    {
        public ObservableCollection<Model.Period> Periods { get; set; }

        private Period _selectedPeriod;
        public Period SelectedPeriod
        {
            get { return _selectedPeriod; }
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged("SelectedPeriod");
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
        public PeriodsToMovePage(List<Model.Period> periods)
        {
            Periods = new ObservableCollection<Model.Period>(periods);
            this.sortPeriods();
            this.DataContext = this;
            InitializeComponent();
        }

        private void sortPeriods()
        {
            Periods = new ObservableCollection<Period>(Periods.OrderBy(x => x.MovePeriods.Count).ThenBy(x => x.findSumOfMovePeriods()).ToList<Period>());
        }


        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Model.Resources.OpenPeriods();
            if(SelectedPeriod != null && PeriodsListView.SelectedItem != null)
            {
                foreach(var movePeriod in SelectedPeriod.MovePeriods)
                {
                    foreach(var period in Model.Resources.periods)
                    {
                        if(movePeriod.InitialStartTime == period.StartTime && movePeriod.RoomId == period.RoomId)
                        {
                            period.StartTime = movePeriod.MovedStartTime;
                            sendPostponeNotification(movePeriod, movePeriod.PatientUsername);
                            sendPostponeNotification(movePeriod, movePeriod.DoctorUsername);
                        }
                    }
                }

                Model.Resources.periods.Add(SelectedPeriod);
                Model.Resources.SavePeriods();

                sendDoctorNotification(SelectedPeriod);

                NavigationService.Navigate(new UrgentPeriodSummaryPage(SelectedPeriod));
            }
        }


        private string createDoctorNotificationText(Period urgentPeriod)
        {
            StringBuilder urgentPeriodNotification = new StringBuilder();
            urgentPeriodNotification.Append("URGENT PERIOD BOOKED AT ").Append(urgentPeriod.StartTime.ToString()).Append(" IN ROOM ")
                .Append(urgentPeriod.RoomId);

            return urgentPeriodNotification.ToString();
        }
        private string createPostponeNotificationText(MovePeriod movePeriod, string usernameReceiver)
        {
            StringBuilder notificationText = new StringBuilder();
            notificationText.Append("Dear ").Append(usernameReceiver).Append(", your appointment has been postponed from ")
                .Append(movePeriod.InitialStartTime.ToString()).Append(" to ").Append(movePeriod.MovedStartTime.ToString())
                .Append(" due to urgent appointment. If you are dissatisfied with new appointment, please contact us for rescheduling.");

            return notificationText.ToString();
        }

        private void sendDoctorNotification(Period urgentPeriod)
        {
            Model.Resources.OpenPersonNotifications();
            Model.Resources.OpenNotifications();

            int notificationId = this.calculateNotificationId(Model.Resources.notifications);
            string notificationText = createDoctorNotificationText(urgentPeriod);
            string notificationTitle = "URGENT";
            Model.Notification newNotification = new Model.Notification(notificationText, DateTime.Now, SecretaryWindow.SecretaryUsername, notificationTitle, notificationId);
            Model.Resources.notifications.Add(newNotification);
            Model.Resources.SaveNotifications();

            Model.PersonNotification personNotification = new Model.PersonNotification(urgentPeriod.DoctorUsername, notificationId, false);
            Model.Resources.personNotifications.Add(personNotification);

            Model.Resources.SavePersonNotifications();
        }
        private void sendPostponeNotification(MovePeriod movePeriod, string usernameReceiver)
        {
            Model.Resources.OpenPersonNotifications();
            Model.Resources.OpenNotifications();

            int notificationId = this.calculateNotificationId(Model.Resources.notifications);
            string notificationText = createPostponeNotificationText(movePeriod, usernameReceiver);
            string notificationTitle = "Rescheduling due to urgent appointment";
            Model.Notification newNotification = new Model.Notification(notificationText, DateTime.Now, SecretaryWindow.SecretaryUsername, notificationTitle, notificationId);
            Model.Resources.notifications.Add(newNotification);
            Model.Resources.SaveNotifications();

            Model.PersonNotification personNotification = new Model.PersonNotification(usernameReceiver, notificationId, false);
            Model.Resources.personNotifications.Add(personNotification);

            Model.Resources.SavePersonNotifications();
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
    }
}
