using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Repository.DoctorPersistance;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class DoctorFunctions
    {
        public DoctorRepository DoctorRepository { get; private set; }

        public DoctorFunctions()
        {
            DoctorRepository = new DoctorRepository();
        }

        public List<Doctor> GetGeneralDoctors()
        {
            var doctors = DoctorRepository.GetValues();
            return doctors.Where(doctor => doctor.SpecialistType.SpecializationName.Equals("Doctor")).ToList();
        }

        public List<Doctor> GetAllDoctors()
        {
            return DoctorRepository.GetValues();
        }

        public Doctor GetDoctor(string username)
        {
            var doctors = DoctorRepository.GetValues();
            return doctors.FirstOrDefault(doctor => doctor.Username.Equals(username));
        }

        public bool IsTimeInDoctorsShift(DateTime time,string username)
        {
            Doctor doctor = GetDoctor(username);
            WorkTimeService timeService = new WorkTimeService();
            Shift shift = timeService.GetDoctorShiftByDate(doctor, time);
            return IsTimeInShift(shift, time);
        }

        private bool IsTimeInShift(Shift shift,DateTime time)
        {
            switch (shift)
            {
                case Shift.FIRST:
                    if (time.Hour >= 6 && time.Hour <= 14)
                        return true;
                    break;
                case Shift.SECOND:
                    if (time.Hour >= 14 && time.Hour <= 22)
                        return true;
                    break;
                case Shift.THIRD:
                    if (time.Hour >= 22 && time.Hour <= 6)
                        return true;
                    break;
            }
            return false;
        }



        public string GetDoctorUsername(string name, string surname)
        {
            return (from doctor in DoctorRepository.GetValues() where doctor.Name.Equals(name) && doctor.Surname.Equals(surname) select doctor.Username).FirstOrDefault();
        }

    }
}
