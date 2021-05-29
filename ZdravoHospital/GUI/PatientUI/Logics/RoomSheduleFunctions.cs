using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class RoomSheduleFunctions
    {
        #region Properties
        List<Room> Rooms { get; set; }
        PeriodFunctions PeriodFunctions { get; set; }
        #endregion

        public RoomSheduleFunctions()
        {
            SetProperties();
            PeriodFunctions = new PeriodFunctions();
        }

        private void SetProperties()
        {
            PeriodRepository periodRepository = new PeriodRepository();
            List<Period> periods = periodRepository.GetValues();
            RoomRepository roomRepository = new RoomRepository();
            Rooms = roomRepository.GetValues();
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
            bool available = true;
            RoomScheduleRepository roomScheduleRepository = new RoomScheduleRepository();
            List<RoomSchedule> roomSchedules = roomScheduleRepository.GetValues();
            foreach (RoomSchedule roomSchedule in roomSchedules)
            {
                if (!roomSchedule.RoomId.Equals(room.Id) ||
                    !PeriodFunctions.DoPeriodsOverlap(GeneratePeriodFromSchedule(roomSchedule), checkedPeriod))
                    continue;
                available = false;
                break;
            }
            return available;
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
           
            PeriodRepository periodRepository = new PeriodRepository();
            List<Period> periods = periodRepository.GetValues();
            return periods.Any(period => period.RoomId == room.Id && PeriodFunctions.DoPeriodsOverlap(period, checkedPeriod));
        }
    }
}
