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
            DateTime endDate = period.Treatment.StartDate.AddDays(period.Treatment.Duration); 

            foreach (Period p in _periodService.GetPeriods())
            {
                if (p.Treatment == null || period.PeriodId == p.PeriodId)
                    continue;

                DateTime existingTherapyEndDate = p.Treatment.StartDate.AddDays(p.Treatment.Duration);

                if (period.Treatment.RoomId == p.Treatment.RoomId &&
                    CheckTreatmentOverlap(period.Treatment.StartDate, endDate, p.Treatment.StartDate, existingTherapyEndDate))
                {
                    availableBedsCount--;

                    if (availableBedsCount == 0)
                        throw new RoomUnavailableException();

                }
            }
        }

        public bool CheckTreatmentOverlap(DateTime treatmentStartDate, DateTime treatmentEndDate,
            DateTime existingTreatmentStartDate, DateTime existingTreatmentEndDate)
        {
            if (treatmentStartDate >= existingTreatmentStartDate && treatmentStartDate < existingTreatmentEndDate)
                return true;

            if (treatmentEndDate > existingTreatmentStartDate && treatmentEndDate < existingTreatmentEndDate)
                return true;

            if (treatmentStartDate < existingTreatmentStartDate && treatmentEndDate > existingTreatmentEndDate)
                return true;

            return false;
        }
    }
}
