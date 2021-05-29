using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.RoomSchedulePersistance;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class RoomSheduleFunctions
    {
        #region Properties
        List<Room> Rooms { get; set; }
        public RoomFunctions RoomFunctions { get; private set; }
       public PeriodFunctions PeriodFunctions { get; private set; }
        #endregion

        public RoomSheduleFunctions()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            PeriodFunctions = new PeriodFunctions();
            RoomFunctions = new RoomFunctions();
            Rooms = RoomFunctions.GetAll();
        }

        public int GetFreeRoom(Period checkedPeriod)//vraca prvi slobodan Appointment room za zadati termin
        {
            int roomId = -1;

            foreach (var room in Rooms.Where(room => GetFreeRoomId(room, checkedPeriod) != -1))
                return room.Id;

            //Validate.ShowOkDialog("Warning", "There is no free rooms at selected time!");
            return roomId;
        }

        private int GetFreeRoomId(Room room, Period checkedPeriod)
        {
            int roomId = -1;

            if (!room.IsAppointmentRoom() || !IsRoomAvailableForGivenPeriod(room, checkedPeriod)) return roomId;
            return !PeriodAlreadyExistsInRoom(room, checkedPeriod) ? room.Id : roomId;
        }
        
        private bool IsRoomAvailableForGivenPeriod(Room room, Period checkedPeriod)
        {
            RoomScheduleRepository roomScheduleRepository = new RoomScheduleRepository();
            List<RoomSchedule> roomSchedules = roomScheduleRepository.GetValues();
            return roomSchedules.All(roomSchedule => !roomSchedule.RoomId.Equals(room.Id) || !PeriodFunctions.DoPeriodsOverlap(GeneratePeriodFromSchedule(roomSchedule), checkedPeriod));
        }

        private Period GeneratePeriodFromSchedule(RoomSchedule roomSchedule)
        {
            Period period = new Period
            {
                StartTime = roomSchedule.StartTime,
                Duration = roomSchedule.EndTime.Subtract(roomSchedule.StartTime).Minutes,
                RoomId = -1
            };
            return period;
        }

        private bool PeriodAlreadyExistsInRoom(Room room, Period checkedPeriod)
        {

            PeriodFunctions periodFunctions = new PeriodFunctions();
            List<Period> periods = periodFunctions.GetAllPeriods();
            return periods.Any(period => period.RoomId == room.Id && PeriodFunctions.DoPeriodsOverlap(period, checkedPeriod));
        }
    }
}
