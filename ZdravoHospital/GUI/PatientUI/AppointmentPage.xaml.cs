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
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for AppointmentPage.xaml
    /// </summary>
    /// 
   
    public partial class AppointmentPage : Page
    {
        public ObservableCollection<AppointmentView> AppointmentList { get; set; }
    

        public AppointmentPage()
        {
            InitializeComponent();
            AppointmentList = new ObservableCollection<AppointmentView>();
            AppointmentView appointmentView = new AppointmentView();
            Doctor doctor = new Doctor();
            doctor.Name = "Stefan";
            doctor.Surname = "Stefanovic";

            appointmentView.Doctor = doctor;
            appointmentView.Duration = 30;
            appointmentView.RoomId = 201;
            appointmentView.PeriodType = PeriodType.APPOINTMENT;
            appointmentView.StartTime = new DateTime();
            //
            AppointmentView appointmentView1 = new AppointmentView();
            Doctor doctor1 = new Doctor();
            doctor1.Name = "Jozef";
            doctor1.Surname = "Jozic";

            appointmentView1.Doctor = doctor1;
            appointmentView1.Duration = 30;
            appointmentView1.RoomId = 102;
            appointmentView1.PeriodType = PeriodType.OPERATION;
            appointmentView1.StartTime = new DateTime();
            //
            AppointmentList.Add(appointmentView);
            AppointmentList.Add(appointmentView1);
            DataContext = this;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;
            AppointmentList.Remove(appointmentView);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;
            MessageBox.Show("Appointment you pressed has doctors name: " + appointmentView.Doctor.Name);
        }
    }
}
