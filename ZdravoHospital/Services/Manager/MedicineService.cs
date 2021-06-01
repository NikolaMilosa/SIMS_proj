using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Model;
using Repository.MedicinePersistance;
using Repository.MedicineRecensionPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.Services.Manager
{
    public class MedicineService
    {
        #region Repos

        private IMedicineRepository _medicineRepository;
        private IMedicineRecensionRepository _medicineRecensionRepository;

        #endregion

        #region Event things

        public delegate void MedicineChangedEventHandler(object sender, EventArgs e);

        public event MedicineChangedEventHandler MedicineChanged;

        protected virtual void OnMedicineChanged()
        {
            if (MedicineChanged != null)
            {
                MedicineChanged(this, EventArgs.Empty);
            }
        }

        public delegate void IngredientChangedEventHandler(object sender, EventArgs e);

        public event IngredientChangedEventHandler IngredientChanged;

        protected virtual void OnIngredientChanged()
        {
            if (IngredientChanged != null)
            {
                IngredientChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        public MedicineService(AddOrEditMedicineDialogViewModel activeDialog, InjectorDTO injector)
        {
            MedicineChanged += ManagerWindowViewModel.GetDashboard().OnMedicineChanged;
            if (activeDialog != null)
            {
                IngredientChanged += activeDialog.OnIngredientChanged;
            }
            _medicineRecensionRepository = injector.MedicineRecensionRepository;
            _medicineRepository = injector.MedicineRepository;
        }

        public void AddNewMedicine(Medicine newMedicine)
        {
            newMedicine.MedicineName = Regex.Replace(newMedicine.MedicineName, @"\s+", " ");
            newMedicine.MedicineName = newMedicine.MedicineName.Trim().ToLower();

            newMedicine.Supplier = Regex.Replace(newMedicine.Supplier, @"\s+", " ");
            newMedicine.Supplier = newMedicine.Supplier.Trim();
            newMedicine.Supplier = newMedicine.Supplier.Substring(0, 1).ToUpper() + newMedicine.Supplier.Substring(1).ToLower();

            _medicineRepository.Create(newMedicine);

            OnMedicineChanged();
        }

        public void EditMedicine(Medicine newMedicine)
        {
            newMedicine.MedicineName = Regex.Replace(newMedicine.MedicineName, @"\s+", " ");
            newMedicine.MedicineName = newMedicine.MedicineName.Trim().ToLower();

            newMedicine.Supplier = Regex.Replace(newMedicine.Supplier, @"\s+", " ");
            newMedicine.Supplier = newMedicine.Supplier.Trim();
            newMedicine.Supplier = newMedicine.Supplier.Substring(0, 1).ToUpper() + newMedicine.Supplier.Substring(1).ToLower();

            newMedicine.Status = MedicineStatus.STAGED;

            _medicineRepository.Update(newMedicine);

            OnMedicineChanged();
        }

        public bool DeleteIngredientFromMedicine(Ingredient ingredient, Medicine medicine)
        {
            medicine.Ingredients.Remove(ingredient);
            OnIngredientChanged();
            return true;
        }

        public bool DeleteMedicine(Medicine medicine)
        {
            _medicineRepository.DeleteById(medicine.MedicineName);

            _medicineRecensionRepository.DeleteById(medicine.MedicineName);

            OnMedicineChanged();

            return true;
        }

        public void AddIngredientToMedicine(Ingredient ingredient, Medicine medicine)
        {
            ingredient.IngredientName = Regex.Replace(ingredient.IngredientName, @"\s+", " ");
            medicine.Ingredients.Add(ingredient);
            OnIngredientChanged();
        }

        public void EditIngredientInMedicine(Ingredient oldIngredient, Ingredient newIngedient, Medicine medicine)
        {
            var index = medicine.Ingredients.IndexOf(oldIngredient);
            medicine.Ingredients.Remove(oldIngredient);

            newIngedient.IngredientName = Regex.Replace(newIngedient.IngredientName, @"\s+", " ");

            medicine.Ingredients.Insert(index, newIngedient);

            OnIngredientChanged();
        }

        public void SendMedicineOnRecension(Medicine medicine, Doctor doctor)
        {
            var medicineRecension = new MedicineRecension()
            {
                DoctorUsername = doctor.Username,
                MedicineName = medicine.MedicineName,
                RecensionNote = ""
            };

            _medicineRecensionRepository.DeleteById(medicine.MedicineName);
            _medicineRecensionRepository.Create(medicineRecension);

            medicine.Status = MedicineStatus.PENDING;

            _medicineRepository.Update(medicine);

            OnMedicineChanged();
        }

        public MedicineRecension FindMedicineRecension(Medicine medicine)
        {
            return _medicineRecensionRepository.GetById(medicine.MedicineName);
        }
    }
}
