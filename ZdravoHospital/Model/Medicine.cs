using System;

namespace Model
{
    public class Medicine
    {
        public string MedicineName { get; set; }
        public string Supplier { get; set; }

        public System.Collections.Generic.List<Ingredient> Ingredient { get; set; }

        public Medicine(string name)
        {
            MedicineName = name;
        }
        public override string ToString()
        {
            return MedicineName;
        }
    }
}