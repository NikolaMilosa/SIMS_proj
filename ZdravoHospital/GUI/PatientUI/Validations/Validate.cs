using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI.Validations
{
    public static class Validate
    {

        public static void suggestDoctor(Period checkedPeriod, ObservableCollection<DoctorView> doctorList)
        { 
            foreach (DoctorView doctor in doctorList.ToList())
            {
                foreach (Period period in Model.Resources.periods)
                {
                    if (period.DoctorUsername.Equals(doctor.Username))
                        if (Validate.doPeriodsOverlap(period, checkedPeriod))
                        {
                            doctorList.Remove(doctor);
                            break;
                        }
                }

            }
        }

        public static void suggestTime(Period period,ObservableCollection<TimeSpan> periodList)
        {
            int dayNums = 3;
            List<TimeSpan> timeList = new List<TimeSpan>();
            generateTimeSpan(timeList);
            periodList.Clear();
            while (periodList.Count < 2)
            {
                periodList.Clear();
                foreach (TimeSpan timeSpan in timeList)
                {
                    if (periodList.Count == 4)
                    {
                        break;
                    }
                    period.StartTime = DateTime.Today.AddDays(dayNums);
                    period.StartTime += timeSpan;
                    if (Validate.checkPeriod(period, false))
                    {
                        periodList.Add(timeSpan);
                    }
                }
                ++dayNums;
            }
            MessageBox.Show("Time list is updated to suggested times!");
        }

        public static void generateTimeSpan(List<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
        }

        public static void generateObesrvableTimes(ObservableCollection<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
        }


        public static int getFreeRoom(Period checkedPeriod)//vraca prvi slobodan Appointment room za zadati termin
        {
            int roomId = -1;
            Model.Resources.OpenRooms();
            if(Resources.periods==null)
              Model.Resources.OpenPeriods();

            bool exists = true;
            foreach (Room room in Model.Resources.rooms.Values)
            {
                if (room.RoomType == RoomType.APPOINTMENT_ROOM && room.Available)
                {
                    exists = false;
                    foreach (Period period in Model.Resources.periods)
                    {
                        if (period.RoomId == room.Id)
                        {
                            if (doPeriodsOverlap(period, checkedPeriod))
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

        internal static void generateTimeSpan(ObservableCollection<TimeSpan> periodList)
        {
            throw new NotImplementedException();
        }

        public static bool doPeriodsOverlap(Period period, Period checkedPeriod)
        {
            DateTime endingtDateTime = period.StartTime.AddMinutes(period.Duration);
            DateTime endingDateTimePeriod = checkedPeriod.StartTime.AddMinutes(30);
            if (period.Equals(checkedPeriod))//u slucaju kad edituje period
                return false;

            if ((checkedPeriod.StartTime >= period.StartTime && checkedPeriod.StartTime < endingtDateTime) || (endingDateTimePeriod > period.StartTime && endingDateTimePeriod <= endingtDateTime))
                return true;

            return false;
        }


        public static bool checkPeriod(Period checkedPeriod,bool writeWarnings)
        {
            bool doesntExist = true;
            if (Resources.periods == null)
                Model.Resources.OpenPeriods();
            foreach (Period period in Model.Resources.periods)
            {
                if (period.StartTime.Date == checkedPeriod.StartTime.Date)
                {
                    if (period.PatientUsername.Equals(checkedPeriod.PatientUsername)) //proveri da li pacijent tad ima zakazano
                    {
                        if (doPeriodsOverlap(period, checkedPeriod))
                        {
                            if(writeWarnings)
                                MessageBox.Show("Patient has an existing appointment at selected time!");
                            doesntExist = false;
                            break;
                        }
                    }
                    else if (period.DoctorUsername.Equals(checkedPeriod.DoctorUsername))//proveri da li doktor tad ima zakazano
                    {
                        if (doPeriodsOverlap(period, checkedPeriod))
                        {
                           if(writeWarnings)
                                MessageBox.Show("Doctor has an existing appointment at selected time!");
                           doesntExist = false;
                           break;
                        }
                    }
                }
            }
            return doesntExist;
        }
    }
}
