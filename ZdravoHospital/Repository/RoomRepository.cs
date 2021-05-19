using System;
using System.Collections.Generic;
using System.Threading;

namespace Model.Repository
{
    public class RoomRepository : Repository<int, Room>
    {
        private static string path = @"../../../Resources/rooms.json";

        public RoomRepository() : base(path)
        {
        }
        
        public override Room GetById(int id)
        {
            mutex.WaitOne();

            List<Room> allRooms = GetValues();

            foreach (Room room in allRooms)
            {
                if (room.Id == id)
                {
                    mutex.ReleaseMutex();
                    return room;
                }
            }
            mutex.ReleaseMutex();
            return null;
        }

        public override void DeleteById(int id)
        {
            mutex.WaitOne();
            var values = GetValues();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
            mutex.ReleaseMutex();
        }

        public override void Update(Room newValue)
        {
            mutex.WaitOne();
            var values = GetValues();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
            mutex.ReleaseMutex();
        }
    }
}