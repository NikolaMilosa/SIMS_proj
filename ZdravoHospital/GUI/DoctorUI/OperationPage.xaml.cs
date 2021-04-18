using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for OperationPage.xaml
    /// </summary>
    public partial class OperationPage : Page
    {
        public Doctor Doctor { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        private Period period;

        public OperationPage(Period period, bool readonlyMode)
        {
            InitializeComponent();

            this.DataContext = this;

            this.period = period;

            Doctor = Model.Resources.doctors[period.DoctorUsername];
            Patients = new ObservableCollection<Patient>(Model.Resources.patients.Values);
            Model.Resources.OpenRooms();
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values.Where(room => room.RoomType == RoomType.OPERATING_ROOM));

            PatientsComboBox.SelectedItem = Model.Resources.patients[period.PatientUsername];
            AppointmentDatePicker.SelectedDate = period.StartTime.Date;
            StartTimeTextBox.Text = period.StartTime.ToString("HH:mm");
            DurationTextBox.Text = period.Duration.ToString();
            RoomsComboBox.SelectedItem = Model.Resources.rooms[period.RoomId];

            if (readonlyMode)
            {
                PatientsComboBox.IsEnabled = false;
                AppointmentDatePicker.IsEnabled = false;
                StartTimeTextBox.IsEnabled = false;
                DurationTextBox.IsEnabled = false;
                RoomsComboBox.IsEnabled = false;
                CancelOperationButton.IsEnabled = false;
            }

            if (DateTime.Now >= period.StartTime)
                CancelOperationButton.IsEnabled = false;
            else
                OperationReportButton.IsEnabled = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
                return;

            string[] parts = StartTimeTextBox.Text.Split(':');
            int hours = Int32.Parse(parts[0]);
            int minutes = Int32.Parse(parts[1]);
            DateTime dateTime = new DateTime(AppointmentDatePicker.SelectedDate.Value.Year,
                                    AppointmentDatePicker.SelectedDate.Value.Month,
                                    AppointmentDatePicker.SelectedDate.Value.Day,
                                    hours, minutes, 0);

            Period editedPeriod = new Period(dateTime, Int32.Parse(DurationTextBox.Text), PeriodType.OPERATION,
                                       (PatientsComboBox.SelectedItem as Patient).Username,
                                       Doctor.Username,
                                       (RoomsComboBox.SelectedItem as Room).Id,
                                       this.period.PrescriptionId);

            int available = IsPeriodAvailable(editedPeriod, this.period);

            if (available == 0)
            {
                foreach (Period existingPeriod in Model.Resources.periods)
                {
                    if (existingPeriod.RoomId == this.period.RoomId && existingPeriod.StartTime == this.period.StartTime)
                    {
                        Model.Resources.periods.Remove(existingPeriod);
                        break;
                    }
                }

                Model.Resources.periods.Add(editedPeriod);
                Model.Resources.SavePeriods();

                this.period = editedPeriod;

                MessageBox.Show("Operation edited successfully.", "Success");
            }
            else if (available == 1)
            {
                MessageBox.Show("Selected room is unavailable in selected period.", "Room unavailable");
            }
            else if (available == 2)
            {
                MessageBox.Show("Selected doctor is unavailable in selected period.", "Doctor unavailable");
            }
            else
            {
                MessageBox.Show("Selected patient is unavailable in selected period.", "Patient unavailable");
            }
        }

        private bool IsInputValid()
        {
            if (PatientsComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select patient.", "Invalid input");
                return false;
            }

            Regex regex = new Regex(@"^\d{2}:\d{2}$");

            if (!regex.IsMatch(StartTimeTextBox.Text))
            {
                MessageBox.Show("Please enter start time in correct format (HH:mm).", "Invalid input");
                return false;
            }

            string[] parts = StartTimeTextBox.Text.Split(':');
            int hours = Int32.Parse(parts[0]);
            int minutes = Int32.Parse(parts[1]);

            if (hours > 24 || minutes > 60)
            {
                MessageBox.Show("Please enter valid start time.", "Invalid input");
                return false;
            }

            regex = new Regex(@"^\d+$");

            if (!regex.IsMatch(DurationTextBox.Text))
            {
                MessageBox.Show("Please enter duration in correct format (numbers only).", "Invalid input");
                return false;
            }

            if (RoomsComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select operation room.", "Invalid input");
                return false;
            }

            return true;
        }

        private int IsPeriodAvailable(Period period, Period periodToIgnore) // vraca 0 ako je termin ok, 1 ako je soba zauzeta, 2 ako je doktor zauzet, 3 ako je pacijent zauzet
        {
            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);

            foreach (Period existingPeriod in Model.Resources.periods)
            {
                if (existingPeriod.RoomId == periodToIgnore.RoomId && existingPeriod.StartTime == periodToIgnore.StartTime)
                    continue;

                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.RoomId == existingPeriod.RoomId)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 1;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 1;
                }

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 2;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 2;
                }

                if (period.PatientUsername == existingPeriod.PatientUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 3;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 3;
                }
            }

            return 0;
        }

        private void CancelOperationButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the operation?\nThis action cannot be undone.",
                                                      "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                foreach (Period existingPeriod in Model.Resources.periods)
                {
                    if (existingPeriod.RoomId == this.period.RoomId && existingPeriod.StartTime == this.period.StartTime)
                    {
                        Model.Resources.periods.Remove(existingPeriod);
                        break;
                    }
                }

                Model.Resources.SavePeriods();

                MessageBox.Show("Operation canceled successfully.", "Success");
                NavigationService.GoBack();
            }
        }

        private void OperationReportButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PeriodDetailsPage(this.period));
        }

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PatientInfoPage(PatientsComboBox.SelectedItem as Patient));
        }
    }
}
