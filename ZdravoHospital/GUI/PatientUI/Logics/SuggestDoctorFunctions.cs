using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using Period = Model.Period;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public  class SuggestDoctorFunctions
    {
        public PeriodFunctions PeriodFunctions { get; set; }
        public Period FundamentalPeriod { get; private set; }
        public List<DoctorDTO> FreeDoctors { get; private set; }
        public InjectFunctions Injection { get;private set; }
        public ObservableCollection<PeriodDTO> SuggestedPeriods { get; private set; }


        public SuggestDoctorFunctions(DateTime date,TimeSpan time,ObservableCollection<PeriodDTO> suggestedPeriods)
        {
            SetProperties();
            SuggestedPeriods = suggestedPeriods;
            FundamentalPeriod = new Period
            {
                PatientUsername = PatientWindowVM.PatientUsername,
                Duration = 30,
                StartTime = date+time,
                PeriodId = PeriodFunctions.GeneratePeriodId(),
                PeriodType = PeriodType.APPOINTMENT
            };
        }

        private void SetProperties()
        {
            Injection = new InjectFunctions();
            PeriodFunctions = new PeriodFunctions();
            FreeDoctors = new List<DoctorDTO>();
        }

        public void GetSuggestedPeriods()
        {
            
            if (!PeriodFunctions.CheckPeriodAvailability(FundamentalPeriod))
                return;

            Injection.FillDoctorDTOCollection(FreeDoctors);
            RemoveUnavailableDoctors();
            GenerateSuggestedPeriods();
        }

        private void RemoveUnavailableDoctors()
        {
            List<DoctorDTO> doctors = new List<DoctorDTO>();
            Injection.FillDoctorDTOCollection(doctors);
            foreach (var doctor in doctors) 
                RemoveUnavailableDoctor(doctor);

        }

        private void GenerateSuggestedPeriods()
        {
            foreach (var doctor in FreeDoctors)
                SuggestedPeriods.Add(GetPeriodDTO(doctor));

            if (SuggestedPeriods.Count != 0) return;
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowOkDialog("Warning","No available doctors for the selected time!");
        }

        private void RemoveUnavailableDoctor(DoctorDTO doctor)
        {
            DoctorFunctions doctorFunctions = new DoctorFunctions();
            List<Period> periods = PeriodFunctions.GetAllPeriods();

                if (!doctorFunctions.IsTimeInDoctorsShift(FundamentalPeriod.StartTime, doctor.Username))//ukloni ukoliko doktor nije u smeni u datom vremenu
                    FreeDoctors.RemoveAll(p => p.Username.Equals(doctor.Username));

            if (periods.Any(period => period.DoctorUsername.Equals(doctor.Username) && PeriodFunctions.DoPeriodsOverlap(period, FundamentalPeriod)))//ukloni preglede tokom kojih vec ima zakazano
                FreeDoctors.RemoveAll(p => p.Username.Equals(doctor.Username));

        }

        private PeriodDTO GetPeriodDTO(DoctorDTO doctor)
        {
            PeriodConverter periodConverter = new PeriodConverter();
            FillOutPeriod(doctor);
            return periodConverter.GetPeriodDTO(FundamentalPeriod);
        }

        private void FillOutPeriod(DoctorDTO doctor)
        {
            RoomSheduleFunctions roomFunctions = new RoomSheduleFunctions();
            FundamentalPeriod.DoctorUsername = doctor.Username;
            FundamentalPeriod.PeriodId = PeriodFunctions.GeneratePeriodId();
            FundamentalPeriod.RoomId = roomFunctions.GetFreeRoom(FundamentalPeriod);
        }
    }
}
