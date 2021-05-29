using Model;
using System;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class ReferralController
    {
        private ReferralService _referralService;

        public ReferralController()
        {
            _referralService = new ReferralService();
        }

        public Referral GetReferral(int referralId)
        {
            return _referralService.GetReferral(referralId);
        }

        public void CreateNewReferral(Referral referral)
        {
            _referralService.CreateNewReferral(referral);
        }

        public void UpdateReferral(Referral referral)
        {
            _referralService.UpdateReferral(referral);
        }
    }
}
