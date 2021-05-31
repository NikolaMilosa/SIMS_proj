using Model;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class TreatmentController
    {
        private TreatmentService _treatmentService;

        public TreatmentController()
        {
            _treatmentService = new TreatmentService();
        }

        public void SaveTreatment(Period period)
        {
            _treatmentService.SaveTreatment(period);
        }
    }
}
