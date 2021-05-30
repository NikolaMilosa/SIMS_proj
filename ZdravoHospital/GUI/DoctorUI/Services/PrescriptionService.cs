using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class PrescriptionService
    {
        private PrescriptionValidation _prescriptionValidation;

        public PrescriptionService()
        {
            _prescriptionValidation = new PrescriptionValidation();
        }

        public void CheckAllergens(Medicine medicine, Patient patient)
        {
            _prescriptionValidation.CheckAllergens(medicine, patient);
        }

        public void GenerateTherapies(Prescription prescription, ObservableCollection<Therapy> therapies)
        {
            prescription.TherapyList = new List<Therapy>();

            foreach (Therapy t in therapies)
                prescription.TherapyList.Add(t);
        }

        public  ObservableCollection<Therapy> CollectTherapies(Prescription prescription)
        {
            var therapies = new ObservableCollection<Therapy>();

            foreach (Therapy therapy in prescription.TherapyList)
                therapies.Add(new Therapy()
                {
                    Medicine = therapy.Medicine,
                    StartHours = therapy.StartHours,
                    TimesPerDay = therapy.TimesPerDay,
                    PauseInDays = therapy.PauseInDays,
                    EndDate = therapy.EndDate,
                    Instructions = therapy.Instructions
                });

            return therapies;
        }
    }
}
