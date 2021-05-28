using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class PeriodController
    {
        private PeriodService _periodService;

        public PeriodController()
        {
            _periodService = new PeriodService();
        }

        public void CreateNewPeriod(Period period, Referral referral)
        {
            _periodService.CreateNewPeriod(period, referral);
        }

        public void UpdatePeriod(Period period)
        {
            _periodService.UpdatePeriod(period);
        }

        public void CancelPeriod(int periodId)
        {
            _periodService.CancelPeriod(periodId);
        }
    }
}
