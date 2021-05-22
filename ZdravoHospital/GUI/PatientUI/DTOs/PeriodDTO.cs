using System;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Converters;

namespace ZdravoHospital.GUI.PatientUI.DTOs
{
    public class PeriodDTO 
    { 
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
        public DateTime Date { get; set; }
        public int RoomNumber { get; set; }
        public PeriodType PeriodType { get; set; }
        public int PeriodId { get; set; }
        //
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
