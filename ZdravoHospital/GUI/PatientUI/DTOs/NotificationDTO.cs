using System;
using Model;
using Model.Repository;

namespace ZdravoHospital.GUI.PatientUI.DTOs
{
    public class NotificationDTO
    {
        public string Username { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }//Role Name Surname
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }

        public NotificationDTO(string username, int id, DateTime date, string @from, bool seen,string title,string content)
        {
            Username = username;
            Id = id;
            Date = date;
            From = @from;
            Seen = seen;
            Title = title;
            Content = content;
        }
    }
}
