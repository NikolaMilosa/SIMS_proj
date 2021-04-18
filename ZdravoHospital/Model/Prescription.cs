using System;
using System.Collections.Generic;

namespace Model
{
   public class Prescription
   {
   
        public Prescription() 
        {
            TherapyList = new List<Therapy>();
        }

        public int Id { get; set; }
        public string DoctorUsername { get; set; }
        public  List<Therapy> TherapyList { get; set; }

    }
}