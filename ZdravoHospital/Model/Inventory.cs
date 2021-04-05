using System;

namespace Model
{
    public class Inventory
    {
        public string Name { get; set; }
        public string Suplier { get; set; }
        public int Quantity { get; set; }
        public InventoryType InventoryType { get; set; }

        public Inventory() { }

        public Inventory(string n, string su, int qu, InventoryType it)
        {
            this.Name = n;
            this.Suplier = su;
            this.Quantity = qu;
            this.InventoryType = it;
        }

    }
}