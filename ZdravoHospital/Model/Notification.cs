using System;
using System.Collections.Generic;

namespace Model
{
    public class Notification
    {
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public string UsernameSender { get; set; }
        public string Title { get; set; }
        public int NotificationId { get; set; }

        public Notification(string text, DateTime createDate, string usernameSender, string title, int notificationid)
        {
            Text = text;
            CreateDate = createDate;
            UsernameSender = usernameSender;
            Title = title;
            NotificationId = notificationid;
        }

        public override string ToString()
        {
            return CreateDate.ToShortDateString() + " | " + Title;
        }

    }
}