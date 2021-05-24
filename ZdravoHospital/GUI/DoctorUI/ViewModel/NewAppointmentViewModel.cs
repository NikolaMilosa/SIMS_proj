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
        
        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        public void Executed_ConfirmCommand()
        {
            if (!IsInputValid())
                return;

            Period period = FormPeriod();
            
            try
            {
                _periodController.CreateNewPeriod(period, _referral);
                MessageBox.Show("Appointment created successfully.", "Success");
                //TODO: navigate back
            }
            catch (PeriodInPastException exception)
            {
                MessageBox.Show("Cannot create appointment in the past.", "Invalid date and time");
            }
            catch (RoomUnavailableException exception)
            {
                MessageBox.Show("Selected room is unavailable in selected period.", "Room unavailable");
            }
            catch (DoctorUnavailableException exception)
            {
                MessageBox.Show("Selected doctor is unavailable in selected period.", "Doctor unavailable");
            }
            catch (PatientUnavailableException exception)
            {
                MessageBox.Show("Selected patient is unavailable in selected period.", "Patient unavailable");
            }
        }

        public bool CanExecute_ConfirmCommand()
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
        }

        private bool IsInputValid()
        {
            if (Patient == null)
            {
                MessageBox.Show("Please select patient.", "Invalid input");
                return false;
            }

            if (!BasicValidation.IsTimeFromTextFormatValid(StartTimeText))
            {
                MessageBox.Show("Please enter start time in correct format (HH:mm).", "Invalid input");
                return false;
            }

            if (!BasicValidation.IsTimeFromTextValueValid(StartTimeText))
            {
                MessageBox.Show("Please enter valid start time.", "Invalid input");
                return false;
            }

            if (!BasicValidation.IsIntegerFromTextValid(DurationText))
            {
                MessageBox.Show("Please enter duration in correct format (numbers only).", "Invalid input");
                return false;
            }

            if (Room == null)
            {
                MessageBox.Show("Please select operation room.", "Invalid input");
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
