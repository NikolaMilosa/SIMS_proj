using Model;
using System.Windows;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.ViewModel
{
    public class PeriodDetailsViewModel : ViewModel
    {
        private NavigationService _navigationService;
        private Period _period;
        private PeriodService _periodService;
        private bool _isEditModeOn;

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

        private Visibility _changesDialogVisibility;
        public Visibility ChangesDialogVisibility
        {
            get
            {
                return _changesDialogVisibility;
            }
            set
            {
                _changesDialogVisibility = value;
                OnPropertyChanged("ChangesDialogVisibility");
            }
        }
        public string PeriodDetailsText { get; set; }
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

        public MyICommand BackCommand { get; set; }

        public void Executed_BackCommand()
        {
            _navigationService.GoBack();
        }

        public bool CanExecute_BackCommand()
        {
            return true;
        }

        public MyICommand ConfirmCommand { get; set; }

        public void Executed_ConfirmCommand()
        {
            if (_period.Details == null)
                Executed_YesChangeCommand();
            else if (!PeriodDetailsText.Equals(_period.Details))
                ChangesDialogVisibility = Visibility.Visible;
            else
            {
                ConfirmButtonVisibility = Visibility.Collapsed;
                EditButtonVisibility = Visibility.Visible;
                IsEditModeOn = false;
            }
        }

        public bool CanExecute_ConfirmCommand()
        {
            return true;
        }

        public MyICommand YesChangeCommand { get; set; }

        public void Executed_YesChangeCommand()
        {
            _period.Details = PeriodDetailsText;
            _periodService.UpdatePeriodWithoutValidation(_period);
            ChangesDialogVisibility = Visibility.Collapsed;
            MessageText = "Anamnesis saved successfully.";
            MessagePopUpVisibility = Visibility.Visible;
        }

        public bool CanExecute_YesChangeCommand()
        {
            return true;
        }

        public MyICommand NoChangeCommand { get; set; }

        public void Executed_NoChangeCommand()
        {
            ChangesDialogVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_NoChangeCommand()
        {
            return true;
        }

        public MyICommand CloseMessagePopUpCommand { get; set; }

        public void Executed_CloseMessagePopUpCommand()
        {
            MessagePopUpVisibility = Visibility.Collapsed;
            _navigationService.GoBack();
        }

        public bool CanExecute_CloseMessagePopUpCommand()
        {
            return true;
        }

        public PeriodDetailsViewModel(NavigationService navigationService, Period period)
        {
            _navigationService = navigationService;
            _period = period;
            _periodService = new PeriodService();

            InitializeCommands();

            if (_period.Details == null)
            {
                EditButtonVisibility = Visibility.Collapsed;
                IsEditModeOn = true;
            }
            else
            {
                ConfirmButtonVisibility = Visibility.Collapsed;
                PeriodDetailsText = _period.Details;
            }

            ChangesDialogVisibility = Visibility.Collapsed;
            MessagePopUpVisibility = Visibility.Collapsed;
        }

        private void InitializeCommands()
        {
            ConfirmCommand = new MyICommand(Executed_ConfirmCommand, CanExecute_ConfirmCommand);
            EditCommand = new MyICommand(Executed_EditCommand, CanExecute_EditCommand);
            BackCommand = new MyICommand(Executed_BackCommand, CanExecute_BackCommand);
            YesChangeCommand = new MyICommand(Executed_YesChangeCommand, CanExecute_YesChangeCommand);
            NoChangeCommand = new MyICommand(Executed_NoChangeCommand, CanExecute_NoChangeCommand);
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
        }
    }
}
