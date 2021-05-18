using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class PatientNote
    {
        public string Content { get; set; }
        public DateTime NotifyTime { get; set; }
        public string Title { get; set; }

        public PatientNote() 
        { 
        }
    }
}
