using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ZdravoHospital.GUI.DoctorUI.Controllers;
using ZdravoHospital.GUI.DoctorUI.Validations;
using ZdravoHospital.GUI.DoctorUI.Commands;
using System.Windows;
using ZdravoHospital.GUI.DoctorUI.Exceptions;

namespace ZdravoHospital.GUI.DoctorUI.ViewModel
{
    public class NewAppointmentViewModel : ViewModel
    {
        private Referral _referral;
        private PeriodController _periodController;

        public bool DoctorPatientEditable { get; set; }
        public bool IsUrgent { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTimeText { get; set; }
        public string DurationText { get; set; }
        public Room Room { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        private Visibility messagePopUpVisibility;
        public Visibility MessagePopUpVisibility 
        {
            get
            {
                return messagePopUpVisibility;
            }
            set
            {
                messagePopUpVisibility = value;
                OnPropertyChanged("MessagePopUpVisibility");
            }
        }
        private string messageText;
        public string MessageText
        {
            get
            {
                return messageText;
            }
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        public void Executed_ConfirmCommand()
        {
            if (!IsInputValid())
            {
                MessagePopUpVisibility = Visibility.Visible;
                return;
            }

            Period period = FormPeriod();
            
            try
            {
                _periodController.CreateNewPeriod(period, _referral);
                MessageText = "Appointment created successfully.";
                MessagePopUpVisibility = Visibility.Visible;
                //TODO: navigate back
                return;
            }
            catch (PeriodInPastException exception)
            {
                MessageText = "Cannot create appointment in the past.";
            }
            catch (RoomUnavailableException exception)
            {
                MessageText = "Selected room is unavailable in selected period.";
            }
            catch (DoctorUnavailableException exception)
            {
                MessageText = "Selected doctor is unavailable in selected period.";
            }
            catch (PatientUnavailableException exception)
            {
                MessageText = "Selected patient is unavailable in selected period.";
            }

            MessagePopUpVisibility = Visibility.Visible;
        }

        public bool CanExecute_ConfirmCommand()
        {
            return true;
        }
        
        public MyICommand CloseMessagePopUpCommand { get; set; }

        public void Executed_CloseMessagePopUpCommand()
        {
            MessagePopUpVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_CloseMessagePopUpCommand()
        {
            return true;
        }

        #endregion
        
        public NewAppointmentViewModel(Doctor doctor, DateTime startTime, int duration)
        {
            InitializeCommands();

            _periodController = new PeriodController();

            Doctors = new ObservableCollection<Doctor>(new DoctorController().GetDoctors());
            Patients = new ObservableCollection<Patient>(new PatientController().GetPatients());
            Rooms = new ObservableCollection<Room>(new RoomController().GetAppointmentRooms());

            Doctor = Doctors.ToList().Find(d => d.Username.Equals(doctor.Username));
            StartDate = startTime.Date;
            StartTimeText = startTime.ToString("HH:mm");
            DurationText = duration.ToString();

            DoctorPatientEditable = true; // enable combo boxes

            MessagePopUpVisibility = Visibility.Collapsed;
        }
        
        public NewAppointmentViewModel(Referral referral, Patient patient)
        {
            InitializeCommands();

            _periodController = new PeriodController();

            Doctors = new ObservableCollection<Doctor>(new DoctorController().GetDoctors());
            Patients = new ObservableCollection<Patient>(new PatientController().GetPatients());
            Rooms = new ObservableCollection<Room>(new RoomController().GetAppointmentRooms());

            Doctor = Doctors.ToList().Find(d => d.Username.Equals(referral.ReferredDoctorUsername));
            Patient = Patients.ToList().Find(p => p.Username.Equals(patient.Username));
            StartDate = DateTime.Today;
            StartTimeText = "00:00";
            DurationText = "0";

            DoctorPatientEditable = false; // disable combo boxes
        }

        private void InitializeCommands()
        {
            ConfirmCommand = new MyICommand(Executed_ConfirmCommand, CanExecute_ConfirmCommand);
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
        }

        private bool IsInputValid()
        {
            if (Patient == null)
            {
                MessageText = "Please select patient.";
                return false;
            }

            if (!BasicValidation.IsTimeFromTextFormatValid(StartTimeText))
            {
                MessageText = "Please enter start time in correct format (HH:mm).";
                return false;
            }

            if (!BasicValidation.IsTimeFromTextValueValid(StartTimeText))
            {
                MessageText = "Please enter valid start time.";
                return false;
            }

            if (!BasicValidation.IsIntegerFromTextValid(DurationText))
            {
                MessageText = "Please enter duration in correct format (numbers only).";
                return false;
            }

            if (Room == null)
            {
                MessageText = "Please select appointment room.";
                return false;
            }

            return true;
        }

        private Period FormPeriod()
        {
            string[] parts = StartTimeText.Split(':');
            int hours = Int32.Parse(parts[0]);
            int minutes = Int32.Parse(parts[1]);
            DateTime dateTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, hours, minutes, 0);

            Period period = new Period(dateTime, Int32.Parse(DurationText), PeriodType.APPOINTMENT,
                                       Patient.Username, Doctor.Username, Room.Id);
            period.IsUrgent = IsUrgent;

            return period;
        }
    }
}
