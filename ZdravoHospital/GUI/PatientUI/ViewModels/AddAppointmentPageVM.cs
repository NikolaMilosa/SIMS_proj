using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.View;

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
        public bool Mode { get; private set; }//true=add,false=edit

        public PeriodService PeriodFunctions { get; private set; }
        public RoomSheduleService RoomFunctions { get; private set; }
        public PatientService PatientFunctions { get; private set; }
        public InjectService Injection { get; private set; }
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
            if (!PatientFunctions.ActionTaken())
                return;
            SerializePeriod();
            PatientWindowVM.NavigationService.Navigate(new PeriodPage(Period.PatientUsername));
        }

        private bool ConfirmCanExecute(object parameter)
        {
            if (PatientFunctions.IsTrollDetected() || !IsFormFilled())
                return false;

            FillOutPeriod();//pokupi podatke iz forme i kreiraj ostatak perioda na osnovu njih

            if (!IsPeriodAvailable()) return false;

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
        private bool IsPeriodAvailable()
        {
            if (Period.RoomId == -1)
            {
                ErrorMessage = "No free rooms at the selected time.";
                return false;
            }

            if (PeriodFunctions.CheckPeriodAvailability(Period)) return true;
            ErrorMessage = PeriodFunctions.ErrorMessage;
            return false;

        }
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
            ViewService viewFunctions = new ViewService();
            if (Mode)
            {
                PeriodFunctions.SerializeNewPeriod(Period);
                viewFunctions.ShowOkDialog("Appointment", "Appointment is succesfully added!");
            }
            else
            {
                PeriodFunctions.UpdatePeriod(Period);
                viewFunctions.ShowOkDialog("Appointment", "Appointment is succesfully edited!");
            }
        }
        private void SetProperties(bool mode)
        {
            RoomFunctions = new RoomSheduleService();
            PatientFunctions = new PatientService(PatientWindowVM.PatientUsername);
            PeriodFunctions = new PeriodService();
            Mode = mode;
            PeriodList = new ObservableCollection<TimeSpan>();
            DoctorList = new ObservableCollection<DoctorDTO>();
            Injection = new InjectService();
        }

        private void GenerateComboBoxes()
        {
            Injection.GenerateObesrvableTimes(PeriodList);
            Injection.FillObservableDoctorDTOCollection(DoctorList);
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
