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

        public string Format()
        {
            string ret = "";

            ret += "   Type : " + Type;
            ret += "\n     Patient name : " + PatientName;
            ret += "\n     Patient username : " + PatientUsername;
            ret += "\n     Room number : " + RoomNumber;
            ret += "\n     Date : " + Date.Date;

            return ret;
        }
    }
}
