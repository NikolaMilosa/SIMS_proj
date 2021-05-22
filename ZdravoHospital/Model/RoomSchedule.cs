using System;
using System.ComponentModel;
using System.Threading;
using ZdravoHospital.Services.Manager;

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
            RoomScheduleService roomScheduleService = new RoomScheduleService();
            roomScheduleService.ScheduleRenovationEnd(this);
        }

        public void WaitEndRenovation()
        {
            TimeSpan ts = EndTime.Subtract(DateTime.Now);
            if (ts > new TimeSpan(0, 0, 0))
                Thread.Sleep(ts);

            /* end room renovation */
            RoomScheduleService roomScheduleService = new RoomScheduleService();
            roomScheduleService.FinishRenovation(this);
        }
    }
}