using Model;
using Repository.DoctorPersistance;
using Repository.PeriodPersistance;
using Repository.ReferralPersistance;
using System.Collections.Generic;
using ZdravoHospital.GUI.DoctorUI.DTOs;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class PeriodService
    {
        private PeriodRepository _periodRepository;
        private ReferralRepository _referralRepository;
        private PeriodValidation _periodValidation;
        private RoomScheduleValidation _roomScheduleValidation;
        private DoctorRepository _doctorRepository;

        public PeriodService()
        {
            _periodRepository = new PeriodRepository();
            _referralRepository = new ReferralRepository();
            _periodValidation = new PeriodValidation();
            _roomScheduleValidation = new RoomScheduleValidation();
            _doctorRepository = new DoctorRepository();
        }

        public List<Period> GetPeriods()
        {
            return _periodRepository.GetValues();
        }

        public void CreateNewPeriod(Period period, Referral referral)
        {
            Validate(period);
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

        private void Validate(Period period)
        {
            _roomScheduleValidation.ValidateRoomScheduleAvailability(period);
            _periodValidation.ValidatePeriod(period);
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
            Validate(period);
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

        public List<PatientInfoPeriodDisplayDTO> GetPatientInfoPeriodDisplayDTOs(string patientUsername)
        {
            var dtos = new List<PatientInfoPeriodDisplayDTO>();

            foreach (Period period in _periodRepository.GetValues())
                if (period.PatientUsername.Equals(patientUsername))
                    dtos.Add(new PatientInfoPeriodDisplayDTO(period,
                        _doctorRepository.GetById(period.DoctorUsername)));

            return dtos;
        }
    }
}
