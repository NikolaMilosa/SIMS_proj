using System;

namespace Model
{
    public class Medicine
    {
        public string MedicineName { get; set; }

        public System.Collections.Generic.List<Ingredient> Ingredient { get; set; }

    }
}