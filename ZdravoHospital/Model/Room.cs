using System;

namespace Model
{
    public class Room
    {
        public RoomType RoomType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }

        public System.Collections.Generic.List<Inventory> Inventory { get; set; }

    }
}