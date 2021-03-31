using System;

namespace Model
{
    public class Prescription
    {
        public string DoctorUsername { get; set; }

        public System.Collections.Generic.List<Therapy> Therapy { get; set; }
    }
}