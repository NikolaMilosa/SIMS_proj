using System;
using System.Collections.Generic;
using System.Threading;

namespace Model.Repository
{
    public class RoomRepository : Repository<int, Room>
    {
        private static string path = @"../../../Resources/rooms.json";
        private static Mutex mutex;
        
        public RoomRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public override Room GetById(int id)
        {
            List<Room> allRooms = base.GetValues();

            GetMutex().WaitOne();
            foreach (Room room in allRooms)
            {
                if (room.Id == id)
                {
                    mutex.ReleaseMutex();
                    return room;
                }
            }
            GetMutex().ReleaseMutex();
            return null;
        }

        public override void DeleteById(int id)
        {
            var values = base.GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Update(Room newValue)
        {
            var values = base.GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}