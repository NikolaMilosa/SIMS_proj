using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Model;
using Repository.DoctorPersistance;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class DoctorReportDialogViewModel : ViewModel
    {
        #region Fields

        private IDoctorRepository _doctorRepository;

        private ObservableCollection<Doctor> _doctors;
        private int _selectedIndex;
        private Doctor _selectedDoctor;
        private bool _isDropDownOpen;

        private DateTime _startDate;
        private bool _isDropDownOpenStartPicker;

        private DateTime _endDate;
        private bool _isDropDownOpenEndPicker;

        private ObservableCollection<DoctorReportDTO> _doctorReport;
        
        private DoctorReportService _doctorReportService;

        private bool _focusExport;

        #endregion

        #region Properties

        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set
            {
                _doctors = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                FillDoctorReport();
                OnPropertyChanged();
            }
        }

        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set
            {
                _isDropDownOpen = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;

                if (EndDate <= _startDate)
                {
                    EndDate = _startDate.AddDays(1);
                }

                FillDoctorReport();
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                FillDoctorReport();
                OnPropertyChanged();
            }
        }

        public bool IsDropDownOpenStartPicker
        {
            get => _isDropDownOpenStartPicker;
            set
            {
                _isDropDownOpenStartPicker = value;
                OnPropertyChanged();
            }
        }

        public bool IsDropDownOpenEndPicker
        {
            get => _isDropDownOpenEndPicker;
            set
            {
                _isDropDownOpenEndPicker = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DoctorReportDTO> DoctorReport
        {
            get => _doctorReport;
            set
            {
                _doctorReport = value;
                OnPropertyChanged();
            }
        }

        public bool FocusExport
        {
            get => _focusExport;
            set
            {
                _focusExport = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand<KeyEventArgs> ComboBoxCommand { get; set; }
        public MyICommand<KeyEventArgs> StartDateCommand { get; set; }
        public MyICommand<KeyEventArgs> EndDateCommand { get; set; }
        public MyICommand ConfirmCommand { get; set; }
        public MyICommand<KeyEventArgs> TableCommand { get; set; }

        #endregion

        public DoctorReportDialogViewModel(InjectorDTO injector)
        {
            _doctorRepository = injector.DoctorRepository;

            _doctorReportService = new DoctorReportService(injector);

            Doctors = new ObservableCollection<Doctor>(_doctorRepository.GetValues());
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);

            ComboBoxCommand = new MyICommand<KeyEventArgs>(OnComboBox);
            StartDateCommand = new MyICommand<KeyEventArgs>(OnStartDate);
            EndDateCommand = new MyICommand<KeyEventArgs>(OnEndDate);
            TableCommand = new MyICommand<KeyEventArgs>(OnTableCommand);
            ConfirmCommand = new MyICommand(OnConfirm);
        }

        #region Button functions

        private void OnComboBox(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsDropDownOpen = (IsDropDownOpen == false) ? true : false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (SelectedIndex < Doctors.Count - 1 && IsDropDownOpen)
                {
                    SelectedIndex += 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (SelectedIndex > 0 && IsDropDownOpen)
                {
                    SelectedIndex -= 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                IsDropDownOpen = false;
            }
        }

        private void OnStartDate(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsDropDownOpenStartPicker = (IsDropDownOpenStartPicker == false);
            }
            else if (e.Key == Key.Tab)
            {
                IsDropDownOpenStartPicker = false;
            }
            else if (!IsDropDownOpenStartPicker)
            {
                e.Handled = true;
            }
        }

        private void OnEndDate(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsDropDownOpenEndPicker = (IsDropDownOpenEndPicker == false) ? true : false;
            }
            else if (e.Key == Key.Tab)
            {
                IsDropDownOpenEndPicker = false;
            }
            else if (!IsDropDownOpenEndPicker)
            {
                e.Handled = true;
            }
        }

        private void OnTableCommand(KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {

            }
            else if (e.Key == Key.Tab || e.Key == Key.Escape)
            {
                FocusExport = true;
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }

            FocusExport = false;
        }

        private void OnConfirm()
        {
            _doctorReportService.GeneratePDF(SelectedDoctor, StartDate, EndDate, new List<DoctorReportDTO>(DoctorReport));
        }

        #endregion

        #region Private functions

        private void FillDoctorReport()
        {
            if (SelectedDoctor != null)
            {
                DoctorReport = _doctorReportService.CreateReport(SelectedDoctor, StartDate, EndDate);
            }
        }

        #endregion
    }
}
