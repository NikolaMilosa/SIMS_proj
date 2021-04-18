using System;
using System.Collections.Generic;

namespace Model
{
   public class Prescription
   {
   
        public Prescription() { }

        public string DoctorUsername { get; set; }
        public  List<Therapy> TherapyList { get; set; }

    }
}