using System;

namespace Model
{
    public class Referral
    {
        public int ReferralId { get; set; }
        public string ReferringDoctorUsername { get; set; }
        public string ReferredDoctorUsername { get; set; }
        public int DaysToUse { get; set; }
        public string Note { get; set; }
        public bool IsUsed { get; set; }
        public Period Period { get; set; }
        //todo: change to periodID
    }
}