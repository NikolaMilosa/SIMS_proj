using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZdravoHospital.Repository.IngredientPersistance
{
    public class IngredientRepository : IIngredientRepository
    {
        private string _path = @"..\..\..\Resources\ingredients.json";
        public void Create(Ingredient newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.IngredientName.Equals(id));
            Save(values);
        }

        public Ingredient GetById(string id)
        {
            List<Ingredient> ingredients = GetValues();
            foreach (Ingredient ingredient in ingredients)
                if (ingredient.IngredientName.Equals(id))
                    return ingredient;

            return null;
        }

        public List<Ingredient> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Ingredient>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<Ingredient>();
            }

            return values;
        }

        public void Save(List<Ingredient> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Ingredient newValue)
        {
            List<Ingredient> ingredients = GetValues();
            ingredients[ingredients.FindIndex(patient => patient.IngredientName.Equals(newValue.IngredientName))] = newValue;
            Save(ingredients);
        }
    }
}
