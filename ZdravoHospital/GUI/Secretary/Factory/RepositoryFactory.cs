using Repository.CredentialsPersistance;
using Repository.DoctorPersistance;
using Repository.FeedbackPersistance;
using Repository.MedicinePersistance;
using Repository.NotificationsPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.PersonNotificationPersistance;
using Repository.RoomPersistance;
using Repository.SpecializationPersistance;
using ZdravoHospital.Repository.IngredientPersistance;

namespace ZdravoHospital.GUI.Secretary.Factory
{
    public static class RepositoryFactory
    {
        public static ICredentialsRepository CreateCredentialsRepository()
        {
            return new CredentialsRepository();
        }
        public static IIngredientRepository CreateIngredientRepository()
        {
            return new IngredientRepository();
        }

        public static IMedicineRepository CreateMedicineRepository()
        {
            return new MedicineRepository();
        }

        public static IPatientRepository CreatePatientRepository()
        {
            return new PatientRepository();
        }

        public static IFeedbackRepository CreateFeedbackRepository()
        {
            return new FeedbackRepository();
        }

        public static INotificationsRepository CreateNotificationRepository()
        {
            return new NotificationRepository();
        }

        public static IPersonNotificationRepository CreatePersonNotificationRepository()
        {
            return new PersonNotificationRepository();
        }

        public static IDoctorRepository CreateDoctorRepository()
        {
            return new DoctorRepository();
        }

        public static IRoomRepository CreateRoomRepository()
        {
            return new RoomRepository();
        }

        public static IPeriodRepository CreatePeriodRepository()
        {
            return new PeriodRepository();
        }

        public static ISpecializationRepository CreateSpecializationRepository()
        {
            return new SpecializationRepository();
        }

    }
}
