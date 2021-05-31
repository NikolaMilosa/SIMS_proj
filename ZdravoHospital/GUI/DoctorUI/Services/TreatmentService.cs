using Model;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class TreatmentService
    {
        private TreatmentValidation _treatmentValidation;
        private PeriodService _periodService;

        public TreatmentService()
        {
            _treatmentValidation = new TreatmentValidation();
            _periodService = new PeriodService();
        }

        public void SaveTreatment(Period period)
        {
            _treatmentValidation.ValidateTreatment(period);
            _periodService.UpdatePeriodWithoutValidation(period);
        }
    }
}
