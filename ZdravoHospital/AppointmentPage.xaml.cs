using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Specialist> Specialists { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<AppointmentRoom> AppointmentRooms { get; set; }

        public AppointmentPage()
        {
            InitializeComponent();

            this.DataContext = this;
            Doctors = new ObservableCollection<Doctor>(Model.Resources.Doctors.Values);
            Specialists = new ObservableCollection<Specialist>(Model.Resources.Specialists.Values);
            Patients = new ObservableCollection<Patient>(Model.Resources.Patients.Values);
            AppointmentRooms = new ObservableCollection<AppointmentRoom>(Model.Resources.AppointmentRooms.Values);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment();

            if (DoctorsComboBox.SelectedItem != null)
                appointment.Doctor = DoctorsComboBox.SelectedItem as Doctor;
            else
                appointment.Doctor = SpecialistsComboBox.SelectedItem as Doctor;

            appointment.Patient = PatientsComboBox.SelectedItem as Patient;
            string[] parts = StartTimeTextBox.Text.Split(":");
            string hours = parts[0];
            string minutes = parts[1];
            appointment.StartTime = new DateTime(AppointmentDatePicker.SelectedDate.Value.Year, AppointmentDatePicker.SelectedDate.Value.Month,
                AppointmentDatePicker.SelectedDate.Value.Day, Int32.Parse(hours), Int32.Parse(minutes), 0, DateTimeKind.Utc);
            appointment.Duration = Int32.Parse(DurationTextBox.Text);
            appointment.AppointmentRoom = AppointmentRoomComboBox.SelectedItem as AppointmentRoom;


            if (DoctorsComboBox.SelectedItem != null)
                (DoctorsComboBox.SelectedItem as Doctor).AddAppointment(appointment);
            else
                (SpecialistsComboBox.SelectedItem as Specialist).AddAppointment(appointment);

            Model.Resources.Serialize(); // fix

            NavigationService.GoBack();
        }
        private void DoctorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpecialistsComboBox.SelectionChanged -= SpecialistsComboBox_SelectionChanged;
            SpecialistsComboBox.SelectedItem = null;
            SpecialistsComboBox.SelectionChanged += SpecialistsComboBox_SelectionChanged;
        }

        private void SpecialistsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoctorsComboBox.SelectionChanged -= DoctorsComboBox_SelectionChanged;
            DoctorsComboBox.SelectedItem = null;
            DoctorsComboBox.SelectionChanged += DoctorsComboBox_SelectionChanged;
        }
    }
}
