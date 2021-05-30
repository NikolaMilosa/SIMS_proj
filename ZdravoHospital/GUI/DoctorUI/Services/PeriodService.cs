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
            _periodRepository.Create(period);

            if (referral != null)
            {
                referral.PeriodId = period.PeriodId;
                referral.IsUsed = true;
                _referralRepository.Update(referral);

                period.ParentReferralId = referral.ReferralId;
                _periodRepository.Update(period);
            }
        }

        public void CancelPeriod(int periodId)
        {
            int referralId = _periodRepository.GetById(periodId).ParentReferralId;
            _periodRepository.DeleteById(periodId);

            if (referralId != -1)
            {
                Referral referral =_referralRepository.GetById(referralId);
                referral.PeriodId = -1;
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

        public Period GetPeriod(int periodId)
        {
            return _periodRepository.GetById(periodId);
        }
    }
}
