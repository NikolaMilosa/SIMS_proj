using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class PeriodsViewHolderDTO
    {
        public Period BestPeriod { get; set; }
        public List<Period> Periods { get; set; }
        public UrgentPeriodStatus Status { get; set; }
    }
}
