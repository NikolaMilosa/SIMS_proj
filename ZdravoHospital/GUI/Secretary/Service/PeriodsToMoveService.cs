using Model;
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using Repository.SpecializationPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class PeriodsToMoveService
    {
        private IPeriodRepository _periodRepository;
        private ISpecializationRepository _specializationRepository;
        private IPatientRepository _patientRepository;
        private IDoctorRepository _doctorRepository;
        private IRoomRepository _roomRepository;
        public NotificationService NotificationService { get; set; }
        public PeriodsToMoveService()
        {
            _periodRepository = new PeriodRepository();
            _specializationRepository = new SpecializationRepository();
            _patientRepository = new PatientRepository();
            _doctorRepository = new DoctorRepository();
            _roomRepository = new RoomRepository();
            NotificationService = new NotificationService();
        }
        public List<Period> GetPeriods()
        {
            return _periodRepository.GetValues();
        }
        public List<Room> GetRooms()
        {
            return _roomRepository.GetValues();
        }

        private List<Model.Room> findAvailableEmergencyRooms(Period newPeriod)
        {
            List<Room> allRooms = GetRooms();
            List<Model.Room> availableRooms = new List<Room>();
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
            Model.Resources.OpenPersonNotifications();
            Model.Resources.OpenNotifications();

            int notificationId = NotificationService.CalculateNotificationId();
            string notificationText = createPostponeNotificationText(movePeriod, usernameReceiver);
            string notificationTitle = "Rescheduling due to urgent appointment";
            Model.Notification newNotification = new Model.Notification(notificationText, DateTime.Now, SecretaryWindow.SecretaryUsername, notificationTitle, notificationId);
            Model.Resources.notifications.Add(newNotification);
            Model.Resources.SaveNotifications();

            Model.PersonNotification personNotification = new Model.PersonNotification(usernameReceiver, notificationId, false);
            Model.Resources.personNotifications.Add(personNotification);

            Model.Resources.SavePersonNotifications();
        }

    }
}
