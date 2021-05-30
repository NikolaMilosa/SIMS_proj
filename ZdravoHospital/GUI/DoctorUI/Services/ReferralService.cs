using Model;
using Repository.ReferralPersistance;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class ReferralService
    {
        private ReferralRepository _referralRepository;

        public ReferralService()
        {
            _referralRepository = new ReferralRepository();
        }

        public Referral GetReferral(int referralId)
        {
            return _referralRepository.GetById(referralId);
        }

        public void CreateNewReferral(Referral referral)
        {
            _referralRepository.Create(referral);
        }

        public void UpdateReferral(Referral referral)
        {
            _referralRepository.Update(referral);
        }
    }
}
