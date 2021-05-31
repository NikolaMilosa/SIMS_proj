using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class IngredientController
    {
        private IngredientService _ingredientService;

        public IngredientController()
        {
            _ingredientService = new IngredientService();
        }

        public Ingredient GetIngredient(string ingredientName)
        {
            return _ingredientService.GetIngredient(ingredientName);
        }

        public void CreateNewIngredient(Ingredient ingredient)
        {
            _ingredientService.CreateNewIngredient(ingredient);
        }
    }
}
