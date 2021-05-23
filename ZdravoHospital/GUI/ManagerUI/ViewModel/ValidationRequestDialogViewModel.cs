using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public ValidationRequestDialogViewModel(Medicine medicine, InjectorDTO injector)
        {
            ObservedMedicine = medicine;
            _doctorRepository = injector.DoctorRepository;
            ListOfDoctors = new ObservableCollection<Doctor>(_doctorRepository.GetValues());

            _medicineService = new MedicineService(null, injector);

            ConfirmCommand = new MyICommand(OnConfirm);
        }

        #region Button functions

        private void OnConfirm()
        {
            _medicineService.SendMedicineOnRecension(ObservedMedicine, SelectedDoctor);
        }

        #endregion
    }
}
