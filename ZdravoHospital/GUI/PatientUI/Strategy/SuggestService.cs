using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.Strategy
{
    public class SuggestService
    {

        private ISuggestStrategy _suggestService;

        public void Inject(ISuggestStrategy suggestService)
        {
            _suggestService = suggestService;
        }

        public void Suggest()
        {
            _suggestService.Suggest();
        }
    }
}
