using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Repository.DoctorPersistance;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class DoctorFunctions
    {
        private DoctorRepository doctorRepository;

        public DoctorFunctions()
        {
            doctorRepository = new DoctorRepository();
        }

        public List<Doctor> GetGeneralDoctors()
        {
            var doctors = doctorRepository.GetValues();
            return doctors.Where(doctor => doctor.SpecialistType.SpecializationName.Equals("Doctor")).ToList();
        }

        public List<Doctor> GetAllDoctors()
        {
            return doctorRepository.GetValues();
        }

        public Doctor GetDoctor(string username)
        {
            var doctors = doctorRepository.GetValues();
            return doctors.FirstOrDefault(doctor => doctor.Username.Equals(username));
        }

        public string GetDoctorUsername(string name, string surname)
        {
           DoctorRepository doctorRepository =new DoctorRepository();
            return (from doctor in doctorRepository.GetValues() where doctor.Name.Equals(name) && doctor.Surname.Equals(surname) select doctor.Username).FirstOrDefault();
        }

    }
}
