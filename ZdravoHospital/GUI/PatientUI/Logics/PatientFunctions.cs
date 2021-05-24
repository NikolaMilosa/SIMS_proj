﻿using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Repository;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class PatientFunctions
    {
        private string username;
        private ViewFunctions viewFunctions;
        private PatientRepository patientRepository;

        public PatientFunctions(string username)
        {
            this.username = username;
            viewFunctions = new ViewFunctions();
            patientRepository = new PatientRepository();
        }
        private  Patient LoadPatient()
        {
            return patientRepository.GetById(username);
        }

        public  bool IsTrollDetected()
        {
            bool detected = false;
            Patient patient = LoadPatient();
            if (patient.RecentActions >= 5)
                detected = true;
            
            return detected;
        }

        public bool ActionTaken()
        {
            Patient patient = LoadPatient();
            if (patient.RecentActions == 5)
            {
                viewFunctions.ShowOkDialog("Troll detected!", "Your account has been blocked due to too much recent actions! Please, contact our support!");
                return false;
            }

            ++patient.RecentActions;
            patientRepository.Update(patient);
            return true;
        }
    }
}