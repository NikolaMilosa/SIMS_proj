using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Repository.NotificationsPersistance
{
    public class NotificationRepository : INotificationsRepository
    {
        private string _path = @"..\..\..\Resources\notifications.json";
        public void Create(Notification newValue)
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

        public Notification GetById(int id)
        {
            List<Notification> notifications = GetValues();
            foreach (Notification notification in notifications)
                if (notification.NotificationId.Equals(id))
                    return notification;

            return null;
        }

        public List<Notification> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Notification>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<Notification>();
            }

            return values;
        }

        public void Save(List<Notification> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Notification newValue)
        {
            List<Notification> notifications = GetValues();
            notifications[notifications.FindIndex(notification => notification.NotificationId.Equals(newValue.NotificationId))] = newValue;
            Save(notifications);
        }
    }
}