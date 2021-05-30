using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.DoctorUI.Controllers;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.ViewModel
{
    public class ReferralViewModel : ViewModel
    {
        private NavigationService _navigationService;
        private Referral _referral;
        private Period _period;
        private ReferralController _referralController;
        private PeriodController _periodController;

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

        private Visibility _cancelDialogVisibility;
        public Visibility CancelDialogVisibility
        {
            get
            {
                return _cancelDialogVisibility;
            }
            set
            {
                _cancelDialogVisibility = value;
                OnPropertyChanged("CancelDialogVisibility");
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

        private Visibility _referredAppointmentButtonVisibility;
        public Visibility ReferredAppointmentButtonVisibility
        {
            get
            {
                return _referredAppointmentButtonVisibility;
            }
            set
            {
                _referredAppointmentButtonVisibility = value;
                OnPropertyChanged("ReferredAppointmentButtonVisibility");
            }
        }

        private Visibility _referredOperationButtonVisibility;
        public Visibility ReferredOperationButtonVisibility
        {
            get
            {
                return _referredOperationButtonVisibility;
            }
            set
            {
                _referredOperationButtonVisibility = value;
                OnPropertyChanged("ReferredOperationButtonVisibility");
            }
        }

        private Visibility _useStackPanelVisibility;
        public Visibility UseStackPanelVisibility
        {
            get
            {
                return _useStackPanelVisibility;
            }
            set
            {
                _useStackPanelVisibility = value;
                OnPropertyChanged("UseStackPanelVisibility");
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
            }
        }
        private string _noteText;
        public string NoteText
        {
            get
            {
                return _noteText;
            }
            set
            {
                _noteText = value;
                OnPropertyChanged("NoteText");
            }
        }

        private string _daysToUseText;
        public string DaysToUseText
        {
            get
            {
                return _daysToUseText;
            }
            set
            {
                _daysToUseText = value;
                OnPropertyChanged("DaysToUseText");
            }
        }

        public bool IsReadonlyModeOn { get; set; }
        public Doctor ReferringDoctor { get; set; }
        public Doctor ReferredDoctor { get; set; }
        public Patient Patient { get; set; }

        public ObservableCollection<Doctor> Doctors { get; set; }
        public MyICommand ConfirmCommand { get; set; }

        public void Executed_ConfirmCommand()
        {
            if (!IsInputValid())
            {
                MessagePopUpVisibility = Visibility.Visible;
                return;
            }

            FormReferral();

            if (_referral.Period.ReferringReferralId == -1)
            {
                _referralController.CreateNewReferral(_referral);
                MessageText = "Referral created successfully.";
            }
            else
            {
                _referralController.UpdateReferral(_referral);
                MessageText = "Referral updated successfully.";
            }

            _period.ReferringReferralId = _referral.ReferralId;
            _periodController.UpdatePeriodWithoutValidation(_period);

            MessagePopUpVisibility = Visibility.Visible;
            UseStackPanelVisibility = Visibility.Visible;
            ConfirmButtonVisibility = Visibility.Collapsed;
            EditButtonVisibility = Visibility.Visible;
            IsEditModeOn = false;
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
            UseStackPanelVisibility = Visibility.Collapsed;
            ReferredAppointmentButtonVisibility = Visibility.Collapsed;
            ReferredOperationButtonVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_EditCommand()
        {
            return true;
        }

        public MyICommand BackCommand { get; set; }

        public void Executed_BackCommand()
        {
            _navigationService.GoBack();
        }

        public bool CanExecute_BackCommand()
        {
            return true;
        }

        public MyICommand UseForAppointmentCommand { get; set; }

        public void Executed_UseForAppointmentCommand()
        {
            _navigationService.Navigate(new NewAppointmentPage(_referral, Patient));
        }

        public bool CanExecute_UseForAppointmentCommand()
        {
            return true;
        }

        public MyICommand UseForOperationCommand { get; set; }

        public void Executed_UseForOperationCommand()
        {
            _navigationService.Navigate(new NewOperationPage(_referral, Patient));
        }

        public bool CanExecute_UseForOperationCommand()
        {
            return true;
        }

        public MyICommand ReferredAppointmentCommand { get; set; }

        public void Executed_ReferredAppointmentCommand()
        {
            _navigationService.Navigate(new AppointmentPage(_referral.Period));
        }

        public bool CanExecute_ReferredAppointmentCommand()
        {
            return true;
        }

        public MyICommand ReferredOperationCommand { get; set; }

        public void Executed_ReferredOperationCommand()
        {
            if (_referral.ReferredDoctorUsername.Equals(App.currentUser) || _referral.ReferringDoctorUsername.Equals(App.currentUser))
                _navigationService.Navigate(new OperationPage(_referral.Period, false));
            else
                _navigationService.Navigate(new OperationPage(_referral.Period, true));
        }

        public bool CanExecute_ReferredOperationCommand()
        {
            return true;
        }

        public ReferralViewModel(NavigationService navigationService, Doctor referringDoctor, Patient patient, Period period)
        {
            _navigationService = navigationService;
            _referralController = new ReferralController();
            _periodController = new PeriodController();
            _referral = _referralController.GetReferral(period.ReferringReferralId);
            ReferringDoctor = referringDoctor;
            Patient = patient;
            _period = period;
            Doctors = new ObservableCollection<Doctor>(new DoctorController().GetOtherDoctors(period.DoctorUsername));

            InitializeCommands();

            if (_referral != null)
            {
                ReferredDoctor = Doctors.ToList().Find(d => d.Username.Equals(_referral.ReferredDoctorUsername));
                NoteText = _referral.Note;
                DaysToUseText = _referral.DaysToUse.ToString();
                ConfirmButtonVisibility = Visibility.Collapsed;

                if (_referral.IsUsed)
                {
                    EditButtonVisibility = Visibility.Collapsed;
                    ConfirmButtonVisibility = Visibility.Collapsed;
                    UseStackPanelVisibility = Visibility.Collapsed;

                    if (_referral.Period.PeriodType == PeriodType.APPOINTMENT)
                    {
                        ReferredAppointmentButtonVisibility = Visibility.Visible;
                        ReferredOperationButtonVisibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ReferredOperationButtonVisibility = Visibility.Visible;
                        ReferredAppointmentButtonVisibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    ReferredAppointmentButtonVisibility = Visibility.Collapsed;
                    ReferredOperationButtonVisibility = Visibility.Collapsed;

                    if (_referral.Period.PeriodType == PeriodType.APPOINTMENT)
                        ReferredOperationButtonVisibility = Visibility.Collapsed;
                    else
                        ReferredAppointmentButtonVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                IsEditModeOn = true;
                EditButtonVisibility = Visibility.Collapsed;
                UseStackPanelVisibility = Visibility.Collapsed;
                ReferredAppointmentButtonVisibility = Visibility.Collapsed;
                ReferredOperationButtonVisibility = Visibility.Collapsed;
            }

            MessagePopUpVisibility = Visibility.Collapsed;
        }

        public ReferralViewModel(NavigationService navigationService, Referral referral, Patient patient)
        {
            //_navigationService = navigationService;

            //readonlyMode = true;
            //ReferringDoctor = Model.Resources.doctors[referral.ReferringDoctorUsername];
            //Doctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values);
            //DoctorsComboBox.SelectedItem = Model.Resources.doctors[referral.ReferredDoctorUsername];
            //Patient = patient;
            //NoteTextBox.Text = referral.Note;
            //DaysToUseTextBox.Text = referral.DaysToUse.ToString();
            //DoctorsComboBox.IsHitTestVisible = false;
            //DoctorsComboBox.IsTabStop = false;
            //NoteTextBox.IsReadOnly = true;
            //DaysToUseTextBox.IsReadOnly = true;
        }

        private void InitializeCommands()
        {
            ConfirmCommand = new MyICommand(Executed_ConfirmCommand, CanExecute_ConfirmCommand);
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
            EditCommand = new MyICommand(Executed_EditCommand, CanExecute_EditCommand);
            BackCommand = new MyICommand(Executed_BackCommand, CanExecute_BackCommand);
            UseForAppointmentCommand = new MyICommand(Executed_UseForAppointmentCommand, CanExecute_UseForAppointmentCommand);
            UseForOperationCommand = new MyICommand(Executed_UseForOperationCommand, CanExecute_UseForOperationCommand);
            ReferredAppointmentCommand = new MyICommand(Executed_ReferredAppointmentCommand, CanExecute_ReferredAppointmentCommand);
            ReferredOperationCommand = new MyICommand(Executed_ReferredOperationCommand, CanExecute_ReferredOperationCommand);
        }

        private bool IsInputValid()
        {
            if (!BasicValidation.IsIntegerFromTextValid(DaysToUseText))
            {
                MessageText = "Please enter days to use in correct format (numbers only).";
                MessagePopUpVisibility = Visibility.Visible;
                return false;
            }

            return true;
        }

        private void FormReferral()
        {
            _referral = new Referral()
            {
                Period = _period,
                ReferringDoctorUsername = ReferringDoctor.Username,
                ReferredDoctorUsername = ReferredDoctor.Username,
                DaysToUse = Int32.Parse(DaysToUseText),
                Note = NoteText
            };
        }
    }
}
