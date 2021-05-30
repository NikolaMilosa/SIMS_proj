using Model;
using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}
