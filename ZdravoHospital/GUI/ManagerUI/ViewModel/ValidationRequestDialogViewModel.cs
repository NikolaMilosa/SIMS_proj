using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.Logics;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class ValidationRequestDialogViewModel : ViewModel
    {
        #region Fields

        private Medicine _observedMedicine;
        private Doctor _selectedDoctor;
        private ObservableCollection<Doctor> _listOfDoctors;

        private MedicineFunctions _medicineFunctions;

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

        public ValidationRequestDialogViewModel(Medicine medicine)
        {
            ObservedMedicine = medicine;
            ListOfDoctors = new ObservableCollection<Doctor>(Resources.doctors.Values);

            _medicineFunctions = new MedicineFunctions(null);

            ConfirmCommand = new MyICommand(OnConfirm);
        }

        #region Button functions

        private void OnConfirm()
        {
            _medicineFunctions.SendMedicineOnRecension(ObservedMedicine, SelectedDoctor);
        }

        #endregion
    }
}
