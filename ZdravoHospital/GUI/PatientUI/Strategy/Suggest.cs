using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.Strategy
{
    public class Suggest
    {
        public ObservableCollection<PeriodDTO> SuggestedPeriods { get; private set; }
        public InjectService Injection { get; private set; }
        public PeriodConverter PeriodConverter { get; private set; }
        public PeriodService PeriodService { get; set; }
        public RoomSheduleService RoomScheduleService { get; private set; }

        public Suggest(ObservableCollection<PeriodDTO> suggestedPeriods)
        {
            SuggestedPeriods = suggestedPeriods;
            Injection = new InjectService();
            PeriodConverter = new PeriodConverter();
            PeriodService = new PeriodService();
            RoomScheduleService = new RoomSheduleService();
        }
    }
}
