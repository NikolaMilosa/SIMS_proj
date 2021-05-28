using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.DoctorUI.Controllers;
using ZdravoHospital.GUI.DoctorUI.Exceptions;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.ViewModel
{
    public class AppointmentViewModel : ViewModel
    {
        private Period _period;
        private PeriodController _periodController;

        private bool _doctorPatientEditable;
        public bool DoctorPatientEditable 
        { 
            get
            {
                return _doctorPatientEditable;
            }
            set
            {
                _doctorPatientEditable = value;
                OnPropertyChanged("DoctorPatientEditable");
            }
        }
        private bool _isEditModeOn;
        public bool IsEditModeOn
        {
            get
            {
                return _isEditModeOn;
            }
            set
            {
                _isEditModeOn = value;
                OnPropertyChanged("IsEditModeOn");

                if (value == true)
                {
                    if (_period.ReferredReferralId == -1)
                        DoctorPatientEditable = true;
                    else
                        DoctorPatientEditable = false;
                }
            }
        }
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

        private Visibility _messagePopUpVisibility;
        public Visibility MessagePopUpVisibility
        {
            get
            {
                return _messagePopUpVisibility;
            }
            set
            {
                _messagePopUpVisibility = value;
                OnPropertyChanged("MessagePopUpVisibility");
            }
        }

        private Visibility _editButtonVisibility;
        public Visibility EditButtonVisibility
        {
            get
            {
                return _editButtonVisibility;
            }
            set
            {
                _editButtonVisibility = value;
                OnPropertyChanged("EditButtonVisibility");
            }
        }

        private Visibility _confirmButtonVisibility;
        public Visibility ConfirmButtonVisibility
        {
            get
            {
                return _confirmButtonVisibility;
            }
            set
            {
                _confirmButtonVisibility = value;
                OnPropertyChanged("ConfirmButtonVisibility");
            }
        }

        private Visibility _cancelButtonVisibility;
        public Visibility CancelButtonVisibility
        {
            get
            {
                return _cancelButtonVisibility;
            }
            set
            {
                _cancelButtonVisibility = value;
                OnPropertyChanged("CancelButtonVisibility");
            }
        }

        private Visibility _periodEventButtonsVisibility;
        public Visibility PeriodEventButtonsVisibility
        {
            get
            {
                return _periodEventButtonsVisibility;
            }
            set
            {
                _periodEventButtonsVisibility = value;
                OnPropertyChanged("PeriodEventButtonsVisibility");
            }
        }
        private string _messageText;
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                _messageText = value;
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
                _periodController.UpdatePeriod(period);
                MessageText = "Appointment updated successfully.";
                IsEditModeOn = false;
                ConfirmButtonVisibility = Visibility.Collapsed;
                EditButtonVisibility = Visibility.Visible;
                //TODO: navigate back
                return;
            }
            catch (PeriodInPastException exception)
            {
                MessageText = "Cannot edit appointment which has already started or ended.";
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
            finally
            {
                MessagePopUpVisibility = Visibility.Visible;
            }
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

        public MyICommand EditCommand { get; set; }

        public void Executed_EditCommand()
        {
            IsEditModeOn = true;
            EditButtonVisibility = Visibility.Collapsed;
            ConfirmButtonVisibility = Visibility.Visible;
        }

        public bool CanExecute_EditCommand()
        {
            return true;
        }

        #endregion

        public AppointmentViewModel(Period period)
        {
            InitializeCommands();

            _period = period;
            _periodController = new PeriodController();

            Doctors = new ObservableCollection<Doctor>(new DoctorController().GetDoctors());
            Patients = new ObservableCollection<Patient>(new PatientController().GetPatients());
            Rooms = new ObservableCollection<Room>(new RoomController().GetAppointmentRooms());

            Doctor = Doctors.ToList().Find(d => d.Username.Equals(period.DoctorUsername));
            Patient = Patients.ToList().Find(p => p.Username.Equals(period.PatientUsername));
            StartDate = period.StartTime.Date;
            StartTimeText = period.StartTime.ToString("HH:mm");
            DurationText = period.Duration.ToString();
            Room = Rooms.ToList().Find(r => r.Id.Equals(period.RoomId));
            IsUrgent = period.IsUrgent;

            MessagePopUpVisibility = Visibility.Collapsed;
            ConfirmButtonVisibility = Visibility.Collapsed;

            if (DateTime.Now > period.StartTime)
            {
                EditButtonVisibility = Visibility.Collapsed;
                CancelButtonVisibility = Visibility.Collapsed;
            }
            else
            {
                PeriodEventButtonsVisibility = Visibility.Collapsed;
            }
        }

        private void InitializeCommands()
        {
            ConfirmCommand = new MyICommand(Executed_ConfirmCommand, CanExecute_ConfirmCommand);
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
            EditCommand = new MyICommand(Executed_EditCommand, CanExecute_EditCommand);
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
            period.PeriodId = _period.PeriodId;
            period.IsUrgent = IsUrgent;

            return period;
        }
    }
}
