using System;
using System.Threading;

namespace Model.Repository
{
    public class NotificationRepository : Repository<int, Notification>
    {
        private static string path = @"..\..\..\Resources\notifications.json";

        public NotificationRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            return new Mutex();
        }

        public override Notification GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Notification newValue)
        {
            throw new NotImplementedException();
        }
    }
}