using Model;
using Repository.CredentialsPersistance;
using Repository.DoctorPersistance;
using Repository.NotificationsPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.PersonNotificationPersistance;
using Repository.RoomPersistance;
using Repository.SpecializationPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class PeriodsToMoveService
    {
        private IPeriodRepository _periodRepository;
        private IPatientRepository _patientRepository;
        private IDoctorRepository _doctorRepository;
        private IRoomRepository _roomRepository;
        public NotificationService NotificationService { get; set; }
        public PeriodsToMoveService()
        {
            _periodRepository = new PeriodRepository();
            _patientRepository = new PatientRepository();
            _doctorRepository = new DoctorRepository();
            _roomRepository = new RoomRepository();

            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            INotificationsRepository notificationsRepository = RepositoryFactory.CreateNotificationRepository();
            IPersonNotificationRepository personNotificationRepository = RepositoryFactory.CreatePersonNotificationRepository();
            NotificationService = new NotificationService(notificationsRepository, personNotificationRepository, credentialsRepository);
        }
        public List<Period> GetPeriods()
        {
            return _periodRepository.GetValues();
        }
        public List<Room> GetRooms()
        {
            return _roomRepository.GetValues();
        }

        private List<Room> findAvailableEmergencyRooms(Period newPeriod)
        {
            List<Room> allRooms = GetRooms();
            List<Room> availableRooms = new List<Room>();
            foreach (Room room in allRooms)
            {
                if (room.RoomType == RoomType.EMERGENCY_ROOM)
                {
                    bool available = true;
                    List<Period> allPeriods = GetPeriods();
                    foreach (Period existingPeriod in allPeriods)
                    {
                        if (periodsOverlap(newPeriod, existingPeriod))
                        {
                            if (room.Id == existingPeriod.RoomId)
                                available = false;
                        }
                    }
                    if (available)
                        availableRooms.Add(room);
                }

            }
            return availableRooms;
        }

        private bool periodsOverlap(Period newPeriod, Period existingPeriod)
        {
            DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);
            DateTime newPeriodEndtime = newPeriod.StartTime.AddMinutes(newPeriod.Duration);
            if (newPeriod.StartTime < existingPeriodEndTime && newPeriodEndtime > existingPeriod.StartTime)
            {
                return true;
            }
            return false;
        }

        private string createPostponeNotificationText(MovePeriod movePeriod, string usernameReceiver)
        {
            StringBuilder notificationText = new StringBuilder();
            notificationText.Append("Dear ").Append(usernameReceiver).Append(", your appointment has been postponed from ")
                .Append(movePeriod.InitialStartTime.ToString()).Append(" to ").Append(movePeriod.MovedStartTime.ToString())
                .Append(" due to urgent appointment. If you are dissatisfied with new appointment, please contact us for rescheduling.");

            return notificationText.ToString();
        }
        private void sendPostponeNotification(MovePeriod movePeriod, string usernameReceiver)
        {
            int notificationId = NotificationService.CalculateNotificationId();
            string notificationText = createPostponeNotificationText(movePeriod, usernameReceiver);
            string notificationTitle = "Reschedule";
            Notification newNotification = new Model.Notification(notificationText, DateTime.Now, SecretaryWindowVM.SecretaryUsername, notificationTitle, notificationId);
            NotificationService.CreateNewNotification(newNotification);
            PersonNotification personNotification = new PersonNotification(usernameReceiver, notificationId, false);
            NotificationService.CreateNewPersonNotification(personNotification);
        }

        public void ProcessMovePeriodSubmit(Period selectedPeriod)
        {
            List<Period> allPeriods = GetPeriods();
            foreach (var movePeriod in selectedPeriod.MovePeriods)
            {
                foreach (var period in allPeriods)
                {
                    if (movePeriod.InitialStartTime == period.StartTime && movePeriod.RoomId == period.RoomId)
                    {
                        period.StartTime = movePeriod.MovedStartTime;
                        _periodRepository.Update(period);
                        sendPostponeNotification(movePeriod, movePeriod.PatientUsername);
                        sendPostponeNotification(movePeriod, movePeriod.DoctorUsername);
                    }
                }
            }
            List<Room> availableEmergencyRooms = findAvailableEmergencyRooms(selectedPeriod);
            if (availableEmergencyRooms.Count != 0)
                selectedPeriod.RoomId = availableEmergencyRooms[0].Id;
            _periodRepository.Create(selectedPeriod);
        }

        public ObservableCollection<Period> GetSortedPeriods(ObservableCollection<Period> periods)
        {
            return new ObservableCollection<Period>(periods.OrderBy(x => x.MovePeriods.Count).ThenBy(x => findSumOfMovePeriods(x)).ToList<Period>());
        }

        public Patient GetPatientById(string id)
        {
            return _patientRepository.GetById(id);
        }

        public Doctor GetDoctorById(string id)
        {
            return _doctorRepository.GetById(id);
        }

        public int findSumOfMovePeriods(Period p)
        {
            int ret = 0;
            foreach (var movePeriod in p.MovePeriods)
            {
                ret += (int)movePeriod.MovedStartTime.Subtract(movePeriod.InitialStartTime).TotalMinutes;
            }
            return ret;
        }
    }
}
