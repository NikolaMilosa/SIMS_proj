using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class MedicineController
    {
        private MedicineService _medicineService;

        public MedicineController()
        {
            _medicineService = new MedicineService();
        }

        public List<Medicine> GetMedicines()
        {
            return _medicineService.GetMedicines();
        }

        public List<Medicine> GetApprovedMedicines()
        {
            return _medicineService.GetApprovedMedicines();
        }

        public ObservableCollection<Ingredient> GetAvailableIngredients(Medicine medicine)
        {
            return _medicineService.GetAvailableIngredients(medicine);
        }

        public ObservableCollection<string> GetAvailableReplacements(Medicine medicine)
        {
            return _medicineService.GetAvailableReplacements(medicine);
        }

        public void UpdateMedicine(Medicine medicine)
        {
            _medicineService.UpdateMedicine(medicine);
        }

        public void RenameMedicine(string oldName, string newName)
        {
            _medicineService.RenameMedicine(oldName, newName);
        }
    }
}
