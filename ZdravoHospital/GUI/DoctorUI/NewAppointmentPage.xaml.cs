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
    /// Interaction logic for NewAppointmentPage.xaml
    /// </summary>
    public partial class NewAppointmentPage : Page
    {
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        public NewAppointmentPage(Doctor doctor, DateTime startTime, int duration)
        {
            InitializeComponent();

            this.DataContext = this;

            Doctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values);
            Patients = new ObservableCollection<Patient>(Model.Resources.patients.Values);
            Model.Resources.OpenRooms();
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values.Where(room => room.RoomType == RoomType.APPOINTMENT_ROOM));

            DoctorsComboBox.SelectedItem = doctor;
            AppointmentDatePicker.SelectedDate = startTime.Date;
            StartTimeTextBox.Text = startTime.ToString("HH:mm");
            DurationTextBox.Text = duration.ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
                return;

            string time = StartTimeTextBox.Text;
            string[] parts = time.Split(':');
            int hours = Int32.Parse(parts[0]);
            int minutes = Int32.Parse(parts[1]);
            DateTime dateTime = new DateTime(AppointmentDatePicker.SelectedDate.Value.Year,
                                    AppointmentDatePicker.SelectedDate.Value.Month,
                                    AppointmentDatePicker.SelectedDate.Value.Day,
                                    hours, minutes, 0);

            Period period = new Period(dateTime, Int32.Parse(DurationTextBox.Text), PeriodType.APPOINTMENT, 
                                       (PatientsComboBox.SelectedItem as Patient).Username,
                                       (DoctorsComboBox.SelectedItem as Doctor).Username,
                                       (RoomsComboBox.SelectedItem as Room).Id,
                                       -1);

            int available = IsPeriodAvailable(period);

            if (available == 0)
            {
                Model.Resources.periods.Add(period);
                Model.Resources.SavePeriods();

                MessageBox.Show("Appointment created successfully.", "Success");
                NavigationService.GoBack();
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

        private int IsPeriodAvailable(Period period) // vraca 0 ako je termin ok, 1 ako je soba zauzeta, 2 ako je doktor zauzet, 3 ako je pacijent zauzet
        {
            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);

            foreach (Period existingPeriod in Model.Resources.periods)
            {
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

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = PatientsComboBox.SelectedItem as Patient;

            if (patient != null)
                NavigationService.Navigate(new PatientInfoPage(patient));
        }
    }
}
