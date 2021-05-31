using Model;
using System;
using ZdravoHospital.Repository.IngredientPersistance;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class IngredientService
    {
        private IngredientRepository _ingredientRepository;

        public IngredientService()
        {
            _ingredientRepository = new IngredientRepository();
        }

        public Ingredient GetIngredient(string ingredientName)
        {
            return _ingredientRepository.GetById(ingredientName);
        }

        public void CreateNewIngredient(Ingredient ingredient)
        {
            _ingredientRepository.Create(ingredient);
        }
    }
}
