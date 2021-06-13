using Model;
using System;
using ZdravoHospital.GUI.DoctorUI.Exceptions;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class TreatmentValidation
    {
        private PeriodService _periodService;
        private BedService _bedService;

        public TreatmentValidation()
        {
            _periodService = new PeriodService();
            _bedService = new BedService();
        }

        public void ValidateTreatment(Period period)
        {
            int availableBedsCount = _bedService.GetRoomBedCount(period.Treatment.RoomId);
            DateTime treatmentEndTime = period.Treatment.StartDate.AddDays(period.Treatment.Duration); 

            foreach (Period p in _periodService.GetPeriods())
            {
                if (p.Treatment == null || period.PeriodId == p.PeriodId)
                    continue;

                DateTime existingTreatmentEndTime = p.Treatment.StartDate.AddDays(p.Treatment.Duration);

                if (period.Treatment.RoomId == p.Treatment.RoomId &&
                    CheckTreatmentOverlap(period.Treatment.StartDate, treatmentEndTime, p.Treatment.StartDate, existingTreatmentEndTime))
                {
                    availableBedsCount--;

                    if (availableBedsCount == 0)
                        throw new BedsUnavailableException();
                }
            }
        }

        public bool CheckTreatmentOverlap(DateTime treatmentStartTime, DateTime treatmentEndTime,
            DateTime existingTreatmentStartTime, DateTime existingTreatmentEndTime)
        {
            if (treatmentStartTime >= existingTreatmentStartTime && treatmentStartTime < existingTreatmentEndTime)
                return true;

            if (treatmentEndTime > existingTreatmentStartTime && treatmentEndTime < existingTreatmentEndTime)
                return true;

            if (treatmentStartTime < existingTreatmentStartTime && treatmentEndTime > existingTreatmentEndTime)
                return true;

            return false;
        }
    }
}
