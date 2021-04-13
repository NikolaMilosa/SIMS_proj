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

       bool Mode { get; set; }//true=add,false=edit

        public AddAppointmentPage(Period period,bool mode,string username)
        {
            
            InitializeComponent();
            DataContext = this;
            Mode = mode;
            generateTimeSpan();
            fillList();
            if(mode)
            {
                Period = new Period();
                Period.PatientUsername =username;
                Period.Duration = 30;
                //selectDate.DisplayDateStart = DateTime.Today;
            }
           else
            {
                Period = period;
                selectDate.SelectedDate = Period.StartTime;
                if(Period.StartTime.AddDays(-3) > DateTime.Today)
                    selectDate.DisplayDateStart = Period.StartTime.AddDays(-3);
                selectDate.DisplayDateEnd = Period.StartTime.AddDays(3);
                selectDoctor.SelectedItem = getDoctor(Period.DoctorUsername);
                selectTime.SelectedItem = Period.StartTime.TimeOfDay;
            }
           
            
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
                Period.RoomId = getFreeRoom();
                if (checkPeriod() && Period.RoomId!=-1)
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

        private int getFreeRoom()
        {
            int roomId = -1;
            Model.Resources.OpenRooms();
            if(Mode)
                Model.Resources.OpenPeriods();
            bool exists = true;
            foreach (Room room in Model.Resources.rooms.Values)
            {
                if(room.RoomType==RoomType.APPOINTMENT_ROOM && room.Available)
                {
                    exists = false;
                    foreach (Period period in Model.Resources.periods)
                    {
                        if(period.RoomId==room.Id)
                        {
                            if (doPeriodsOverlap(period))
                            {
                                exists = true;
                                break;
                            }
                        }
                    }
                    if (!exists)
                        return room.Id;
                }
            }
            MessageBox.Show("Theres no rooms available at selected time!");
            return roomId;
        }

        private bool checkPeriod() 
        {
            bool doesntExist = true;

            foreach (Period period in Model.Resources.periods)
            {    
                if (period.StartTime.Date == Period.StartTime.Date)
                {
                    if (period.PatientUsername.Equals(Period.PatientUsername)) //proveri da li pacijent tad ima zakazano
                    {
                        if (doPeriodsOverlap(period))
                        {
                            MessageBox.Show("Patient has an existing appointment at selected time!");
                            doesntExist = false;
                            break;
                        }
                    }
                    else if (period.DoctorUsername.Equals(Period.DoctorUsername))//proveri da li doktor tad ima zakazano
                    {
                        if(doPeriodsOverlap(period))
                        {
                            MessageBox.Show("Doctor has an existing appointment at selected time!");
                            doesntExist = false;
                            break;
                        }
                    }
                } 
            }
            return doesntExist;
        }

        private bool doPeriodsOverlap(Period period)
        {
            DateTime endingtDateTime = period.StartTime.AddMinutes(period.Duration);
            DateTime endingDateTimePeriod = Period.StartTime.AddMinutes(30);
            if (period.Equals(Period))//u slucaju kad edituje period
                return false;

            if ((Period.StartTime >= period.StartTime && Period.StartTime < endingtDateTime) || (endingDateTimePeriod > period.StartTime && endingDateTimePeriod <= endingtDateTime))
                return true;

            return false;
        }
    }
}
