using Model;
using System.Windows;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Commands;

namespace ZdravoHospital.GUI.DoctorUI.ViewModel
{
    public class TreatmentViewModel : ViewModel
    {
        private Treatment _treatment;
        private NavigationService _navigationService;
        private Period _period;

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
            //TODO
        }

        public bool CanExecute_EditCommand()
        {
            return true;
        }

        public MyICommand ConfirmCommand { get; set; }

        public void Executed_ConfirmCommand()
        {
            //TODO
        }

        public bool CanExecute_ConfirmCommand()
        {
            return true;
        }

        public TreatmentViewModel(NavigationService navigationService, Period period)
        {
            _navigationService = navigationService;
            _period = period;
            _treatment = period.Treatment;

            InitializeCommands();

            if (_treatment == null)
            {
                EditButtonVisibility = Visibility.Collapsed;
            }
            else
            {
                ConfirmButtonVisibility = Visibility.Collapsed;
            }
        }

        private void InitializeCommands()
        {
            BackCommand = new MyICommand(Executed_BackCommand, CanExecute_BackCommand);
            EditCommand = new MyICommand(Executed_EditCommand, CanExecute_EditCommand);
            ConfirmCommand = new MyICommand(Executed_ConfirmCommand, CanExecute_ConfirmCommand);
        }
    }
}
