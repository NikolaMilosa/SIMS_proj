using Model;
using Repository.DoctorPersistance;
using Repository.PeriodPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoHospital.GUI.DoctorUI.Exceptions;
using ZdravoHospital.GUI.DoctorUI.Services;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class PeriodValidation
    {
        private PeriodRepository _periodRepository;
        private WorkTimeService _workTimeService;
        private DoctorService _doctorService;

        public PeriodValidation()
        {
            _periodRepository = new PeriodRepository();
            _workTimeService = new WorkTimeService(new DoctorRepository());
            _doctorService = new DoctorService();
        }

        public void ValidatePeriod(Period period, bool updating = false)
        {
            if (period.StartTime < DateTime.Now)
                throw new PeriodInPastException();

            DateTime periodEndTime = period.StartTime.AddMinutes(period.Duration);
            List<Period> periods;
            
            if (!updating)
                periods = _periodRepository.GetValues();
            else
                periods = _periodRepository.GetValues().Where(p => !p.PeriodId.Equals(period.PeriodId)).ToList();
            
            ValidateRoomAvailability(period, periodEndTime, periods);
            ValidateDoctorShift(period, periodEndTime);
            ValidateDoctorAvailability(period, periodEndTime, periods);
            ValidatePatientAvailability(period, periodEndTime, periods);
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

        private void ValidateDoctorShift(Period period, DateTime periodEndTime)
        {
            Shift shift = _workTimeService.GetDoctorShiftByDate(_doctorService.GetDoctor(period.DoctorUsername), period.StartTime);

            switch (shift)
            {
                case Shift.FIRST:
                    if (period.StartTime.Hour >= 6 && periodEndTime.Hour < 14)
                        return;
                    break;
                case Shift.SECOND:
                    if (period.StartTime.Hour >= 14 && periodEndTime.Hour < 22)
                        return;
                    break;
                case Shift.THIRD:
                    if (period.StartTime.Hour >= 22 && periodEndTime.Hour < 6)
                        return;
                    break;
            }

            throw new DoctorShiftException();
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
