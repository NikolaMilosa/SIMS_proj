using System;
using System.Collections.Generic;
using System.Text;
using Repository.CredentialsPersistance;
using Repository.DoctorPersistance;
using Repository.EmployeePersistance;
using Repository.InventoryPersistance;
using Repository.MedicinePersistance;
using Repository.MedicineRecensionPersistance;
using Repository.NotificationsPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.PersonNotificationPersistance;
using Repository.ReferralPersistance;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using Repository.RoomSchedulePersistance;
using Repository.SpecializationPersistance;
using Repository.SurveyPersistance;
using Repository.TransferRequestPersistance;

namespace ZdravoHospital.GUI.ManagerUI.DTOs
{
    public class InjectorDTO
    {
        public ICredentialsRepository CredentialsRepository { get; set; }
        public IDoctorRepository DoctorRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IInventoryRepository InventoryRepository { get; set; }
        public IMedicineRepository MedicineRepository { get; set; }
        public IMedicineRecensionRepository MedicineRecensionRepository { get; set; }
        public INotificationsRepository NotificationsRepository { get; set; }
        public IPatientRepository PatientRepository { get; set; }
        public IPeriodRepository PeriodRepository { get; set; }
        public IPersonNotificationRepository PersonNotificationRepository { get; set; }
        public IReferralRepository ReferralRepository { get; set; }
        public IRoomInventoryRepository RoomInventoryRepository { get; set; }
        public IRoomRepository RoomRepository { get; set; }
        public IRoomScheduleRepository RoomScheduleRepository { get; set; }
        public ISpecializationRepository SpecializationRepository { get; set; }
        public ISurveyRepository SurveyRepository { get; set; }
        public ITransferRequestRepository TransferRepository { get; set; }

    }
}
