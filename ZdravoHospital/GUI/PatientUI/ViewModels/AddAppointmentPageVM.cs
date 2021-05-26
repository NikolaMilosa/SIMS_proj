using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.ManagerUI.Logics;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class AddAppointmentPageVM : ViewModel
    {
        #region Properties
        public ObservableCollection<DoctorDTO> DoctorList { get; set; }
        public ObservableCollection<TimeSpan> PeriodList { get; set; }

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
        public DateTime DisplayDateStart { get=>DateTime.Now.AddDays(3);
            set{}
        }
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
        public Period Period { get; set; }

        public AddAppointmentValidations Validations { get; private set; }
        public bool Mode { get; private set; }//true=add,false=edit

        public PeriodFunctions PeriodFunctions { get; private set; }
        public RoomSheduleFunctions RoomFunctions { get; private set; }
        public PatientFunctions PatientFunctions { get; private set; }
        public InjectFunctions Injection { get; private set; }
        #endregion

        #region Constructors

        public AddAppointmentPageVM()
        {
            SetProperties(true);
            GenerateComboBoxes();
            GenerateNewPeriod();
            SetCommands();
        }

        public AddAppointmentPageVM(Period period)
        {
            SetProperties(false);
            GenerateComboBoxes();
            GenerateOldPeriod(period);
            SetCommands();
        }

        #endregion

        #region Commands

        public RelayCommand ConfirmCommand { get; private set; }

        public RelayCommand SuggestCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region CommandActions

        private void ConfirmExecute(object parameter)
        {
            SerializePeriod();
            PatientFunctions.ActionTaken();
            PatientWindowVM.NavigationService.Navigate(new PeriodPage(Period.PatientUsername));
        }

        private bool ConfirmCanExecute(object parameter)
        {
            if (PatientFunctions.IsTrollDetected() || !IsFormFilled())
                return false;

            FillOutPeriod();//pokupi podatke iz forme i kreiraj ostatak perioda na osnovu njih

            if (Period.RoomId == -1)
            {
                ErrorMessage = "No free rooms at the selected time.";
                return false;
            }

            if (!PeriodFunctions.CheckPeriodAvailability(Period, false))
            {
                ErrorMessage = PeriodFunctions.ErrorMessage;
                return false;
            }

            ErrorMessage = "";

            return true;
        }

        public void SuggestExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new SuggestAppointmentPage());
        }

        public void CancelExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new PeriodPage(Period.PatientUsername));
        }

        #endregion

        #region Methods

        private void SetCommands()
        {
            ConfirmCommand = new RelayCommand(ConfirmExecute, ConfirmCanExecute);
            CancelCommand = new RelayCommand(CancelExecute);
            SuggestCommand = new RelayCommand(SuggestExecute);
        }
        private void FillOutPeriod()
        {
            Period.StartTime = Period.StartTime.Date + SelectedTimeSpan;
            Period.DoctorUsername = SelectedDoctorDTO.Username;
            Period.RoomId = RoomFunctions.GetFreeRoom(Period);
            
        }

        private  bool IsFormFilled()
        {
            
            return SelectedTimeSpan != TimeSpan.Zero && Period.StartTime != null && SelectedDoctorDTO != null;
        }

        private void SerializePeriod()
        {
            if(Mode)
                PeriodFunctions.SerializeNewPeriod(Period);
            else
                PeriodFunctions.UpdatePeriod(Period);
        }
        private void SetProperties(bool mode)
        {
            RoomFunctions = new RoomSheduleFunctions();
            PatientFunctions = new PatientFunctions(PatientWindowVM.PatientUsername);
            PeriodFunctions = new PeriodFunctions();
            Mode = mode;
            PeriodList = new ObservableCollection<TimeSpan>();
            DoctorList = new ObservableCollection<DoctorDTO>();
            Injection = new InjectFunctions();
        }

        private void GenerateComboBoxes()
        {
            Injection.GenerateObesrvableTimes(PeriodList);
            Injection.FillDoctorList(DoctorList);
        }

        private void GenerateNewPeriod()
        {
            Period = new Period
            {
                PatientUsername = PatientWindowVM.PatientUsername,
                Duration = 30,
                StartTime = DateTime.Now.AddDays(3),
                PeriodId = PeriodFunctions.GeneratePeriodId()
            };
        }

        private void GenerateOldPeriod(Period period)
        {
            Period = period;
            SelectedTimeSpan = period.StartTime.TimeOfDay;
            SelectedDoctorDTO = GetDoctor(period.DoctorUsername);
        }

        private DoctorDTO GetDoctor(string username)
        {
            return DoctorList.FirstOrDefault(doctor => doctor.Username.Equals(username));
        }
        #endregion

    }
}
