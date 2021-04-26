using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Model;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public static class MedicineFunctions
    {
        public static void AddNewMedicine(Medicine newMedicine)
        {
            newMedicine.MedicineName = Regex.Replace(newMedicine.MedicineName, @"\s+", " ");
            newMedicine.MedicineName = newMedicine.MedicineName.Trim().ToLower();
            Model.Resources.medicines.Add(newMedicine);
            ManagerWindow.Medicines.Add(newMedicine);

            Model.Resources.SaveMedicines();
        }

        public static void EditMedicine(Medicine oldMedicine, Medicine newMedicine)
        {
            newMedicine.MedicineName = Regex.Replace(newMedicine.MedicineName, @"\s+", " ");
            newMedicine.MedicineName = newMedicine.MedicineName.Trim().ToLower();

            int index = Model.Resources.medicines.IndexOf(oldMedicine);
            Model.Resources.medicines.Remove(oldMedicine);
            Model.Resources.medicines.Insert(index, newMedicine);

            index = ManagerWindow.Medicines.IndexOf(oldMedicine);
            ManagerWindow.Medicines.Remove(oldMedicine);
            ManagerWindow.Medicines.Insert(index, newMedicine);

            Model.Resources.SaveMedicines();
        }

        public static bool DeleteIngredientFromMedicine(Ingredient ingredient, List<Ingredient> temporarayIngredients, ObservableCollection<Ingredient> viewableIngredients)
        {
            viewableIngredients.Remove(ingredient);
            temporarayIngredients.RemoveAll(i => i.IngredientName.Equals(ingredient.IngredientName));

            return true;
        }

        public static bool DeleteMedicine(Medicine medicine)
        {
            Model.Resources.medicines.Remove(medicine);
            ManagerWindow.Medicines.Remove(medicine);

            Model.Resources.SaveMedicines();

            return true;
        }
    }
}
