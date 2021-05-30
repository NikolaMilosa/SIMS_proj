using Model;
using Repository.PeriodPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.DoctorUI.Exceptions;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class PeriodValidation
    {
        private PeriodRepository periodRepository;

        public PeriodValidation()
        {
            periodRepository = new PeriodRepository();
        }

        public void ValidatePeriod(Period period, bool updating = false)
        {
            if (period.StartTime < DateTime.Now)
                throw new PeriodInPastException();

            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);
            List<Period> periods;
            
            if (!updating)
                periods = periodRepository.GetValues();
            else
                periods = periodRepository.GetValues().Where(p => !p.PeriodId.Equals(period.PeriodId)).ToList();
            
            ValidateRoomAvailability(period, periodEndtime, periods);
            ValidateDoctorAvailability(period, periodEndtime, periods);
            ValidatePatientAvailability(period, periodEndtime, periods);
        }

        private void ValidateRoomAvailability(Period period, DateTime periodEndTime, List<Period> periods)
        {
            foreach (Period existingPeriod in periods)
            {
                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.RoomId == existingPeriod.RoomId)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        throw new RoomUnavailableException();

                    if (periodEndTime > existingPeriod.StartTime && periodEndTime < existingPeriodEndTime)
                        throw new RoomUnavailableException();

                    if (period.StartTime < existingPeriod.StartTime && periodEndTime > existingPeriodEndTime)
                        throw new RoomUnavailableException();
                }
            }
        }

        private void ValidateDoctorAvailability(Period period, DateTime periodEndTime, List<Period> periods)
        {
            foreach (Period existingPeriod in periods)
            {
                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        throw new DoctorUnavailableException();

                    if (periodEndTime > existingPeriod.StartTime && periodEndTime < existingPeriodEndTime)
                        throw new DoctorUnavailableException();

                    if (period.StartTime < existingPeriod.StartTime && periodEndTime > existingPeriodEndTime)
                        throw new DoctorUnavailableException();
                }
            }
        }

        private void ValidatePatientAvailability(Period period, DateTime periodEndTime, List<Period> periods)
        {
            foreach (Period existingPeriod in periods)
            {
                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.PatientUsername == existingPeriod.PatientUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        throw new PatientUnavailableException();

                    if (periodEndTime > existingPeriod.StartTime && periodEndTime < existingPeriodEndTime)
                        throw new PatientUnavailableException();

                    if (period.StartTime < existingPeriod.StartTime && periodEndTime > existingPeriodEndTime)
                        throw new PatientUnavailableException();
                }
            }
        }
    }
}
