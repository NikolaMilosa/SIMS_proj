using System;
using System.Collections.Generic;

namespace Model
{
   public class Prescription
   {
   
        public Prescription() { }

        public string DoctorUsername { get; set; }
        public DateTime StartHours { get; set; }
        public int TimesPerDay { get; set; }
        public int PauseInDays { get; set; }
        public DateTime EndDate { get; set; }
        public  List<Therapy> TherapyList { get; set; }

    }
}