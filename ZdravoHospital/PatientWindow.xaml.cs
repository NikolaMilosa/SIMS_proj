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
using System.Data;
using Model;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    /// 
    
    public partial class PatientWindow : Window
    {

        public ObservableCollection<Appointment> AppointmentList { get; set; }
        public Patient Patient { get; set; }

        public  Dictionary<string, Patient> Patients { get; set; }
        public string HelloString { get; set; }

        public Resources Resources { get; set; }

     

        public PatientWindow(string username,Resources res)
        {
            InitializeComponent();
            Resources = res;
            HelloString = "Hello, " + username;
            Patients= Resources.Patients;
            //Patient = Patients[username];
            //AppointmentList = new ObservableCollection<Appointment>(Patient.Appointment);
           
            Patient = new Patient("22", "nikola", "sad@gmail.com", new DateTime(), "2", "kolja", "dsada", MaritalStatus.MARRIED, Gender.MALE);
            Doctor d = new Doctor("ime", "nikola", "sad@gmail.com", new DateTime(), "2", "kolja", "dsada", MaritalStatus.MARRIED, Gender.MALE);
<<<<<<< HEAD
            //AppointmentRoom ap = new AppointmentRoom();
            //Appointment appointment = new Appointment(new DateTime(), 30, p,d,ap);
            //AppointmentList.Add(appointment);
=======
            AppointmentRoom ap = new AppointmentRoom(RoomType.APPOINTMENT_ROOM,22,"testSoba",true);
>>>>>>> feature-PatientAppointmentCRUD

            Appointment appointment = new Appointment(new DateTime(30), 30, Patient,d,ap);
            //AppointmentList = new ObservableCollection<Appointment>();
            Patient.Appointment.Add(appointment);
            AppointmentList = new ObservableCollection<Appointment>(Patient.Appointment);
            //AppointmentList.Add(appointment);
           
            DataContext = this;
        }

       

        private void addAppointment_Click(object sender, RoutedEventArgs e)
        {
            AddAppointmentDialog dialog = new AddAppointmentDialog(this);
            MessageBox.Show(Patient.Appointment.Count.ToString());
            dialog.Show();
        }

        private void cancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment= (Appointment)myDataGrid.SelectedItem;
            Patient.Appointment.Remove(appointment);
            AppointmentList.Remove(appointment);
            /*
            Resources.Patients[Patient.Username].Appointment = Patient.Appointment; //pacijentu ukloni pregled
            Resources.Appointments.Remove(appointment);//pregledima ukloni pregled
            Resources.AppointmentRooms[appointment.Ap.Id].Appointment.Remove(appointment);//iz liste pregleda u appointment room-u ukloni pregled
            Resources.Doctors[appointment.D.Username].Appointment.Remove(appointment);//doktoru iz liste pregleda ukloni pregled
            */

        }
    }
}
