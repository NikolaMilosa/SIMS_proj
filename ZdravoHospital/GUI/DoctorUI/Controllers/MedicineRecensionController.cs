using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class MedicineRecensionController
    {
        private MedicineRecensionService _medicineRecensionService;

        public MedicineRecensionController()
        {
            _medicineRecensionService = new MedicineRecensionService();
        }

        public void ApproveMedicine(string medicineName)
        {
            _medicineRecensionService.ApproveMedicine(medicineName);
        }

        public void RejectMedicine(string medicineName, string recensionNote)
        {
            _medicineRecensionService.RejectMedicine(medicineName, recensionNote);
        }

        public void RenameMedicine(string oldName, string newName)
        {
            _medicineRecensionService.RenameMedicine(oldName, newName);
        }
    }
}
