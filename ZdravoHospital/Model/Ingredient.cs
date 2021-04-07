using System;

namespace Model
{
    public class Ingredient
    {
        public string IngredientName { get; set; }

        public Ingredient(string name)
        {
            IngredientName = name;
        }
        public override string ToString()
        {
            return IngredientName;
        }
    }
}