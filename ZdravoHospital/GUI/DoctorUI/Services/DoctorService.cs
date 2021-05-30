using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class DoctorService
    {
        private DoctorRepository _doctorRepository;

        public DoctorService()
        {
            _doctorRepository = new DoctorRepository();
        }

        public List<Doctor> GetDoctors()
        {
            return _doctorRepository.GetValues();
        }

        public List<Doctor> GetSpecialists()
        {
            return _doctorRepository.GetValues().Where(d => !d.SpecialistType.SpecializationName.Equals("Doctor")).ToList();
        }

        public List<Doctor> GetOtherDoctors(string doctorUsername)
        {
            List<Doctor> doctors = _doctorRepository.GetValues();
            doctors.RemoveAll(d => d.Username.Equals(doctorUsername));
            return doctors;
        }

        public Doctor GetDoctor(string doctorUsername)
        {
            return _doctorRepository.GetById(doctorUsername);
        }
    }
}
