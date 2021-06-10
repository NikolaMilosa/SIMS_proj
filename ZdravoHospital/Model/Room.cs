using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Model
{
    public class Room
    {
        public RoomType RoomType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
        public Room(RoomType rt, int id, string name, bool a)
        {
            this.RoomType = rt;
            this.Id = id;
            this.Name = name;
            this.Available = a;
        }

        public Room()
        {

        }
        public override string ToString()
        {
            return Id + " | " + Name;
        }

        public bool IsAppointmentRoom()
        {
            if (RoomType == RoomType.APPOINTMENT_ROOM)
                return true;

            return false;
        }
    }
}