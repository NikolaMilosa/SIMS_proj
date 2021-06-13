using Model;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class TreatmentService
    {
        private RoomScheduleValidation _roomScheduleValidation;
        private TreatmentValidation _treatmentValidation;
        private PeriodService _periodService;

        public TreatmentService()
        {
            _roomScheduleValidation = new RoomScheduleValidation();
            _treatmentValidation = new TreatmentValidation();
            _periodService = new PeriodService();
        }

        public void SaveTreatment(Period period)
        {
            _roomScheduleValidation.ValidateRoomScheduleAvailability(period.Treatment);
            _treatmentValidation.ValidateTreatment(period);
            _periodService.UpdatePeriodWithoutValidation(period);
        }
    }
}
