using System;
using System.Collections.Generic;

namespace Model.Repository
{
    public class RoomRepository : Repository<int, Room>
    {
        private static string path = @"../../../Resources/rooms.json";

        public RoomRepository() : base(path)
        {
        }

        public Room FindRoomByPrio(RoomType roomType)
        {
            List<Room> allRooms = GetValues();

            foreach (Room room in allRooms)
            {
                if (room.RoomType == roomType)
                {
                    return room;
                }
            }

            return null;
        }

        public override Room GetById(int id)
        {
            List<Room> allRooms = GetValues();

            foreach (Room room in allRooms)
            {
                if (room.Id == id)
                {
                    return room;
                }
            }

            return null;
        }

        public override void DeleteById(int id)
        {
            var values = GetValues();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
        }

        public override void Update(Room newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
        }
    }
}