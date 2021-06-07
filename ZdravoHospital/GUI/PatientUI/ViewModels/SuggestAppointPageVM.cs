using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class SuggestAppointPageVM : ViewModel
    {
        #region Properties

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private Visibility doctorVisibility;

        public Visibility DoctorPanelVisibility
        {
            get => doctorVisibility;
            set
            {
                doctorVisibility = value;
                OnPropertyChanged("DoctorPanelVisibility");
            }
        }

        private Visibility dateVisibility;

        public Visibility DatePanelVisibility
        {
            get => dateVisibility;
            set
            {
                dateVisibility = value;
                OnPropertyChanged("DatePanelVisibility");
            }
        }

        public DateTime DisplayDateStart
        {
            get => DateTime.Now.AddDays(3);
            set { }
        }

        public ObservableCollection<DoctorDTO> DoctorList { get; set; }
        public ObservableCollection<TimeSpan> PeriodList { get; set; }
        public InjectFunctions Injection { get; private set; }

        private DoctorDTO selectedDoctorDto;

        public DoctorDTO SelectedDoctorDTO
        {
            get => selectedDoctorDto;
            set
            {
                selectedDoctorDto = value;
                OnPropertyChanged("SelectedDoctorDTO");
            }
        }

        private TimeSpan selectedTimeSpan;

        public TimeSpan SelectedTimeSpan
        {
            get => selectedTimeSpan;
            set
            {
                selectedTimeSpan = value;
                OnPropertyChanged("SelectedTimeSpan");
            }

        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }

        public ObservableCollection<PeriodDTO> PeriodDTOs { get; private set; }
        public PeriodDTO SelectedPeriodDTO { get; set; }

        #endregion

        #region Constructors

        public SuggestAppointPageVM()
        {
            SetProperties();
            SetCommands();
            GenerateComboBoxes();
        }

        #endregion

        #region Commands

        public RelayCommand RadioButtonCommand { get; private set; }
        public RelayCommand SuggestCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand ConfirmCommand { get; private set; }

        #endregion

        #region CommandActions

        private void ConfirmExecute(object parameter)
        {
            PatientFunctions patientFunctions = new PatientFunctions(PatientWindowVM.PatientUsername);
            if (!patientFunctions.ActionTaken())
                return;
            SerializePeriod();
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowOkDialog("Appointment", "Appointment is succesfully added!");
            PatientWindowVM.NavigationService.Navigate(new PeriodPage(PatientWindowVM.PatientUsername));
        }

        private bool ConfirmCanExecute(object parameter) 
        {
            PatientFunctions patientFunctions = new PatientFunctions(PatientWindowVM.PatientUsername);
            return !patientFunctions.IsTrollDetected();
        }

        private void RadioExecute(object parameter)
        {
            int radioNum = Int32.Parse((string) parameter);
            SetPanelVisibility(radioNum);
            PeriodDTOs.Clear();
        }

        private void SuggestExecute(object parameter)
        {
            PeriodDTOs.Clear();
            if (DoctorPanelVisibility == Visibility.Visible)
            {
                SuggestTimeFunctions timeFunctions = new SuggestTimeFunctions(PeriodDTOs, SelectedDoctorDTO);
                timeFunctions.GetSuggestedPeriods();
            }
            else
            {
                SuggestDoctorFunctions doctorFunctions =
                    new SuggestDoctorFunctions(SelectedDate, SelectedTimeSpan, PeriodDTOs);
                doctorFunctions.GetSuggestedPeriods();
            }
        }

        private bool SuggestCanExecute(object parameter)
        {
            return IsDateFormFilledCorrect() || IsDoctorFormFilled();
        }

        public void CancelExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new AddAppointmentPage(null));
        }

        #endregion

        #region Methods

        private void SetPanelVisibility(int radioNum)
        {
            if (radioNum == 1)
            {
                DoctorPanelVisibility = Visibility.Visible;
                DatePanelVisibility = Visibility.Collapsed;
            }
            else
            {
                DoctorPanelVisibility = Visibility.Collapsed;
                DatePanelVisibility = Visibility.Visible;
            }
        }
        private void SerializePeriod()
        {
            PeriodConverter periodConverter = new PeriodConverter();
            PeriodFunctions periodFunctions = new PeriodFunctions();
            periodFunctions.SerializeNewPeriod(periodConverter.GeneratePeriod(SelectedPeriodDTO));
        }
        private bool IsDoctorFormFilled()
        {
            return DoctorPanelVisibility == Visibility.Visible && SelectedDoctorDTO != null;
        }

        private bool IsDateFormFilledCorrect()
        {
            if (DatePanelVisibility != Visibility.Visible || SelectedTimeSpan == TimeSpan.Zero ||
                SelectedDate == DateTime.MinValue) return false;
            return CheckIsDateAvailable();
        }

        private bool CheckIsDateAvailable()
        {
            PeriodFunctions periodFunctions = new PeriodFunctions();
            Period period = GeneratePeriodFromInput(periodFunctions);
            if (period.RoomId != -1) return CheckIsPeriodAvailable(periodFunctions, period);
            ErrorMessage = "There is no free rooms at selected time!";
            return false;
        }

        private bool CheckIsPeriodAvailable(PeriodFunctions periodFunctions, Period period)
        {
            if (periodFunctions.CheckPeriodAvailability(period))
            {
                ErrorMessage = "";
                return true;
            }
            ErrorMessage = periodFunctions.ErrorMessage;
            return false;
        }

        private Period GeneratePeriodFromInput(PeriodFunctions periodFunctions)
        {
            RoomSheduleFunctions roomFunctions = new RoomSheduleFunctions();
            Period period= new Period()
            {
                PatientUsername = PatientWindowVM.PatientUsername,
                Duration = 30,
                StartTime = SelectedDate + SelectedTimeSpan,
                PeriodId = periodFunctions.GeneratePeriodId(),
                PeriodType = PeriodType.APPOINTMENT
            };
            period.RoomId = roomFunctions.GetFreeRoom(period);
            return period;
        }

        private void SetProperties()
        {
            SetPanelVisibility();
            PeriodDTOs = new ObservableCollection<PeriodDTO>();
            SelectedDate = DateTime.Today.AddDays(3);
            Injection = new InjectFunctions();
            PeriodList = new ObservableCollection<TimeSpan>();
            DoctorList = new ObservableCollection<DoctorDTO>();
        }

        private void SetPanelVisibility()
        {
            DatePanelVisibility = Visibility.Collapsed;
            DoctorPanelVisibility = Visibility.Collapsed;
        }

        private void SetCommands()
        {
            RadioButtonCommand = new RelayCommand(RadioExecute);
            CancelCommand = new RelayCommand(CancelExecute);
            SuggestCommand = new RelayCommand(SuggestExecute, SuggestCanExecute);
            ConfirmCommand = new RelayCommand(ConfirmExecute,ConfirmCanExecute);
        }
        private void GenerateComboBoxes()
        {
            Injection.GenerateObesrvableTimes(PeriodList);
            Injection.FillObservableDoctorDTOCollection(DoctorList);
        }
        #endregion
    }
}
