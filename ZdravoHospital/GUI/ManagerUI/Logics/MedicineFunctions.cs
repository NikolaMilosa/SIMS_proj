using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Input;
using Model;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

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

        public MedicineFunctions(AddOrEditMedicineDialogViewModel activeDialog)
        {
            MedicineChanged += ManagerWindowViewModel.GetDashboard().OnMedicineChanged;
            if (activeDialog != null)
                IngredientChanged += activeDialog.OnIngredientChanged;
        }

        public void AddNewMedicine(Medicine newMedicine)
        {
            newMedicine.MedicineName = Regex.Replace(newMedicine.MedicineName, @"\s+", " ");
            newMedicine.MedicineName = newMedicine.MedicineName.Trim().ToLower();

            newMedicine.Supplier = Regex.Replace(newMedicine.Supplier, @"\s+", " ");
            newMedicine.Supplier = newMedicine.Supplier.Trim();
            newMedicine.Supplier = newMedicine.Supplier.Substring(0, 1).ToUpper() + newMedicine.Supplier.Substring(1).ToLower();

            GetMedicineMutex().WaitOne();

            Model.Resources.medicines.Add(newMedicine);

            Model.Resources.SaveMedicines();

            GetMedicineMutex().ReleaseMutex();

            OnMedicineChanged();
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

            Model.Resources.SaveMedicines();

            GetMedicineMutex().ReleaseMutex();
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
            GetMedicineMutex().WaitOne();

            Model.Resources.medicines.Remove(medicine);

            Model.Resources.SaveMedicines();

            if (Model.Resources.medicineRecensions.Remove(FindMedicineRecension(medicine)))
            {
                Model.Resources.SaveMedicineRecensions();
            }

            GetMedicineMutex().ReleaseMutex();

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
            
            medicine.Ingredients.Insert(index,newIngedient);
            Resources.SaveMedicines();

            OnIngredientChanged();
        }

        public void SendMedicineOnRecension(Medicine medicine, Doctor doctor)
        {
            GetMedicineMutex().WaitOne();
            
            var medicineRecension = new MedicineRecension(){DoctorUsername = doctor.Username, MedicineName = medicine.MedicineName, RecensionNote = ""};


            Model.Resources.medicineRecensions.RemoveAll(mr => mr.MedicineName.Equals(medicine.MedicineName));
            Model.Resources.medicineRecensions.Add(medicineRecension);
            Model.Resources.SaveMedicineRecensions();
            
            medicine.Status = MedicineStatus.PENDING;

            Model.Resources.SaveMedicines();

            GetMedicineMutex().ReleaseMutex();

            OnMedicineChanged();
        }

        public MedicineRecension FindMedicineRecension(Medicine medicine)
        {
            return Model.Resources.medicineRecensions.Find(m => m.MedicineName.Equals(medicine.MedicineName));
        }
    }
}
