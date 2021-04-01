using System;
using System.Collections.Generic;

namespace Model
{
    public class Room
    {
        public RoomType RoomType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }

        public System.Collections.Generic.List<Inventory> Inventory { get; set; }
        public Room(RoomType rt, int id, string name, bool a)
        {
            this.RoomType = rt;
            this.Id = id;
            this.Name = name;
            this.Available = a;
            this.Inventory = new List<Inventory>();
        }

    }
}