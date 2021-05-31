using Model;
using Repository.MedicinePersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZdravoHospital.Repository.IngredientPersistance;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class MedicineService
    {
        private MedicineRepository _medicineRepository;
        private IngredientRepository _ingredientRepository;

        public MedicineService()
        {
            _medicineRepository = new MedicineRepository();
            _ingredientRepository = new IngredientRepository();
        }

        public List<Medicine> GetMedicines()
        {
            return _medicineRepository.GetValues();
        }

        public List<Medicine> GetApprovedMedicines()
        {
            return _medicineRepository.GetValues().Where(m => m.Status == MedicineStatus.APPROVED).ToList();
        }

        public ObservableCollection<Ingredient> GetAvailableIngredients(Medicine medicine)
        {
            var availableIngredients = new ObservableCollection<Ingredient>();

            foreach (Ingredient ingredient in _ingredientRepository.GetValues())
                if (medicine.Ingredients.Find(ing => ing.IngredientName.Equals(ingredient.IngredientName)) == null)
                    availableIngredients.Add(ingredient);

            return availableIngredients;
        }

        public ObservableCollection<string> GetAvailableReplacements(Medicine medicine)
        {
            var availableReplacements = new ObservableCollection<string>();

            foreach (Medicine m in _medicineRepository.GetValues())
                if (!m.MedicineName.Equals(medicine.MedicineName) && medicine.Replacements.Find(medicineName => medicineName.Equals(m.MedicineName)) == null)
                    availableReplacements.Add(m.MedicineName);

            return availableReplacements;
        }

        public void UpdateMedicine(Medicine medicine)
        {
            _medicineRepository.Update(medicine);
        }

        public void RenameMedicine(string oldName, string newName)
        {
            _medicineRepository.RenameMedicine(oldName, newName);
        }
    }
}
