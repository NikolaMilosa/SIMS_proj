using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.ViewModels;

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
        public string ErrorMessage { get; private set; }
        #endregion

        public PeriodFunctions()
        {
            PeriodRepository = new PeriodRepository();
            PatientUsername = PatientWindowVM.PatientUsername;
        }

        #region Methods
   

        public  void UpdatePeriod(Period period)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            periodRepository.Update(period);
            Validate.ShowOkDialog("Appointment", "Appointment is succesfully edited!");
        }

        public  void SerializeNewPeriod(Period period)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            periodRepository.Create(period);
            Validate.ShowOkDialog("Appointment", "Appointment is succesfully added!");
        }
        //
        public  bool IsPeriodWithinGivenMinutes(DateTime dateTime, int minutes)
        {
            bool itIs = false || dateTime >= DateTime.Now && dateTime <= DateTime.Now.AddMinutes(minutes);

            return itIs;
        }

        public int GeneratePeriodId()
        {
            if (PeriodRepository.GetValues().Count == 0)
                return 0;

            return PeriodRepository.GetValues().Last().PeriodId + 1;//vrati vrednost za jedan vecu od poslednjeg id-a iz liste
        }
        //
        public bool CheckPeriodAvailability(Period checkedPeriod, bool writeWarnings)
        {
            List<Period> periods = PeriodRepository.GetValues();
            foreach (Period period in periods)
                if (!IsPeriodAvailable(period, checkedPeriod, writeWarnings))
                    return false;

            return true;
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
                ErrorMessage = "Doctor has an existing appointment at selected time!";

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
                ErrorMessage = "Patient has an existing appointment at selected time!";
                available = false;
            }

            return available;
        }

        public bool DoPeriodsOverlap(Period period, Period checkedPeriod)
        {
            if (period.PeriodId.Equals(checkedPeriod.PeriodId))//u slucaju kad edituje period
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
