using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public  class SuggestDoctorFunctions
    {
        public PeriodRepository PeriodRepository { get; set; }
        public PeriodFunctions PeriodFunctions { get; set; }
        public SuggestDoctorFunctions(string username)
        {
            PeriodRepository = new PeriodRepository();
            PeriodFunctions = new PeriodFunctions(username);
        }

        public void SuggestDoctor(Period checkedPeriod, ObservableCollection<DoctorView> doctorList)
        {
            foreach (DoctorView doctor in doctorList.ToList())
            {
                RemoveUnavailableDoctorFromCollection(doctor, checkedPeriod, doctorList);
            }
        }

        private void RemoveUnavailableDoctorFromCollection(DoctorView doctor, Period checkedPeriod, ObservableCollection<DoctorView> doctorList)
        { 
            List<Period> periods = PeriodRepository.GetValues();
            foreach (Period period in periods)
            {
                if (period.DoctorUsername.Equals(doctor.Username) && PeriodFunctions.DoPeriodsOverlap(period, checkedPeriod))
                {
                    doctorList.Remove(doctor);
                    break;
                }
            }
        }
    }
}
