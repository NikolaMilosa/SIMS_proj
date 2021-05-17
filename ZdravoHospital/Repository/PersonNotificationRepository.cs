using System;

namespace Model.Repository
{
    public class PersonNotificationRepository : Repository<string, PersonNotification>
    {
        private static string path = @"..\..\..\Resources\personNotifications.json";

        public PersonNotificationRepository() : base(path)
        {
        }

        public override PersonNotification GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(PersonNotification newValue)
        {
            throw new NotImplementedException();
        }
    }
}