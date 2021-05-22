using Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Repository.RoomPersistance
{
    public class RoomRepository : IRoomRepository
    {
        private static string _path = @"../../../Resources/rooms.json";
        private static Mutex _mutex;

        public RoomRepository()
        {
        }

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }

        public void Create(Room newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public Room FindRoomByPrio(Room notThisRoom)
        {
            var someRoom = FindRoomByType(RoomType.STORAGE_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.BED_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.APPOINTMENT_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.OPERATING_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.EMERGENCY_ROOM, notThisRoom);

            return someRoom;
        }

        private Room FindRoomByType(RoomType rt, Room room)
        {
            if (room != null)
            {
                foreach (var r in GetValues())
                {
                    if (r.Available == true && r.RoomType == rt && r.Id != room.Id)
                        return r;
                }
            }
            else
            {
                foreach (var r in GetValues())
                {
                    if (r.Available == true && r.RoomType == rt)
                        return r;
                }
            }


            return null;
        }

        public void DeleteById(int id)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.Id == id);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public Room GetById(int id)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.Id == id)
                {
                    return val;
                }
            }

            return null;
        }

        public List<Room> GetValues()
        {
            GetMutex().WaitOne();
            var values = JsonConvert.DeserializeObject<List<Room>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<Room>();
            }
            GetMutex().ReleaseMutex();
            return values;
        }

        public void Save(List<Room> values)
        {
            File.WriteAllText(_path,JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Room newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.Id == newValue.Id)] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}