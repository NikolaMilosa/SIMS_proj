using System;

namespace Model
{
   /// roomId and initialStartTime -> id of an appointment which is being moved
   public class MovePeriod
   {
        public string DoctorUsername { get; set; }
        public string PatientUsername { get; set; }
        public int RoomId { get; set; }
        public DateTime InitialStartTime { get; set; }
        public DateTime MovedStartTime { get; set; }
        public int Duration { get; set; }

        public MovePeriod()
        {

        }
        public MovePeriod(string doctorUsername, string patientUsername, int roomId, DateTime initialStartTime, DateTime movedStartTime, int duration)
        {
            DoctorUsername = doctorUsername;
            PatientUsername = patientUsername;
            RoomId = roomId;
            InitialStartTime = initialStartTime;
            MovedStartTime = movedStartTime;
            Duration = duration;
        }

    }
}