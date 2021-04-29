using System;
using System.ComponentModel;
using System.Threading;

using ZdravoHospital.GUI.ManagerUI.Logics;

namespace Model
{
    /// - When renovation is scheduled or moving of static inventory
    public class RoomSchedule
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
        public ReservationType ScheduleType { get; set; }

        public void WaitStartRenovation()
        {
            TimeSpan ts = StartTime.Subtract(DateTime.Now);
            if (ts > new TimeSpan(0, 0, 0))
                Thread.Sleep(ts);

            /* schedule waiting for end of renovation */
            RoomScheduleFunctions roomScheduleFunctions = new RoomScheduleFunctions();
            roomScheduleFunctions.ScheduleRenovationEnd(this);
        }

        public void WaitEndRenovation()
        {
            TimeSpan ts = EndTime.Subtract(DateTime.Now);
            if (ts > new TimeSpan(0, 0, 0))
                Thread.Sleep(ts);

            /* end room renovation */
            RoomScheduleFunctions roomScheduleFunctions = new RoomScheduleFunctions();
            roomScheduleFunctions.FinishRenovation(this);
        }
    }
}