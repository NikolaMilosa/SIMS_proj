using Model;
using Repository.RoomSchedulePersistance;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.DoctorUI.Exceptions;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class RoomScheduleValidation
    {
        private RoomScheduleRepository _roomScheduleRepository;

        public RoomScheduleValidation()
        {
            _roomScheduleRepository = new RoomScheduleRepository();
        }

        public void ValidateRoomScheduleAvailability(Period period)
        {
            DateTime periodEndTime = period.StartTime.AddMinutes(period.Duration);

            foreach (RoomSchedule roomSchedule in _roomScheduleRepository.GetValues())
                if (CheckEventRenovationOverlap(period.StartTime, periodEndTime, roomSchedule.StartTime, roomSchedule.EndTime))
                    throw new RoomRenovatingException();
        }

        public void ValidateRoomScheduleAvailability(Treatment treatment)
        {
            DateTime treatmentEndTime = treatment.StartTime.AddMinutes(treatment.Duration);

            foreach (RoomSchedule roomSchedule in _roomScheduleRepository.GetValues())
                if (CheckEventRenovationOverlap(treatment.StartTime, treatmentEndTime, roomSchedule.StartTime, roomSchedule.EndTime))
                    throw new RoomRenovatingException();
        }

        public bool CheckEventRenovationOverlap(DateTime eventStartTime, DateTime eventEndTime,
            DateTime renovationStartTime, DateTime renovationEndTime)
        {
            if (eventStartTime >= renovationStartTime && eventStartTime < renovationEndTime)
                return true;

            if (eventEndTime > renovationStartTime && eventEndTime < renovationEndTime)
                return true;

            if (eventStartTime < renovationStartTime && eventEndTime > renovationEndTime)
                return true;

            return false;
        }
    }
}
