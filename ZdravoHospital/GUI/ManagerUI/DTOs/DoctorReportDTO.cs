using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace ZdravoHospital.GUI.ManagerUI.DTOs
{
    public class DoctorReportDTO
    {
        public string Type { get; set; }
        public string PatientName { get; set; }
        public string PatientUsername { get; set; }
        public int RoomNumber { get; set; }
        public DateTime Date { get; set; }
    }
}
