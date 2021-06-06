using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class MessageBoxDTO : BindableBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set 
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set 
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        public MessageBoxDTO(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public MessageBoxDTO()
        {

        }
    }
}
