using System;

namespace Model
{
    public class PersonNotification
    {
        public string Username { get; set; }
        public int NotificationId { get; set; }
        public bool IsRead { get; set; }

        public PersonNotification(string username, int notificationId, bool isRead)
        {
            this.Username = username;
            this.NotificationId = notificationId;
            this.IsRead = isRead;
        }
    }
}