using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class PeriodFunctions
    {
        #region Properties

        private  PeriodRepository _periodRepository;
        public PeriodRepository PeriodRepository
        {
            get
            {
                if (_periodRepository == null)
                    _periodRepository = new PeriodRepository();

                return _periodRepository;
            }
            set { _periodRepository = value; }
        }

        public string PatientUsername { get; set; }
        #endregion

        public PeriodFunctions(string username)
        {
            PeriodRepository = new PeriodRepository();
            PatientUsername = username;
        }

        #region Methods
        //doctor related functions
        public  void SuggestDoctor(Period checkedPeriod, ObservableCollection<DoctorView> doctorList)
        {
            foreach (DoctorView doctor in doctorList.ToList())
            {
                RemoveUnavailableDoctorFromCollection(doctor, checkedPeriod, doctorList);
            }
        }

        public  void RemoveUnavailableDoctorFromCollection(DoctorView doctor, Period checkedPeriod, ObservableCollection<DoctorView> doctorList)
        {
            List<Period> periods = PeriodRepository.GetValues();
            foreach (Period period in periods)
            {
                if (period.DoctorUsername.Equals(doctor.Username) && DoPeriodsOverlap(period, checkedPeriod))
                {
                    doctorList.Remove(doctor);
                    break;
                }
            }
        }


        //Room related functions
        public  int GetFreeRoom(Period checkedPeriod)//vraca prvi slobodan Appointment room za zadati termin
        {
            int roomId = -1;
            RoomRepository roomRepository = new RoomRepository();
            List<Room> rooms = roomRepository.GetValues();
            foreach (Room room in rooms)
                if (GetFreeRoomId(room, checkedPeriod) != -1)
                    return room.Id;

            Validate.ShowOkDialog("Warning", "There is no free rooms at selected time!");
            return roomId;
        }

        public  int GetFreeRoomId(Room room, Period checkedPeriod)
        {
            int roomId = -1;

            if (room.IsAppointmentRoom() && room.Available)//IZMENA OVOG TREBA KAD SE URADI ROOM SCHEDULER
                if (!PeriodAlreadyExistsInRoom(room, checkedPeriod))
                    return room.Id;

            return roomId;
        }

        public  bool PeriodAlreadyExistsInRoom(Room room, Period checkedPeriod)
        {
            bool exists = false;
            List<Period> periods = PeriodRepository.GetValues();
            foreach (Period period in periods)
                if (period.RoomId == room.Id && DoPeriodsOverlap(period, checkedPeriod))
                    return true;

            return exists;
        }
        //

        public bool CheckPeriodAvailability(Period checkedPeriod, bool writeWarnings)
        {
            bool available = true;
            List<Period> periods = PeriodRepository.GetValues();
            foreach (Period period in periods)
                if (!IsPeriodAvailable(period, checkedPeriod, writeWarnings))
                    return false;

            return available;
        }

        private bool IsPeriodAvailable(Period period, Period checkedPeriod, bool writeWarnings)
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

        private bool IsDoctorAvailable(Period period, Period checkedPeriod, bool writeWarnings)
        {
            bool available = true;
            if (DoPeriodsOverlap(period, checkedPeriod))
            {
                if (writeWarnings)
                    Validate.ShowOkDialog("Warning", "Doctor has an existing appointment at selected time!");

                available = false;
            }

            return available;
        }

        private bool IsPatientAvailable(Period period, Period checkedPeriod, bool writeWarnings)
        {
            bool available = true;
            if (DoPeriodsOverlap(period, checkedPeriod))
            {
                if (writeWarnings)
                    Validate.ShowOkDialog("Warning", "Patient has an existing appointment at selected time!");

                available = false;
            }

            return available;
        }

        public bool DoPeriodsOverlap(Period period, Period checkedPeriod)
        {
            if (period.Equals(checkedPeriod))//u slucaju kad edituje period
                return false;

            DateTime endingPeriodTime = period.StartTime.AddMinutes(period.Duration);
            DateTime endingCheckedPeriodTime = checkedPeriod.StartTime.AddMinutes(checkedPeriod.Duration);

            if ((checkedPeriod.StartTime >= period.StartTime && checkedPeriod.StartTime < endingPeriodTime) || (endingCheckedPeriodTime > period.StartTime && endingCheckedPeriodTime <= endingPeriodTime))
                return true;

            return false;
        }

        #endregion

    }
}
