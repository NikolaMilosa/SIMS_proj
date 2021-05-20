using System;
using System.Threading;
using System.Windows.Automation;

namespace Model.Repository
{
    public class RoomScheduleRepository : Repository<int, RoomSchedule>
    {
        private static string path = @"..\..\..\Resources\roomSchedule.json";
        private static Mutex mutex;

        public RoomScheduleRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public override RoomSchedule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(RoomSchedule newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.RoomId == newValue.RoomId &&
                                           val.StartTime.Equals(newValue.StartTime) &&
                                           val.EndTime.Equals(newValue.EndTime) &&
                                           val.ScheduleType == newValue.ScheduleType)] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteByEquality(RoomSchedule roomSchedule)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.RoomId == roomSchedule.RoomId &&
                                    val.StartTime.Equals(roomSchedule.StartTime) &&
                                    val.EndTime.Equals(roomSchedule.EndTime) &&
                                    val.ScheduleType == roomSchedule.ScheduleType);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public bool ExistsInDataBase(RoomSchedule roomSchedule)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            var foundValue = values.Find(val => val.RoomId == roomSchedule.RoomId &&
                                                val.StartTime.Equals(roomSchedule.StartTime) &&
                                                val.EndTime.Equals(roomSchedule.EndTime) &&
                                                val.ScheduleType == roomSchedule.ScheduleType);
            GetMutex().ReleaseMutex();
            if (foundValue == null)
            {
                return false;
            }

            return true;
        }

        public void DeleteByRoomId(int roomId)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.RoomId == roomId);
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}