using System;
using System.Collections.Generic;
using System.Threading;

namespace Model.Repository
{
    public class RoomRepository : Repository<int, Room>
    {
        private static string path = @"../../../Resources/rooms.json";
        private static Mutex mutex;

        private Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public RoomRepository() : base(path)
        {
        }

        public override List<Room> GetValues()
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            GetMutex().ReleaseMutex();
            return values;
        }

        public override Room GetById(int id)
        {
            GetMutex().WaitOne();

            List<Room> allRooms = GetValues();

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
            GetMutex().WaitOne();
            var values = GetValues();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Update(Room newValue)
        {
            GetMutex().WaitOne();
            var values = GetValues();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Create(Room newValue)
        {
            GetMutex().WaitOne();
            base.Create(newValue);
            GetMutex().ReleaseMutex();
        }
    }
}