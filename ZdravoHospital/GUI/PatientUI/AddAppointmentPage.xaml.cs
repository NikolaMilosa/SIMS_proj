using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoHospital.GUI.PatientUI.Validations;
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

        public List<TimeSpan> TimeList { get; set; }

       bool Mode { get; set; }//true=add,false=edit

        public AddAppointmentPage(Period period,bool mode,string username)
        {
            
            InitializeComponent();
            DataContext = this;
            Mode = mode;
            PeriodList = new ObservableCollection<TimeSpan>();
            Validate.generateObesrvableTimes(PeriodList);
            fillList();
            if(mode)
            {
                Period = new Period();
                Period.PatientUsername =username;
                Period.Duration = 30;
            }
           else
            {
                Period = period;
                selectDate.SelectedDate = Period.StartTime;
                selectDoctor.SelectedItem = getDoctor(Period.DoctorUsername);
                selectTime.SelectedItem = Period.StartTime.TimeOfDay;
            }
           selectDate.DisplayDateStart = DateTime.Today.AddDays(3);
        }

        public DoctorView getDoctor(string username)
        {
           
            
            foreach(DoctorView doctor in DoctorList)
            {
                if (doctor.Username.Equals(username))
                    return doctor;
            }

            return null;
        }

        public void fillList() 
        {
            Model.Resources.DeserializeDoctors();

            if (DoctorList == null)
                DoctorList = new ObservableCollection<DoctorView>();
            else
                DoctorList.Clear();

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
                Period.StartTime=Period.StartTime.Date+ (TimeSpan)selectTime.SelectedItem; 
                Period.DoctorUsername=((DoctorView)selectDoctor.SelectedItem).Username;
                Period.RoomId = Validate.getFreeRoom(Period);
                if (Validate.checkPeriod(Period,true) && Period.RoomId!=-1)
                {
                    if (Mode)
                    {
                        Model.Resources.OpenPeriods();
                        Model.Resources.periods.Add(Period);
                        
                    }

                    Model.Resources.SavePeriods();
                }
            }
        }

       

        private void suggestButton_Click(object sender, RoutedEventArgs e)
        {
            Period period = new Period();
            period.PatientUsername = Period.PatientUsername;

            if (selectDoctor.SelectedItem != null && selectDate.SelectedDate == null && selectTime.SelectedItem == null) //suggest time based on doctor
            {
                period.DoctorUsername = ((DoctorView)selectDoctor.SelectedItem).Username;
                Validate.suggestTime(period, PeriodList);
                selectDate.SelectedDate = period.StartTime;
                selectTime.Focus();
                selectTime.IsDropDownOpen = true;
            }
            else if(selectDoctor.SelectedItem == null && selectDate.SelectedDate != null && selectTime.SelectedItem != null)//suggest doctor based on time
            {

                period.StartTime = (DateTime)selectDate.SelectedDate;
                period.StartTime += (TimeSpan)selectTime.SelectedItem;
                if (!Validate.checkPeriod(period, true))
                    return;

                fillList();
                Validate.suggestDoctor(period, DoctorList);
                if (DoctorList.Count <= 0)
                    MessageBox.Show("There is no available doctor at the selected time!");
                else
                {
                    MessageBox.Show("Doctor list is updated to suggested doctors!");
                    selectDoctor.Focus();
                    selectDoctor.IsDropDownOpen = true;
                }
            }
            else
            {
                MessageBox.Show("Please choose doctor or time so the system could suggest you  periods!");
            }
        }
    }
}
