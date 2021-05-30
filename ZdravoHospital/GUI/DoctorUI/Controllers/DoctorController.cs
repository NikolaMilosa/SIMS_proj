using Model;
using System;
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

        public List<Doctor> GetOtherDoctors(string doctorUsername)
        {
            return _doctorService.GetOtherDoctors(doctorUsername);
        }

        public Doctor GetDoctor(string doctorUsername)
        {
            return _doctorService.GetDoctor(doctorUsername);
        }
    }
}
