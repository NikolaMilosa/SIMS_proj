using System;
using Model;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.DTOs
{
    public class PeriodDTO : ViewModel
    { 
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
        private DateTime dateTime;
        public DateTime Date
        {
            get => dateTime;
            set { dateTime = value; OnPropertyChanged(); }
        }
        public int RoomNumber { get; set; }
        public PeriodType PeriodType { get; set; }

        public int PeriodId { get; set; }
        public PeriodDTO(string doctorName, string doctorSurname, DateTime date, int roomNumber, PeriodType periodType, int periodId)
        {
            DoctorName = doctorName;
            DoctorSurname = doctorSurname;
            Date = date;
            RoomNumber = roomNumber;
            PeriodType = periodType;
            PeriodId = periodId;
        }

        public PeriodDTO()
        {
            
        }
    }
}
