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
        public Dictionary<string, bool> UsernameRecievers { get; set; }

    }
}