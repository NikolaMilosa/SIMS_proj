using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using ZdravoHospital.GUI.PatientUI.Commands;

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


        #endregion

        #region Constructors

        public SuggestAppointPageVM()
        {
            SetProperties();
            SetCommands();

        }

        #endregion

        #region Commands
        public RelayCommand RadioButtonCommand { get; private set; }
        public RelayCommand SuggestCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region CommandActions

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
            return true;
        }

        public void CancelExecute(object parameter)
        {

        }

        #endregion

        #region Methods

        private void SetProperties()
        {
            DatePanelVisibility = Visibility.Collapsed;
            DoctorPanelVisibility = Visibility.Collapsed;
        }

        private void SetCommands()
        {
            RadioButtonCommand = new RelayCommand(RadioExecute);
            CancelCommand = new RelayCommand(CancelExecute);
            SuggestCommand = new RelayCommand(SuggestExecute, SuggestCanExecute);
        }
        #endregion
    }
}
