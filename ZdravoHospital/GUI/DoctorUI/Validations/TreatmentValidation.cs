using Model;
using System;
using ZdravoHospital.GUI.DoctorUI.Exceptions;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class TreatmentValidation
    {
        private PeriodService _periodService;

        public TreatmentValidation()
        {
            _periodService = new PeriodService();
        }

        public void ValidateTreatment(Period period)
        {
            DateTime endDate = period.Treatment.StartDate.AddDays(period.Treatment.Duration); 

            foreach (Period p in _periodService.GetPeriods())
            {
                if (p.Treatment == null || period.PeriodId == p.PeriodId)
                    continue;

                DateTime existingTherapyEndDate = p.Treatment.StartDate.AddDays(p.Treatment.Duration);

                if (period.Treatment.RoomId == p.Treatment.RoomId)
                {
                    if (period.Treatment.StartDate >= p.Treatment.StartDate && period.Treatment.StartDate < existingTherapyEndDate)
                        throw new RoomUnavailableException();

                    if (endDate > p.Treatment.StartDate && endDate < existingTherapyEndDate)
                        throw new RoomUnavailableException();

                    if (period.Treatment.StartDate < p.Treatment.StartDate && endDate > existingTherapyEndDate)
                        throw new RoomUnavailableException();
                }
            }
        }
    }
}
