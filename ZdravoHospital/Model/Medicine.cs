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
        public List<string> Replacements { get; set; }
        

        public List<Ingredient> Ingredients { get; set; }

        public Medicine(string name)
        {
            MedicineName = name;
            Ingredients = new List<Ingredient>();
            Replacements = new List<string>();
        }

        public Medicine(Medicine medicine)
        {
            this.MedicineName = medicine.MedicineName;
            this.Supplier = medicine.Supplier;
            this.Status = medicine.Status;
            this.Note = medicine.Note;
            this.Replacements = new List<string>(medicine.Replacements);
            this.Ingredients = new List<Ingredient>(medicine.Ingredients);
        }

        public Medicine()
        {
            Status = MedicineStatus.STAGED;
            Ingredients = new List<Ingredient>();
            Replacements = new List<string>();
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