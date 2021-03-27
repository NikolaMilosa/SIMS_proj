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

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    /// 
    
    public partial class PatientWindow : Window
    {

        public List<Appointment> AppointmentList { get; set; }
        public PatientWindow()
        {
            InitializeComponent();
            //public Appointment(DateTime startTime, double duration, Patient patient, Doctor doctor, AppointmentRoom appointmentRoom)
            Patient p = new Patient("22", "nikola", "sad@gmail.com", new DateTime(), "2", "kolja", "dsada", MaritalStatus.MARRIED, Gender.MALE);
            Doctor d = new Doctor("ime", "nikola", "sad@gmail.com", new DateTime(), "2", "kolja", "dsada", MaritalStatus.MARRIED, Gender.MALE);
            //AppointmentRoom ap = new AppointmentRoom();
            //Appointment appointment = new Appointment(new DateTime(), 30, p,d,ap);
            //AppointmentList.Add(appointment);

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Appoinments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
