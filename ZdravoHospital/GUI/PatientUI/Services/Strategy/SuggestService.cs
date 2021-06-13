namespace ZdravoHospital.GUI.PatientUI.Services.Strategy
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
