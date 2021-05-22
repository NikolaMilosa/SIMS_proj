using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class PeriodValidation
    {
        private PeriodRepository periodRepository;

        public PeriodValidation()
        {
            periodRepository = new PeriodRepository();
        }

        public int IsPeriodAvailable(Period period) // vraca 0 ako je termin ok, 1 ako je soba zauzeta, 2 ako je doktor zauzet, 3 ako je pacijent zauzet
        {
            if (period.StartTime < DateTime.Now)
                return -1;

            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);

            foreach (Period existingPeriod in periodRepository.GetValues())
            {
                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.RoomId == existingPeriod.RoomId)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 1;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 1;

                    if (period.StartTime < existingPeriod.StartTime && periodEndtime > existingPeriodEndTime)
                        return 1;
                }

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 2;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 2;

                    if (period.StartTime < existingPeriod.StartTime && periodEndtime > existingPeriodEndTime)
                        return 2;
                }

                if (period.PatientUsername == existingPeriod.PatientUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 3;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 3;

                    if (period.StartTime < existingPeriod.StartTime && periodEndtime > existingPeriodEndTime)
                        return 3;
                }
            }

            return 0;
        }
    }
}
