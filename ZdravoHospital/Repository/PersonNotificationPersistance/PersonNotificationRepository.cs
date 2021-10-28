using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Repository.PersonNotificationPersistance
{
    public class PersonNotificationRepository : IPersonNotificationRepository
    {
        private string _path = @"..\..\..\Resources\personNotifications.json";
        public void Create(PersonNotification newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(int id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.NotificationId.Equals(id));
            Save(values);
        }

        public PersonNotification GetById(int id)
        {
            List<PersonNotification> notifications = GetValues();
            foreach (PersonNotification notification in notifications)
                if (notification.NotificationId.Equals(id))
                    return notification;

            return null;
        }

        public List<PersonNotification> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<PersonNotification>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<PersonNotification>();
            }

            return values;
        }

        public void Save(List<PersonNotification> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(PersonNotification newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username) && val.NotificationId.Equals(newValue.NotificationId))] = newValue;
            Save(values);
        }
    }
}