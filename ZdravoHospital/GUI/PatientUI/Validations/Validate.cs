using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModel;
using System.Threading;

namespace ZdravoHospital.GUI.PatientUI.Validations
{
    public static class Validate
    {

        public static void ShowOkDialog(string title, string content)
        {
            customOkDialog customOkDialog = new customOkDialog(title, content);
            customOkDialog.ShowDialog();
        }
        public static bool TrollDetected()
        {
            bool detected = false;
            if (PatientWindow.RecentActionsNum >= 5)
            {
                detected = true;
                ShowOkDialog("Troll detected", "Too much recent actions have been detected! Please wait couple of minutes then try again!");
            }
            return detected;
        }
        public static bool IsSurveyAvailable(string username)
        {
            bool availability = false;
            Resources.OpenPeriods();
            int numOfPeriods = GetCompletedPeriodsNum(username);
            if (numOfPeriods >= 3 && !AnyRecentSurveys(username))
                availability = true;
            return availability;
        }

        public static int GetCompletedPeriodsNum(string username)
        {
            int periodNum = 0;
            foreach (Period period in Model.Resources.periods)
            {
                if (period.PatientUsername.Equals(username) && period.HasPassed())
                {
                    periodNum++;
                }
            }
            return periodNum;
        }

        public static bool AnyRecentSurveys(string username)
        {
            bool recentSurvey = false;
            Model.Resources.OpenSurveys();
            foreach (Survey survey in Resources.surveys)
            {
                if (survey.PatientUsername.Equals(username) && survey.IsWithin2WeeksFromNow())
                {
                    recentSurvey = true;
                    break;
                }
            }
            return recentSurvey;
        }

        public static void TherapyNotification(object patientUsername)
        {
            string username = (string)patientUsername;
            Resources.OpenPeriods();

            while (true)
            {

                foreach (Period period in Resources.periods)
                {
                    if (period.PatientUsername.Equals(username) && period.Prescription != null)
                    {
                        GeneratePrescriptionTimes(period.Prescription);
                    }
                }
                Sleep(5);
            }
        }

        public static void ResetActionsNum(object patient)
        {
            Patient user = (Patient)patient;
            while (true)
            {
                if (user.LastLogoutTime.AddMinutes(5) <= DateTime.Now)
                    PatientWindow.RecentActionsNum = 0;
                else
                    PatientWindow.RecentActionsNum = user.RecentActions;

                Sleep(5);
            }
        }

        public static void Sleep(int minutes)
        {
            Thread.Sleep(TimeSpan.FromMinutes(minutes));
        }


        public static void GeneratePrescriptionTimes(Prescription prescription)
        {
            foreach (Therapy therapy in prescription.TherapyList)
                GenerateTimes(therapy);

        }


        public static List<DateTime> GenerateTimes(Therapy therapy)
        {
            List<DateTime> notifications = GenerateNotificationsForEachDay(therapy);

            foreach (DateTime dateTime in notifications)
                if (IsWithin5Minutes(dateTime))
                    ShowOkDialog("Therapy", "You have prescripted " + therapy.Medicine.MedicineName + " at " + dateTime.ToString("HH:mm"));

            return notifications;
        }

        public static bool IsWithin5Minutes(DateTime dateTime)
        {
            bool itIs = false;
            if (dateTime >= DateTime.Now && dateTime <= DateTime.Now.AddMinutes(5))
                itIs = true;

            return itIs;
        }

        public static List<DateTime> GenerateNotificationsForEachDay(Therapy therapy)
        {
            List<DateTime> notifications = new List<DateTime>();
            DateTime dateIterator = therapy.StartHours;
            while (dateIterator.Date < therapy.EndDate.Date)
            {
                for (int i = 0; i < therapy.TimesPerDay; ++i)
                    notifications.Add(dateIterator.AddHours(i * 24 / therapy.TimesPerDay));

                dateIterator = dateIterator.AddDays(therapy.PauseInDays + 1);
            }
            return notifications;
        }


        public static void SuggestDoctor(Period checkedPeriod, ObservableCollection<DoctorView> doctorList)
        {
            foreach (DoctorView doctor in doctorList.ToList())
            {
                RemoveUnavailableDoctorsFromCollection(doctor, checkedPeriod, doctorList);
            }
        }

