using Model;
using System;
using System.Collections.Generic;

namespace Repository.RoomPersistance
{
    public interface IRoomRepository : IRepository<int, Room>
    {
        Room FindRoomByPrio(Room notThisRoom);

        List<Room> GetByType(RoomType roomType);
    }
}