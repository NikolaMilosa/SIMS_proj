using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Model;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.Services.Manager
{
    public class DoctorReportService
    {
        #region Fields

        private IPeriodRepository _periodRepository;
        private IPatientRepository _patientRepository;

        #endregion

        public DoctorReportService(InjectorDTO injector)
        {
            _periodRepository = injector.PeriodRepository;
            _patientRepository = injector.PatientRepository;
        }

        public ObservableCollection<DoctorReportDTO> CreateReport(Doctor doctor, DateTime start, DateTime end)
        {
            var report = new List<DoctorReportDTO>();

            var periods = _periodRepository.GetValues();

            foreach (var period in periods)
            {
                if (period.DoctorUsername.Equals(doctor.Username) &&
                    ((period.StartTime < start && period.StartTime.AddMinutes(period.Duration) > start) ||
                     (period.StartTime > start && period.StartTime.AddMinutes(period.Duration) < end) ||
                     (period.StartTime < end && period.StartTime.AddMinutes(period.Duration) > end)))
                {
                    report.Add(new DoctorReportDTO()
                    {
                        Date = period.StartTime,
                        PatientName = _patientRepository.GetById(period.PatientUsername).Name,
                        PatientUsername = period.PatientUsername,
                        RoomNumber = period.RoomId,
                        Type = (period.PeriodType == PeriodType.APPOINTMENT) ? "APPOINTMENT" : "OPERATION"
                    });
                }
            }
            
            return new ObservableCollection<DoctorReportDTO>(report);
        }
    }
}
