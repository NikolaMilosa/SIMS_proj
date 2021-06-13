using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.DoctorUI.Services;
using ZdravoHospital.GUI.DoctorUI.Exceptions;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI.ViewModel
{
    public class TreatmentViewModel : ViewModel
    {
        private NavigationService _navigationService;
        private Period _period;
        private TreatmentService _treatmentService;

        public DateTime StartDate { get; set; }
        public string DurationText { get; set; }
        public Room Room { get; set; }
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

        public MyICommand BackCommand { get; set; }

        public void Executed_BackCommand()
        {
            _navigationService.GoBack();
        }

        public bool CanExecute_BackCommand()
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

        public MyICommand ConfirmCommand { get; set; }

        public void Executed_ConfirmCommand()
        {
            if (!IsInputValid())
            {
                MessagePopUpVisibility = Visibility.Visible;
                return;
            }

            FormTreatment();

            try
            {
                _treatmentService.SaveTreatment(_period);
                MessageText = "Treatment saved successfully.";
                ConfirmButtonVisibility = Visibility.Collapsed;
                EditButtonVisibility = Visibility.Visible;
            }
            catch (RoomRenovatingException)
            {
                MessageText = "Selected room is being renovated during selected period.";
            }
            catch (BedsUnavailableException)
            {
                MessageText = "There are no beds available in selected room during selected period.";
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

        public TreatmentViewModel(NavigationService navigationService, Period period)
        {
            _navigationService = navigationService;
            _period = period;
            _treatmentService = new TreatmentService();
            Rooms = new ObservableCollection<Room>(new RoomService().GetBedrooms());

            InitializeCommands();

            if (period.Treatment == null)
            {
                EditButtonVisibility = Visibility.Collapsed;
                IsEditModeOn = true;
                StartDate = DateTime.Now.Date;
                DurationText = "0";
            }
            else
            {
                ConfirmButtonVisibility = Visibility.Collapsed;
                StartDate = _period.Treatment.StartDate;
                DurationText = _period.Treatment.Duration.ToString();
                Room = Rooms.ToList().Find(r => r.Id == period.Treatment.RoomId);
            }

            MessagePopUpVisibility = Visibility.Collapsed;
        }

        private void InitializeCommands()
        {
            BackCommand = new MyICommand(Executed_BackCommand, CanExecute_BackCommand);
            EditCommand = new MyICommand(Executed_EditCommand, CanExecute_EditCommand);
            ConfirmCommand = new MyICommand(Executed_ConfirmCommand, CanExecute_ConfirmCommand);
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
        }

        private bool IsInputValid()
        {
            if (!BasicValidation.IsIntegerFromTextValid(DurationText))
            {
                MessageText = "Please enter duration in correct format (numbers only).";
                return false;
            }

            if (Room == null)
            {
                MessageText = "Please select room.";
                return false;
            }

            return true;
        }

        private void FormTreatment()
        {
            _period.Treatment = new Treatment()
            {
                StartDate = StartDate.Date,
                Duration = Int32.Parse(DurationText),
                RoomId = Room.Id
            };
        }
    }
}
