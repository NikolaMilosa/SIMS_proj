using Model;
using System.Collections.ObjectModel;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class PrescriptionController
    {
        private PrescriptionService _prescriptionService;
        private PeriodService _periodService;

        public PrescriptionController()
        {
            _prescriptionService = new PrescriptionService();
            _periodService = new PeriodService();
        }

        public void CheckAllergens(Medicine medicine, Patient patient)
        {
            _prescriptionService.CheckAllergens(medicine, patient);
        }

        public void GenerateTherapies(Prescription prescription, ObservableCollection<Therapy> therapies)
        {
            _prescriptionService.GenerateTherapies(prescription, therapies);
        }

        public void SavePrescription(Period period)
        {
            _periodService.UpdatePeriodWithoutValidation(period);
        }

        public ObservableCollection<Therapy> CollectTherapies(Prescription prescription)
        {
            return _prescriptionService.CollectTherapies(prescription);
        }
    }
}
