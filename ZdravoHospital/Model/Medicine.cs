using System;
using System.Collections.Generic;

namespace Model
{
    public class Medicine
    {
        public string MedicineName { get; set; }
        public string Supplier { get; set; }

        public List<Ingredient> Ingredients { get; set; }

        public Medicine(string name)
        {
            MedicineName = name;
        }

        public Medicine(string name, string supplier)
        {
            MedicineName = name;
            Supplier = supplier;
        }

        public override string ToString()
        {
            return MedicineName;
        }
    }
}