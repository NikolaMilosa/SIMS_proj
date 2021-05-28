﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.DTOs;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class InjectFunctions
    {
        public void FillObservableDoctorCollection(ObservableCollection<DoctorDTO> Doctors)
        {
            DoctorRepository doctorRepository = new DoctorRepository();
            List<Doctor> doctors = doctorRepository.GetValues();
            foreach (var doctor in doctors.Where(doctor => doctor.SpecialistType.SpecializationName.Equals("Doctor")))
                Doctors.Add(new DoctorDTO(doctor));
            
        }

        public void FillDoctorCollection(List<DoctorDTO> Doctors)
        {
            DoctorRepository doctorRepository = new DoctorRepository();
            List<Doctor> doctors = doctorRepository.GetValues();
            Doctors.AddRange(from doctor in doctors where doctor.SpecialistType.SpecializationName.Equals("Doctor") select new DoctorDTO(doctor));
        }

        public  void GenerateTimeSpan(List<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
        }

        public  void GenerateObesrvableTimes(ObservableCollection<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
        }
    }
}
