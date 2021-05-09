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
        public  List<Therapy> TherapyList { get; set; }

    }
}