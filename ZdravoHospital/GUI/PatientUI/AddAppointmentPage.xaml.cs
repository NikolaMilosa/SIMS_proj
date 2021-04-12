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
    /// Interaction logic for AddAppointmentPage.xaml
    /// </summary>
    public partial class AddAppointmentPage : Page
    {

        public ObservableCollection<DoctorView> DoctorList { get; set; }
        public ObservableCollection<TimeSpan> PeriodList { get; set; }
        public Period Period { get; set; }

       

        public AddAppointmentPage(string username)
        {
            InitializeComponent();
            DataContext = this;
            generateTimeSpan();
            fillList();
            //DoctorList = new ObservableCollection<DoctorView>(Model.Resources.doctors.Values);
            Period = new Period();
            Period.PatientUsername = username;
        }

        public void generateTimeSpan()
        {
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

        public void fillList() 
        {
            Model.Resources.DeserializeDoctors();
            DoctorList = new ObservableCollection<DoctorView>();
            foreach (Doctor doctor in Model.Resources.doctors.Values) 
            {
                DoctorList.Add(new DoctorView(doctor));
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectTime.SelectedItem == null || selectDate.SelectedDate == null || selectDoctor.SelectedItem == null)
            {
                MessageBox.Show("Please select doctor,date and time when you want to schedule appointment.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //ispraviti!!!!!!
                Period.StartTime=Period.StartTime.Date+ (TimeSpan)selectTime.SelectedItem; 
                Period.DoctorUsername=((DoctorView)selectDoctor.SelectedItem).Username;
                Period.Duration = 30;
                Period.RoomId = 102;
                MessageBox.Show("Date time: "+Period.StartTime.ToString() + "Doctor Username: " + Period.DoctorUsername);
                Model.Resources.OpenPeriods();
                Model.Resources.periods.Add(Period);
                Model.Resources.SavePeriods();
            }
        }
    }
}
