using Model;
using Repository.PeriodPersistance;
using Repository.ReferralPersistance;
using System;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class PeriodService
    {
        private PeriodRepository _periodRepository;
        private ReferralRepository _referralRepository;
        private PeriodValidation _periodValidation;

        public PeriodService()
        {
            _periodRepository = new PeriodRepository();
            _referralRepository = new ReferralRepository();
            _periodValidation = new PeriodValidation();
        }

        public void CreateNewPeriod(Period period, Referral referral)
        {
            _periodValidation.ValidatePeriod(period);

            if (referral != null)
            {
                period.ReferringReferralId = referral.ReferralId;
                referral.Period = period;
                referral.IsUsed = true;
                _referralRepository.Update(referral);
            }

            _periodRepository.Create(period);
        }

        public void CancelPeriod(int periodId)
        {
            int referralId = _periodRepository.GetById(periodId).ReferredReferralId;
            _periodRepository.DeleteById(periodId);

            if (referralId != -1)
            {
                Referral referral =_referralRepository.GetById(referralId);
                referral.IsUsed = false;
                _referralRepository.Update(referral);
            }
        }

        public void UpdatePeriod(Period period)
        {
            _periodValidation.ValidatePeriod(period, true);
            _periodRepository.Update(period);
        }

        public void UpdatePeriodWithoutValidation(Period period)
        {
            _periodRepository.Update(period);
        }

    }
}
