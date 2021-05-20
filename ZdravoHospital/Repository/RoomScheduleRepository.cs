using System;
using System.Threading;

namespace Model.Repository
{
    public class RoomScheduleRepository : Repository<int, RoomSchedule>
    {
        private static string path = @"..\..\..\Resources\roomSchedule.json";

        public RoomScheduleRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            return new Mutex();
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
            throw new NotImplementedException();
        }
    }
}