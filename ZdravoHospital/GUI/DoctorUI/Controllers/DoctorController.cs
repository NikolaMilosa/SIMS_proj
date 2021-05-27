using Model;
using System.Collections.Generic;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class DoctorController
    {
        private DoctorService _doctorService;

        public DoctorController()
        {
            _doctorService = new DoctorService();
        }

        public List<Doctor> GetDoctors()
        {
            return _doctorService.GetDoctors();
        }

        public List<Doctor> GetSpecialists()
        {
            return _doctorService.GetSpecialists();
        }
    }
}
