using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class SuggestAppointPageVM : ViewModel
    {
        #region Properties
 
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

        }
        private void RadioExecute(object parameter)
        {
            int radioNum = Int32.Parse((string)parameter);
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

        private void SuggestExecute(object parameter)
        {
            
        }

        private bool SuggestCanExecute(object parameter)
        {
            return IsDateFormFilled() || IsDoctorFormFilled();
        }

        public void CancelExecute(object parameter)
        {

        }

        #endregion

        #region Methods

        private bool IsDoctorFormFilled()
        {
            return DoctorPanelVisibility == Visibility.Visible && SelectedDoctorDTO != null;
        }

        private bool IsDateFormFilled()
        {
            return DatePanelVisibility == Visibility.Visible && SelectedTimeSpan != TimeSpan.Zero &&
                   SelectedDate != DateTime.MinValue;
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
            ConfirmCommand = new RelayCommand(ConfirmExecute);
        }
        private void GenerateComboBoxes()
        {
            Injection.GenerateObesrvableTimes(PeriodList);
            Injection.FillDoctorList(DoctorList);
        }
        #endregion
    }
}
