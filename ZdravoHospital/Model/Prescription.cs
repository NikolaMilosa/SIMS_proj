using System;
using System.Collections.Generic;

namespace Model
{
   public class Prescription
   {
   
        public Prescription() { }

        public int Id { get; set; }
        public string DoctorUsername { get; set; }
        public  List<Therapy> TherapyList { get; set; }

    }
}