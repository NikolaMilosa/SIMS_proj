using System;

namespace Model
{
    public class RoomInventory
    {
        public string InventoryId { get; set; }
        public int RoomId { get; set; }
        public int Quantity { get; set; }



        public RoomInventory(string iid, int rid, int q)
        {
            this.InventoryId = iid;
            this.RoomId = rid;
            this.Quantity = q;
        }

    }
}