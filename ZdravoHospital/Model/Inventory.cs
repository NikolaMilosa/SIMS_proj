using System;

namespace Model
{
    public class Inventory
    {
        public string Name { get; set; }
        public string Supplier { get; set; }
        public int Quantity { get; set; }
        public InventoryType InventoryType { get; set; }

        public string Id { get; set; }

        public Inventory() { }

        public Inventory(string n, string su, int qu, InventoryType it, string i)
        {
            this.Name = n;
            this.Supplier = su;
            this.Quantity = qu;
            this.InventoryType = it;
            this.Id = i;
        }

    }
}