using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Printing;
using System.Text;
using System.Windows.Input;
using Model;
using Repository.DoctorPersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class ValidationRequestDialogViewModel : ViewModel
    {
        #region Fields

        private Medicine _observedMedicine;
        private Doctor _selectedDoctor;
        private ObservableCollection<Doctor> _listOfDoctors;

        private MedicineService _medicineService;

        private IDoctorRepository _doctorRepository;

        private bool _isDropDownOpen;
        private int _selectedIndex;

        #endregion

        #region Properties

        public Medicine ObservedMedicine
        {
            get => _observedMedicine;
            set
            {
                _observedMedicine = value;
                OnPropertyChanged();
            }
        }

        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Doctor> ListOfDoctors
        {
            get => _listOfDoctors;
            set
            {
                _listOfDoctors = value;
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

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }
        public MyICommand<KeyEventArgs> ComboBoxCommand { get; set; }

        #endregion

        public ValidationRequestDialogViewModel(Medicine medicine, InjectorDTO injector)
        {
            ObservedMedicine = medicine;
            _doctorRepository = injector.DoctorRepository;
            ListOfDoctors = new ObservableCollection<Doctor>(_doctorRepository.GetValues());

            _medicineService = new MedicineService(null, injector);

            SelectedIndex = -1;

            ConfirmCommand = new MyICommand(OnConfirm);
            ComboBoxCommand = new MyICommand<KeyEventArgs>(OnComboBox);
        }

        #region Button functions

        private void OnConfirm()
        {
            _medicineService.SendMedicineOnRecension(ObservedMedicine, SelectedDoctor);
        }

        private void OnComboBox(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsDropDownOpen = (IsDropDownOpen == false) ? true : false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (IsDropDownOpen && SelectedIndex < ListOfDoctors.Count - 1)
                {
                    SelectedIndex += 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (IsDropDownOpen && SelectedIndex > 0)
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
        #endregion
    }
}
