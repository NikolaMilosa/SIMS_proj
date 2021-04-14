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
            generateTimeSpan();
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

        public void generateTimeSpan()
        {
            PeriodList = new ObservableCollection<TimeSpan>();
            TimeList = new List<TimeSpan>();
            TimeList.Add(new TimeSpan(8, 0, 0));
            TimeList.Add(new TimeSpan(8, 30, 0));
            TimeList.Add(new TimeSpan(9, 0, 0));
            TimeList.Add(new TimeSpan(9, 30, 0));
            TimeList.Add(new TimeSpan(10, 0, 0));
            TimeList.Add(new TimeSpan(10, 30, 0));
            TimeList.Add(new TimeSpan(11, 0, 0));
            TimeList.Add(new TimeSpan(11, 30, 0));
            TimeList.Add(new TimeSpan(12, 0, 0));
            TimeList.Add(new TimeSpan(12, 30, 0));
            TimeList.Add(new TimeSpan(13, 0, 0));
            TimeList.Add(new TimeSpan(13, 30, 0));
            TimeList.Add(new TimeSpan(14, 0, 0));
            TimeList.Add(new TimeSpan(14, 30, 0));
            TimeList.Add(new TimeSpan(15, 0, 0));
            TimeList.Add(new TimeSpan(15, 30, 0));
            //
            PeriodList.Add(new TimeSpan(8, 0, 0));
            PeriodList.Add(new TimeSpan(8, 30, 0));
            PeriodList.Add(new TimeSpan(9, 0, 0));
            PeriodList.Add(new TimeSpan(9, 30, 0));
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
                if (checkPeriod(Period) && Period.RoomId!=-1)
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
                            if (doPeriodsOverlap(period,Period))
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

        private bool checkPeriod(Period checkedPeriod) 
        {
            bool doesntExist = true;

            foreach (Period period in Model.Resources.periods)
            {    
                if (period.StartTime.Date == Period.StartTime.Date)
                {
                    if (period.PatientUsername.Equals(Period.PatientUsername)) //proveri da li pacijent tad ima zakazano
                    {
                        if (doPeriodsOverlap(period,checkedPeriod))
                        {
                            MessageBox.Show("Patient has an existing appointment at selected time!");
                            doesntExist = false;
                            break;
                        }
                    }
                    else if (period.DoctorUsername.Equals(Period.DoctorUsername))//proveri da li doktor tad ima zakazano
                    {
                        if(doPeriodsOverlap(period,checkedPeriod))
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

        private bool doPeriodsOverlap(Period period,Period checkedPeriod)
        {
            DateTime endingtDateTime = period.StartTime.AddMinutes(period.Duration);
            DateTime endingDateTimePeriod = checkedPeriod.StartTime.AddMinutes(30);
            if (period.Equals(checkedPeriod))//u slucaju kad edituje period
                return false;

            if ((checkedPeriod.StartTime >= period.StartTime && checkedPeriod.StartTime < endingtDateTime) || (endingDateTimePeriod > period.StartTime && endingDateTimePeriod <= endingtDateTime))
                return true;

            return false;
        }

        private void suggestButton_Click(object sender, RoutedEventArgs e)
        {
            Period period = new Period();
            period.PatientUsername = Period.PatientUsername;
            if (selectDoctor.SelectedItem != null && selectDate.SelectedDate == null && selectTime.SelectedItem == null) 
            {
                period.DoctorUsername=((DoctorView)selectDoctor.SelectedItem).Username;
                int dayNums = 3;
                PeriodList.Clear();
                while (PeriodList.Count < 2)
                {
                    selectDate.SelectedDate = DateTime.Today.AddDays(dayNums);
                    PeriodList.Clear();
                    foreach (TimeSpan timeSpan in TimeList)
                    {
                        if (PeriodList.Count == 4)
                        {
                            break;
                        }
                        period.StartTime = DateTime.Today.AddDays(dayNums);
                        period.StartTime += timeSpan;
                        if (checkPeriod(period))
                        {
                            MessageBox.Show(period.StartTime.ToString());
                            PeriodList.Add(timeSpan);

                        }
                    }
                    ++dayNums;
                    
                }
                    MessageBox.Show("Time list is updated to suggested times!");
            }
            else if(selectDoctor.SelectedItem == null && selectDate.SelectedDate != null && selectTime.SelectedItem != null)
            {

                period.StartTime = (DateTime)selectDate.SelectedDate;
                period.StartTime += (TimeSpan)selectTime.SelectedItem;
                //
                foreach (DoctorView doctor in DoctorList.ToList())
                {
                    foreach (Period period1 in Model.Resources.periods)
                    {
                        if (period1.DoctorUsername.Equals(doctor.Username))
                             if (doPeriodsOverlap(period1, period))
                                    {
                                        DoctorList.Remove(doctor);
                                        break;
                                    }
                    }
                    
                }
               
                if(DoctorList.Count>0)
                    MessageBox.Show("Doctor list is updated to suggested doctors!");
                //
            }
            else
            {
                MessageBox.Show("Please choose doctor or time so the system could suggest you  periods!");
            }
        }
    }
}
