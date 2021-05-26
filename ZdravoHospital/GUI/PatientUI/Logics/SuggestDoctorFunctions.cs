using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.PatientUI.DTOs;
using Period = Model.Period;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public  class SuggestDoctorFunctions
    {
        public PeriodRepository PeriodRepository { get; set; }
        public PeriodFunctions PeriodFunctions { get; set; }
        public SuggestDoctorFunctions(string username)
        {
            PeriodRepository = new PeriodRepository();
            PeriodFunctions = new PeriodFunctions();
        }

        public void SuggestDoctor(Period checkedPeriod, ObservableCollection<DoctorDTO> doctorList)
        {
            foreach (DoctorDTO doctor in doctorList.ToList())
            {
                RemoveUnavailableDoctorFromCollection(doctor, checkedPeriod, doctorList);
            }
        }

        private void RemoveUnavailableDoctorFromCollection(DoctorDTO doctor, Period checkedPeriod, ObservableCollection<DoctorDTO> doctorList)
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
