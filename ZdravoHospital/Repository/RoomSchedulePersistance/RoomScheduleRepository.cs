using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Repository.RoomSchedulePersistance
{
    public class RoomScheduleRepository : IRoomScheduleRepository
    {
        private static string _path = @"..\..\..\Resources\roomSchedule.json";
        private static Mutex _mutex;

        public RoomScheduleRepository()
        {
        }

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }

        public void Create(RoomSchedule newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteById(int id)
        {
            throw new NotSupportedException();
        }

        public RoomSchedule GetById(int id)
        {
            throw new NotSupportedException();
        }

        public List<RoomSchedule> GetValues()
        {
            GetMutex().WaitOne();
            var values = JsonConvert.DeserializeObject<List<RoomSchedule>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<RoomSchedule>();
            }
            GetMutex().ReleaseMutex();
            return values;
        }

        public void Save(List<RoomSchedule> values)
        {
            File.WriteAllText(_path,JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public void Update(RoomSchedule newValue)
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

        public bool ExistsInDataBase(RoomSchedule roomSchedule)
        {
            var values = GetValues();
            var foundValue = values.Find(val => val.RoomId == roomSchedule.RoomId &&
                                                val.StartTime.Equals(roomSchedule.StartTime) &&
                                                val.EndTime.Equals(roomSchedule.EndTime) &&
                                                val.ScheduleType == roomSchedule.ScheduleType);
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
    }
}