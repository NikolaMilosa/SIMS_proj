using Model;
using Repository.ReferralPersistance;
using System;

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

        internal void CreateNewReferral(Referral referral)
        {
            _referralRepository.Create(referral);
        }

        internal void UpdateReferral(Referral referral)
        {
            _referralRepository.Update(referral);
        }
    }
}
