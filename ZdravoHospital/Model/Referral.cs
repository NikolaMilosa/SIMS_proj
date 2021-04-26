using System;

namespace Model
{
    public class Referral
    {
        public string ReferredDoctorUsername { get; set; }
        public int NumberOfDays { get; set; }
        public string Note { get; set; }
        public bool IsUsed { get; set; }

    }
}