using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.Services.Strategy
{
    public class SuggestDoctorService : SuggestAbstract,ISuggestStrategy
    {
        public Period FundamentalPeriod { get; private set; }
        public List<DoctorDTO> FreeDoctors { get; private set; }

        public SuggestDoctorService(ObservableCollection<PeriodDTO> suggestedPeriods, DateTime date, TimeSpan time) :base(suggestedPeriods)
        {
            SetPeriod(date,time);
            FreeDoctors = new List<DoctorDTO>();
        }
        public void Suggest()
        {
            GetSuggestedPeriods();
        }

        private void SetPeriod(DateTime date, TimeSpan time)
        {
            FundamentalPeriod = new Period
            {
                PatientUsername = PatientWindowVM.PatientUsername,
                Duration = 30,
                StartTime = date + time,
                PeriodId = PeriodService.GeneratePeriodId(),
                PeriodType = PeriodType.APPOINTMENT
            };
        }

        private void GetSuggestedPeriods()
        {

            if (!PeriodService.CheckPeriodAvailability(FundamentalPeriod))
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
            ViewService viewFunctions = new ViewService();
            viewFunctions.ShowOkDialog("Warning", "No available doctors for the selected time!");
        }

        private void RemoveUnavailableDoctor(DoctorDTO doctor)
        {
            DoctorService doctorFunctions = new DoctorService();
            List<Period> periods = PeriodService.GetAllPeriods();

            if (!doctorFunctions.IsTimeInDoctorsShift(FundamentalPeriod.StartTime, doctor.Username))//ukloni ukoliko doktor nije u smeni u datom vremenu
                FreeDoctors.RemoveAll(p => p.Username.Equals(doctor.Username));

            if (periods.Any(period => period.DoctorUsername.Equals(doctor.Username) && PeriodService.DoPeriodsOverlap(period, FundamentalPeriod)))//ukloni preglede tokom kojih vec ima zakazano
                FreeDoctors.RemoveAll(p => p.Username.Equals(doctor.Username));

        }

        private PeriodDTO GetPeriodDTO(DoctorDTO doctor)
        {
            FillOutPeriod(doctor);
            return PeriodConverter.GetPeriodDTO(FundamentalPeriod);
        }

        private void FillOutPeriod(DoctorDTO doctor)
        {
            FundamentalPeriod.DoctorUsername = doctor.Username;
            FundamentalPeriod.PeriodId = PeriodService.GeneratePeriodId();
            FundamentalPeriod.RoomId = RoomScheduleService.GetFreeRoom(FundamentalPeriod);
        }
    }
}
