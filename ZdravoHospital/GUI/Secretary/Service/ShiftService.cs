using Model;
using Repository.CredentialsPersistance;
using Repository.DoctorPersistance;
using Repository.NotificationsPersistance;
using Repository.PeriodPersistance;
using Repository.PersonNotificationPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class ShiftService
    {
        private IDoctorRepository _doctorRepository;
        private IPeriodRepository _periodRepository;
        public NotificationService NotificationService { get; set; }
        public ShiftService(IDoctorRepository doctorRepository, IPeriodRepository periodRepository)
        {
            _doctorRepository = doctorRepository;
            _periodRepository = periodRepository;

            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            INotificationsRepository notificationsRepository = RepositoryFactory.CreateNotificationRepository();
            IPersonNotificationRepository personNotificationRepository = RepositoryFactory.CreatePersonNotificationRepository();
            NotificationService = new NotificationService(notificationsRepository, personNotificationRepository, credentialsRepository);
        }

        public void ProcessShiftCreation(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            if (shiftDTO.IsSingleDayShift)
            {
                saveSingleDayShift(selectedDoctor, shiftDTO);
                DeleteScheduledPeriodsSingleDayShift(shiftDTO, selectedDoctor);
            }
                
            else
                saveRegularShift(selectedDoctor, shiftDTO);
        }

        private void saveSingleDayShift(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            //selectedDoctor.ShiftRule.SingleDayShifts.Add(new DoctorsShift(shiftDTO.ScheduledShift, shiftDTO.ShiftStart, shiftDTO.IsSingleDayShift));
            //_doctorRepository.Update(selectedDoctor);
            addSingleDayShift(selectedDoctor, shiftDTO);
        }

        private void addSingleDayShift(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            foreach(var shift in selectedDoctor.ShiftRule.SingleDayShifts)
            {
                if(shift.ShiftStart.Date == shiftDTO.ShiftStart.Date)
                {
                    shift.ScheduledShift = shiftDTO.ScheduledShift;
                    _doctorRepository.Update(selectedDoctor);
                    return;
                }
            }
            selectedDoctor.ShiftRule.SingleDayShifts.Add(new DoctorsShift(shiftDTO.ScheduledShift, shiftDTO.ShiftStart, shiftDTO.IsSingleDayShift));
            _doctorRepository.Update(selectedDoctor);
        }

        private void saveRegularShift(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            selectedDoctor.ShiftRule.RegularShift = new DoctorsShift(shiftDTO.ScheduledShift, shiftDTO.ShiftStart, shiftDTO.IsSingleDayShift);
            _doctorRepository.Update(selectedDoctor);
            DeleteScheduledPeriodsRegularShift(shiftDTO, selectedDoctor);
        }

        private List<Period> getScheduledPeriodsToCancelRegularShift(ShiftDTO shiftDTO, Doctor selectedDoctor)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            //DateTime vacationEnd = vacationDTO.VacationStartTime.AddDays(vacationDTO.NumberOfFreeDays);
            List<Period> scheduledPeriods = new List<Period>();
            foreach (var period in allPeriods)
            {
                if (period.DoctorUsername == selectedDoctor.Username)
                {
                    if (period.StartTime.Date >= shiftDTO.ShiftStart.Date)
                    {
                        scheduledPeriods.Add(period);
                    }
                }
            }
            return scheduledPeriods;
        }

        private List<Period> getScheduledPeriodsToCancelSingleDayShift(ShiftDTO shiftDTO, Doctor selectedDoctor)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            //DateTime vacationEnd = vacationDTO.VacationStartTime.AddDays(vacationDTO.NumberOfFreeDays);
            List<Period> scheduledPeriods = new List<Period>();
            foreach (var period in allPeriods)
            {
                if (period.DoctorUsername == selectedDoctor.Username)
                {
                    if (shiftDTO.IsSingleDayShift && period.StartTime.Date == shiftDTO.ShiftStart.Date)
                    {
                        scheduledPeriods.Add(period);
                    }
                }
            }
            return scheduledPeriods;
        }

        public void DeleteScheduledPeriodsRegularShift(ShiftDTO shiftDTO, Doctor selectedDoctor)
        {
            List<Period> periods = getScheduledPeriodsToCancelRegularShift(shiftDTO, selectedDoctor);
            foreach (var period in periods)
            {
                _periodRepository.DeleteById(period.PeriodId);
                sendCancelledNotification(period, period.PatientUsername);
                sendCancelledNotification(period, SecretaryWindowVM.SecretaryUsername);
            }
        }

        public void DeleteScheduledPeriodsSingleDayShift(ShiftDTO shiftDTO, Doctor selectedDoctor)
        {
            List<Period> periods = getScheduledPeriodsToCancelSingleDayShift(shiftDTO, selectedDoctor);
            foreach (var period in periods)
            {
                _periodRepository.DeleteById(period.PeriodId);
                sendCancelledNotification(period, period.PatientUsername);
                sendCancelledNotification(period, SecretaryWindowVM.SecretaryUsername);
            }
        }

        private string createCancelledNotificationText(Period period, string usernameReceiver)
        {
            StringBuilder notificationText = new StringBuilder();
            notificationText.Append("Dear ").Append(usernameReceiver).Append(", your appointment from ")
                .Append(period.StartTime.ToString())
                .Append(" has been cancelled due to doctor's shift change. Please contact us for rescheduling.");

            return notificationText.ToString();
        }
        private void sendCancelledNotification(Period period, string usernameReceiver)
        {
            int notificationId = NotificationService.CalculateNotificationId();
            string notificationText = createCancelledNotificationText(period, usernameReceiver);
            string notificationTitle = "Cancellation";
            Notification newNotification = new Model.Notification(notificationText, DateTime.Now, SecretaryWindowVM.SecretaryUsername, notificationTitle, notificationId);
            NotificationService.CreateNewNotification(newNotification);
            PersonNotification personNotification = new PersonNotification(usernameReceiver, notificationId, false);
            NotificationService.CreateNewPersonNotification(personNotification);
        }

    }
}
