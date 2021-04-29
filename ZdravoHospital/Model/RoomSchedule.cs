using System;
using System.ComponentModel;

namespace Model
{
    /// - When renovation is scheduled or moving of static inventory
    public class RoomSchedule
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
    }
}