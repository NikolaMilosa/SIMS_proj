using Model;
using Model.Repository;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for NewAppointmentPage.xaml
    /// </summary>
    public partial class NewAppointmentPage : Page
    {
        private Referral referral;
        public Thickness TopPanelMargin { get; set; }

        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        private DoctorRepository doctorRepository;
        private PatientRepository patientRepository;
        private PeriodRepository periodRepository;
        private RoomRepository roomRepository;
        private PeriodValidation periodValidation;

        public NewAppointmentPage(Doctor doctor, DateTime startTime, int duration)
        {
            InitializeComponent();

            this.DataContext = this;
            doctorRepository = new DoctorRepository();
            patientRepository = new PatientRepository();
            periodRepository = new PeriodRepository();
            roomRepository = new RoomRepository();

            Doctors = new ObservableCollection<Doctor>(doctorRepository.GetValues());
            Patients = new ObservableCollection<Patient>(patientRepository.GetValues());
            Rooms = new ObservableCollection<Room>(roomRepository.GetValues().Where(room => room.RoomType == RoomType.APPOINTMENT_ROOM));

            DoctorsComboBox.SelectedItem = Doctors.ToList().Find(d => d.Username.Equals(doctor.Username));
            AppointmentDatePicker.SelectedDate = startTime.Date;
            StartTimeTextBox.Text = startTime.ToString("HH:mm");
            DurationTextBox.Text = duration.ToString();

            periodValidation = new PeriodValidation();
        }

        public NewAppointmentPage(Referral referral, Patient patient)
        {
            InitializeComponent();

            this.DataContext = this;
            this.referral = referral;
            doctorRepository = new DoctorRepository();
            patientRepository = new PatientRepository();
            periodRepository = new PeriodRepository();
            roomRepository = new RoomRepository();

            Doctors = new ObservableCollection<Doctor>(doctorRepository.GetValues());
            Patients = new ObservableCollection<Patient>(patientRepository.GetValues());
            Rooms = new ObservableCollection<Room>(roomRepository.GetValues().Where(room => room.RoomType == RoomType.APPOINTMENT_ROOM));

            DoctorsComboBox.SelectedItem = Doctors.ToList().Find(d => d.Username.Equals(referral.ReferredDoctorUsername));
            PatientsComboBox.SelectedItem = patient;
            AppointmentDatePicker.SelectedDate = DateTime.Today;
            StartTimeTextBox.Text = "00:00";
            DurationTextBox.Text = "0";

            DoctorsComboBox.IsHitTestVisible = false;
            DoctorsComboBox.IsTabStop = false;
            PatientsComboBox.IsHitTestVisible = false;
            PatientsComboBox.IsTabStop = false;

            periodValidation = new PeriodValidation();
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopDockPanel.Margin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 15);
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
                                       (RoomsComboBox.SelectedItem as Room).Id);
            period.IsUrgent = (bool)IsUrgentCheckBox.IsChecked;

            int available = periodValidation.IsPeriodAvailable(period);

            if (available == 0)
            {
                if (referral != null)
                {
                    period.ReferringReferralId = referral.ReferralId;
                    referral.Period = period;
                    referral.IsUsed = true;
                    Model.Resources.SaveReferrals();
                }

                periodRepository.Create(period);

                MessageBox.Show("Appointment created successfully.", "Success");
                NavigationService.GoBack();
            }
            else if (available == -1)
            {
                MessageBox.Show("Cannot create appointment in the past.", "Invalid date and time");
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

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = PatientsComboBox.SelectedItem as Patient;

            if (patient != null)
                NavigationService.Navigate(new PatientInfoPage(patient));
        }

        private void DoctorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DoctorsComboBox.SelectedIndex == -1)
                return;
        }

        private void DoctorsComboBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
                return;

            string text = DoctorsComboBox.Text;
            DoctorsComboBox.SelectedIndex = -1;
            DoctorsComboBox.Text = text;
            TextBox textBox = DoctorsComboBox.Template.FindName("PART_EditableTextBox", DoctorsComboBox) as TextBox;
            textBox.CaretIndex = text.Length;
            DoctorsComboBox.ItemsSource =
                Model.Resources.doctors.Values.Where(d => d.Name.Contains(DoctorsComboBox.Text, StringComparison.OrdinalIgnoreCase)
                                                    || d.Surname.Contains(DoctorsComboBox.Text, StringComparison.OrdinalIgnoreCase)
                                                    || d.SpecialistType.SpecializationName.Contains(DoctorsComboBox.Text, StringComparison.OrdinalIgnoreCase))
                                                    .ToList();

            if (DoctorsComboBox.Items.Count > 0)
                DoctorsComboBox.IsDropDownOpen = true;
            else
                DoctorsComboBox.IsDropDownOpen = false;
        }

        private void DoctorsComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DoctorsComboBox.IsDropDownOpen = true;
        }

        private void PatientsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatientsComboBox.SelectedIndex == -1)
                return;
        }

        private void PatientsComboBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
                return;

            string text = PatientsComboBox.Text;
            PatientsComboBox.SelectedIndex = -1;
            PatientsComboBox.Text = text;
            TextBox textBox = PatientsComboBox.Template.FindName("PART_EditableTextBox", PatientsComboBox) as TextBox;
            textBox.CaretIndex = text.Length;
            PatientsComboBox.ItemsSource =
                Model.Resources.patients.Values.Where(p => p.Name.Contains(PatientsComboBox.Text, StringComparison.OrdinalIgnoreCase)
                                                        || p.Surname.Contains(PatientsComboBox.Text, StringComparison.OrdinalIgnoreCase))
                                                        .ToList();

            if (PatientsComboBox.Items.Count > 0)
                PatientsComboBox.IsDropDownOpen = true;
            else
                PatientsComboBox.IsDropDownOpen = false;
        }

        private void PatientsComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PatientsComboBox.IsDropDownOpen = true;
        }

        private void RoomsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsComboBox.SelectedIndex == -1)
                return;
        }

        private void RoomsComboBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
                return;

            string text = RoomsComboBox.Text;
            RoomsComboBox.SelectedIndex = -1;
            RoomsComboBox.Text = text;
            TextBox textBox = RoomsComboBox.Template.FindName("PART_EditableTextBox", RoomsComboBox) as TextBox;
            textBox.CaretIndex = text.Length;
            RoomsComboBox.ItemsSource =
                Model.Resources.rooms.Values.Where(r => r.Id.ToString().Contains(RoomsComboBox.Text, StringComparison.OrdinalIgnoreCase)
                                                     && r.RoomType == RoomType.APPOINTMENT_ROOM).ToList();

            if (RoomsComboBox.Items.Count > 0)
                RoomsComboBox.IsDropDownOpen = true;
            else
                RoomsComboBox.IsDropDownOpen = false;
        }

        private void RoomsComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            RoomsComboBox.IsDropDownOpen = true;
        }
    }
}
