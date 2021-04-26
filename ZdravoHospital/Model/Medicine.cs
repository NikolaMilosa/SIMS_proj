using System;
using System.Collections.Generic;

namespace Model
{
    public class Medicine
    {
        public string MedicineName { get; set; }
        public string Supplier { get; set; }
        public MedicineStatus Status { get; set; }
        public string Note { get; set; }
        public List<string> ReplacementMedication { get; set; }
        

        public List<Ingredient> Ingredients { get; set; }

        public Medicine(string name)
        {
            MedicineName = name;
            Ingredients = new List<Ingredient>();
            ReplacementMedication = new List<string>();
        }

        public Medicine()
        {
            Ingredients = new List<Ingredient>();
            ReplacementMedication = new List<string>();
        }

        //public Medicine(string name, string supplier)
        //{
        //    MedicineName = name;
        //    Supplier = supplier;
        //}

        public override string ToString()
        {
            return MedicineName;
        }
    }
}