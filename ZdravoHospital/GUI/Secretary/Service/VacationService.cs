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

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class VacationService
    {
        private IDoctorRepository _doctorRepository;
        private IPeriodRepository _periodRepository;
        public WorkTimeService WorkService;
        public NotificationService NotificationService { get; set; }
        public VacationService(IDoctorRepository doctorRepository, IPeriodRepository periodRepository)
        {
            _doctorRepository = doctorRepository;
            _periodRepository = periodRepository;

            WorkService = new WorkTimeService(doctorRepository);

            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            INotificationsRepository notificationsRepository = RepositoryFactory.CreateNotificationRepository();
            IPersonNotificationRepository personNotificationRepository = RepositoryFactory.CreatePersonNotificationRepository();
            NotificationService = new NotificationService(notificationsRepository, personNotificationRepository, credentialsRepository);
        }

        public bool ProcessVacationCreation(VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            int usedDays = 0;
            foreach(var vacation in selectedDoctor.ShiftRule.Vacations)
            {
                if(vacation.VacationStartTime.Year == DateTime.Today.Year)
                {
                    usedDays += vacation.NumberOfFreeDays;
                }
            }
            if(usedDays + vacationDTO.NumberOfFreeDays > 30)
            {
                return false;
            }

            selectedDoctor.ShiftRule.Vacations.Add(new Vacation(vacationDTO.VacationStartTime, vacationDTO.NumberOfFreeDays));
            _doctorRepository.Update(selectedDoctor);
            DeleteScheduledPeriods(vacationDTO, selectedDoctor);
            return true;
            
        }

        public void ProcessVacationDeletion(Doctor selectedDoctor)
        {
            selectedDoctor.ShiftRule.Vacations.Clear();
            _doctorRepository.Update(selectedDoctor);
        }

        private List<Period> getScheduledPeriodsToCancel(VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            DateTime vacationEnd = vacationDTO.VacationStartTime.AddDays(vacationDTO.NumberOfFreeDays);
            List<Period> scheduledPeriods = new List<Period>();
            foreach(var period in allPeriods)
            {
                if(period.DoctorUsername == selectedDoctor.Username)
                {
                    if (period.StartTime.Date >= vacationDTO.VacationStartTime.Date && period.StartTime.Date <= vacationEnd.Date)
                    {
                        scheduledPeriods.Add(period);
                    }
                }
            }
            return scheduledPeriods;
        }

        public void DeleteScheduledPeriods(VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            List<Period> periods = getScheduledPeriodsToCancel(vacationDTO, selectedDoctor);
            foreach(var period in periods)
            {
                _periodRepository.DeleteById(period.PeriodId);
                sendCancelledNotification(period, period.PatientUsername);
                sendCancelledNotification(period, "suki"); // currently hard coded
            }
        }

        private string createCancelledNotificationText(Period period, string usernameReceiver)
        {
            StringBuilder notificationText = new StringBuilder();
            notificationText.Append("Dear ").Append(usernameReceiver).Append(", your appointment from ")
                .Append(period.StartTime.ToString())
                .Append(" has been cancelled due to doctor's vacation. Please contact us for rescheduling.");

            return notificationText.ToString();
        }
        private void sendCancelledNotification(Period period, string usernameReceiver)
        {
            int notificationId = NotificationService.CalculateNotificationId();
            string notificationText = createCancelledNotificationText(period, usernameReceiver);
            string notificationTitle = "Cancellation";
            Notification newNotification = new Model.Notification(notificationText, DateTime.Now, "suki", notificationTitle, notificationId);
            NotificationService.CreateNewNotification(newNotification);
            PersonNotification personNotification = new PersonNotification(usernameReceiver, notificationId, false);
            NotificationService.CreateNewPersonNotification(personNotification);
        }
    }
}
