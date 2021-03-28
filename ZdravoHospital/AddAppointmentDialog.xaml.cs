using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.ObjectModel;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for AddAppointmentDialog.xaml
    /// </summary>
    public partial class AddAppointmentDialog : Window
    {
        public ObservableCollection<Doctor> DoctorList { get; set; }
        public ObservableCollection<TimeSpan> PeriodList { get; set; }
        public PatientWindow PatientWindow { get; set; }

        public Appointment Appointment { get; set; }

        public Resources Resources { get; set; }

        public AddAppointmentDialog(PatientWindow patientWindow)
        {
            InitializeComponent();
            PatientWindow = patientWindow;
            Appointment = new Appointment();
            Resources = patientWindow.Resources;


            var values = Model.Resources.Doctors.Values.ToList();
            DoctorList = new ObservableCollection<Doctor>(values);
            //DoctorList = new ObservableCollection<Doctor>();
            generateTimeSpan();
            //Doctor d = new Doctor("Doca", "Aleksijevic", "sad@gmail.com", new DateTime(), "2", "kolja", "dsada", MaritalStatus.MARRIED, Gender.MALE);
           // DoctorList.Add(d);
            DataContext = this;
        }

        public void dictionaryToList() { 

        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectTime.SelectedItem == null || selectDate.SelectedDate == null || selectDoctor.SelectedItem == null)
            {
                MessageBox.Show("Please select doctor,date and time when you want to schedule appointment.","Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                Appointment.P = PatientWindow.Patient;
                Appointment.Patient= PatientWindow.Patient;
                Appointment.DateTime = Appointment.DateTime.Date + (TimeSpan)selectTime.SelectedItem;
         
                Appointment.Duration = 30;
                Appointment.Ap = getFreeAppointmentRoom();
                Appointment.AppointmentRoom= getFreeAppointmentRoom();
                Appointment.Doctor = (Doctor)selectDoctor.SelectedItem;
                PatientWindow.AppointmentList.Add(Appointment);
                PatientWindow.Patient.Appointment.Add(Appointment);
                //
               //  Resources.Patients[PatientWindow.Patient.Username].Appointment.Add(Appointment);
                
               // Resources.Appointments.Add(Appointment);
                Resources.AppointmentRooms[Appointment.Ap.Id].Appointment.Add(Appointment);
                Resources.Doctors[Appointment.D.Username].Appointment.Add(Appointment);
                MessageBox.Show("Succesfully added Appointment!");
                Close();
            }
        }

        public AppointmentRoom getFreeAppointmentRoom() {
            AppointmentRoom ap = null;
            foreach (var appointmentRoom in Resources.AppointmentRooms) {
                if (appointmentRoom.Value.Avaliabe) {
                    ap=appointmentRoom.Value;
                }
            }
            return ap;
        }

        public void generateTimeSpan() {
            PeriodList = new ObservableCollection<TimeSpan>();
            PeriodList.Add(new TimeSpan(8, 0, 0));
            PeriodList.Add(new TimeSpan(8, 30, 0));
            PeriodList.Add(new TimeSpan(9, 0, 0));
            PeriodList.Add(new TimeSpan(9, 0, 0));
            PeriodList.Add(new TimeSpan(10, 0, 0));
            PeriodList.Add(new TimeSpan(10, 30, 0));
            PeriodList.Add(new TimeSpan(11, 0, 0));
            PeriodList.Add(new TimeSpan(11, 30, 0));
            PeriodList.Add(new TimeSpan(12, 0, 0));
            PeriodList.Add(new TimeSpan(12, 30, 0));
            PeriodList.Add(new TimeSpan(13, 0, 0));
            PeriodList.Add(new TimeSpan(13, 30, 0));
            PeriodList.Add(new TimeSpan(14, 0, 0));
            PeriodList.Add(new TimeSpan(14, 30, 0));
            PeriodList.Add(new TimeSpan(15, 0, 0));
            PeriodList.Add(new TimeSpan(15, 30, 0));
        }

    }
}
