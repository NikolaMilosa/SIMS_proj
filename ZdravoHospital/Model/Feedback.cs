using System;

namespace Model
{
    public class Feedback
    {
        public string SenderUsername { get; set; }
        public FeedbackType Type { get; set; }
        public string Text { get; set; }
        public Guid Id { get; set; }

    }
}