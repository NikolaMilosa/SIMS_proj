using Model;
using System;

namespace Repository.RoomPersistance
{
    public interface IRoomRepository : IRepository<int, Room>
    {
        Room FindRoomByPrio(Room notThisRoom);
    }
}