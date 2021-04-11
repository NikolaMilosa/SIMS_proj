using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.ViewModel
{
    public class AppointmentView : Period
    {
        //TO DO: ucitaj listu doktora i setuj doktora na ovog
        public Doctor Doctor { get; set; }
    }
}
