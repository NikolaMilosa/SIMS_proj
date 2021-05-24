using Model;
using System;

namespace Repository.RoomSchedulePersistance
{
   public interface IRoomScheduleRepository : IRepository<int, RoomSchedule>
   {
       bool ExistsInDataBase(RoomSchedule roomSchedule);

       void DeleteByRoomId(int roomId);

       void DeleteByEquality(RoomSchedule roomSchedule);
   }
}