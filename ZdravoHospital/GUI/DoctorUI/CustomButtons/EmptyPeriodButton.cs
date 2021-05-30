using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.DoctorUI
{
    class EmptyPeriodButton : Button
    {
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }

    }
}
