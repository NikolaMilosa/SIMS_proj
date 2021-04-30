using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Model;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class MedicineFunctions
    {
        private static Mutex _medicineMutex;

        public static Mutex GetMedicineMutex()
        {
            if (_medicineMutex == null)
                _medicineMutex = new Mutex();
            return _medicineMutex;
        }

        public MedicineFunctions() { }

        public void AddNewMedicine(Medicine newMedicine)
        {
            newMedicine.MedicineName = Regex.Replace(newMedicine.MedicineName, @"\s+", " ");
            newMedicine.MedicineName = newMedicine.MedicineName.Trim().ToLower();

            newMedicine.Supplier = Regex.Replace(newMedicine.Supplier, @"\s+", " ");
            newMedicine.Supplier = newMedicine.Supplier.Trim();
            newMedicine.Supplier = newMedicine.Supplier.Substring(0, 1).ToUpper() + newMedicine.Supplier.Substring(1).ToLower();

            GetMedicineMutex().WaitOne();

            Model.Resources.medicines.Add(newMedicine);
            ManagerWindow.Medicines.Add(newMedicine);

            Model.Resources.SaveMedicines();

            GetMedicineMutex().ReleaseMutex();
        }

        public void EditMedicine(Medicine oldMedicine, Medicine newMedicine)
        {
            newMedicine.MedicineName = Regex.Replace(newMedicine.MedicineName, @"\s+", " ");
            newMedicine.MedicineName = newMedicine.MedicineName.Trim().ToLower();

            newMedicine.Supplier = Regex.Replace(newMedicine.Supplier, @"\s+", " ");
            newMedicine.Supplier = newMedicine.Supplier.Trim();
            newMedicine.Supplier = newMedicine.Supplier.Substring(0, 1).ToUpper() + newMedicine.Supplier.Substring(1).ToLower();

            GetMedicineMutex().WaitOne();

            var index = Model.Resources.medicines.IndexOf(oldMedicine);
            Model.Resources.medicines.Remove(oldMedicine);
            Model.Resources.medicines.Insert(index, newMedicine);

            index = ManagerWindow.Medicines.IndexOf(oldMedicine);
            ManagerWindow.Medicines.Remove(oldMedicine);
            ManagerWindow.Medicines.Insert(index, newMedicine);

            Model.Resources.SaveMedicines();

            GetMedicineMutex().ReleaseMutex();
        }

        public bool DeleteIngredientFromMedicine(Ingredient ingredient, List<Ingredient> temporarayIngredients, ObservableCollection<Ingredient> viewableIngredients)
        {
            viewableIngredients.Remove(ingredient);
            temporarayIngredients.RemoveAll(i => i.IngredientName.Equals(ingredient.IngredientName));

            return true;
        }

        public bool DeleteMedicine(Medicine medicine)
        {
            GetMedicineMutex().WaitOne();

            Model.Resources.medicines.Remove(medicine);
            ManagerWindow.Medicines.Remove(medicine);

            Model.Resources.SaveMedicines();

            GetMedicineMutex().ReleaseMutex();

            return true;
        }

        public void AddIngredientToMedicine(Ingredient ingredient, List<Ingredient> existingIngredients, ObservableCollection<Ingredient> viewableIngredients)
        {
            ingredient.IngredientName = Regex.Replace(ingredient.IngredientName, @"\s+", " ");
            existingIngredients.Add(ingredient);
            viewableIngredients.Add(ingredient);
        }

        public void EditIngredientInMedicine(Ingredient ingredient, string newName, List<Ingredient> existingIngredients, ObservableCollection<Ingredient> viewableIngredients)
        {
            var indexView = viewableIngredients.IndexOf(ingredient);
            viewableIngredients.Remove(ingredient);

            var indexExisting = existingIngredients.FindIndex(i => i.IngredientName.Equals(ingredient.IngredientName));
            existingIngredients.RemoveAt(indexExisting);

            ingredient.IngredientName = Regex.Replace(newName, @"\s+", " ");

            viewableIngredients.Insert(indexView, ingredient);
            existingIngredients.Insert(indexExisting, ingredient);
        }
    }
}
