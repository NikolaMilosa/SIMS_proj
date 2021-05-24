using System;
using System.Collections.Generic;
using System.Text;
using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class RejectionNoteDialogViewModel : ViewModel
    {
        #region Fields

        private Medicine _medicine;
        private string _rejectionReason;

        private MedicineService _medicineService;

        #endregion

        #region Properties

        public Medicine Medicine
        {
            get => _medicine;
            set
            {
                _medicine = value;

                RejectionReason = _medicineService.FindMedicineRecension(Medicine).RecensionNote;

                OnPropertyChanged();
            }
        }

        public string RejectionReason
        {
            get => _rejectionReason;
            set
            {
                _rejectionReason = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public RejectionNoteDialogViewModel(Medicine medicine, InjectorDTO injector)
        {
            _medicineService = new MedicineService(null, injector);
            Medicine = medicine;
        }

    }
}
