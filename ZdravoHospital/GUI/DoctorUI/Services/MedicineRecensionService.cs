using Model;
using Repository.MedicineRecensionPersistance;
using System;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class MedicineRecensionService
    {
        private MedicineRecensionRepository _medicineRecensionRepository;

        public MedicineRecensionService()
        {
            _medicineRecensionRepository = new MedicineRecensionRepository();
        }

        public void ApproveMedicine(string medicineName)
        {
            MedicineRecension medicineRecension = _medicineRecensionRepository.GetById(medicineName);
            medicineRecension.RecensionNote = "";
            _medicineRecensionRepository.Update(medicineRecension);
        }

        public void RejectMedicine(string medicineName, string recensionNote)
        {
            MedicineRecension medicineRecension = _medicineRecensionRepository.GetById(medicineName);
            medicineRecension.RecensionNote = recensionNote;
            _medicineRecensionRepository.Update(medicineRecension);
        }

        public void RenameMedicine(string oldName, string newName)
        {
            _medicineRecensionRepository.RenameMedicine(oldName, newName);
        }
    }
}
