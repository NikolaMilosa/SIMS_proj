using Model;
using Repository.MedicinePersistance;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class MedicineService
    {
        private MedicineRepository _medicineRepository;

        public MedicineService()
        {
            _medicineRepository = new MedicineRepository();
        }

        public List<Medicine> GetMedicines()
        {
            return _medicineRepository.GetValues();
        }

        public List<Medicine> GetApprovedMedicines()
        {
            return _medicineRepository.GetValues().Where(m => m.Status == MedicineStatus.APPROVED).ToList();
        }
    }
}
