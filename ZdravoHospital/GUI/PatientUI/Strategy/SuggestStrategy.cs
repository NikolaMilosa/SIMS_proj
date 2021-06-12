using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.Strategy
{
    public interface ISuggestStrategy
    {
        public void Suggest();
    }
}
