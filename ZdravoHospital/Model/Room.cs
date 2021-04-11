using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Model
{
    public class Room : INotifyPropertyChanged
    {
        public RoomType RoomType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }

        public Dictionary<string, int> Inventory { get; set; }
        public Room(RoomType rt, int id, string name, bool a)
        {
            this.RoomType = rt;
            this.Id = id;
            this.Name = name;
            this.Available = a;
            this.Inventory = new Dictionary<string, int>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}