        public static void RemoveUnavailableDoctorsFromCollection(DoctorView doctor, Period checkedPeriod, ObservableCollection<DoctorView> doctorList)
        {
            foreach (Period period in Model.Resources.periods)
            {
                if (period.DoctorUsername.Equals(doctor.Username) && Validate.DoPeriodsOverlap(period, checkedPeriod))
                {
                    doctorList.Remove(doctor);
                    break;
                }
            }
        }


        public static void GenerateTimeSpan(List<TimeSpan> timeList)
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

        public static void GenerateObesrvableTimes(ObservableCollection<TimeSpan> timeList)
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


        public static int GetFreeRoom(Period checkedPeriod)//vraca prvi slobodan Appointment room za zadati termin
        {
            int roomId = -1;
            Model.Resources.OpenRooms();
            OpenPeriods();

            foreach (Room room in Model.Resources.rooms.Values)
                if (GetFreeRoomId(room, checkedPeriod) != -1)
                    return room.Id;

            ShowOkDialog("Warning", "There is no free rooms at selected time!");
            return roomId;
        }

        public static int GetFreeRoomId(Room room, Period checkedPeriod)
        {
            int roomId = -1;

            if (room.IsAppointmentRoom() && room.Available)//IZMENA OVOG TREBA KAD SE URADI ROOM SCHEDULER
                if (!PeriodAlreadyExistsInRoom(room, checkedPeriod))
                    return room.Id;

            return roomId;
        }

        public static bool PeriodAlreadyExistsInRoom(Room room, Period checkedPeriod)
        {
            bool exists = false;

            foreach (Period period in Model.Resources.periods)
                if (period.RoomId == room.Id && DoPeriodsOverlap(period, checkedPeriod))
                    return true;

            return exists;
        }

        public static void OpenPeriods()
        {
            if (Resources.periods == null)
                Model.Resources.OpenPeriods();
        }

        public static bool DoPeriodsOverlap(Period period, Period checkedPeriod)
        {
            if (period.Equals(checkedPeriod))//u slucaju kad edituje period
                return false;

            DateTime endingPeriodTime = period.StartTime.AddMinutes(period.Duration);
            DateTime endingCheckedPeriodTime = checkedPeriod.StartTime.AddMinutes(checkedPeriod.Duration);

            if ((checkedPeriod.StartTime >= period.StartTime && checkedPeriod.StartTime < endingPeriodTime) || (endingCheckedPeriodTime > period.StartTime && endingCheckedPeriodTime <= endingPeriodTime))
                return true;

            return false;
        }


        public static bool CheckPeriodAvailability(Period checkedPeriod, bool writeWarnings)
        {
            bool available = true;
            OpenPeriods();
            foreach (Period period in Model.Resources.periods)
                if (!IsPeriodAvailable(period, checkedPeriod, writeWarnings))
                    return false;

            return available;
        }

        public static bool IsPeriodAvailable(Period period,Period checkedPeriod,bool writeWarnings)
        {
            bool available = true;
            if (period.StartTime.Date == checkedPeriod.StartTime.Date)
            {
                if (period.PatientUsername.Equals(checkedPeriod.PatientUsername) && !IsPatientAvailable(period, checkedPeriod, writeWarnings)) //proveri da li pacijent tad ima zakazano
                    available = false;
                else if (period.DoctorUsername.Equals(checkedPeriod.DoctorUsername) && !IsDoctorAvailable(period, checkedPeriod, writeWarnings))//proveri da li doktor tad ima zakazano
                    available = false;
            }
            return available;
        }


        public static bool IsDoctorAvailable(Period period, Period checkedPeriod, bool writeWarnings)
        {
            bool available = true;
            if (DoPeriodsOverlap(period, checkedPeriod))
            {
                if (writeWarnings)
                    ShowOkDialog("Warning", "Doctor has an existing appointment at selected time!");

                available = false;
            }

            return available;
        }

        public static bool IsPatientAvailable(Period period, Period checkedPeriod, bool writeWarnings)
        {
            bool available = true;
            if (DoPeriodsOverlap(period, checkedPeriod))
            {
                if (writeWarnings)
                    ShowOkDialog("Warning", "Patient has an existing appointment at selected time!");

                available = false;
            }

            return available;
        }

    }
}
