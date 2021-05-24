using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class AddOrEditIngredientDialogViewModel : ViewModel
    {
        #region Fields

        private string _title;
        
        private Ingredient _ingredient;
        private Ingredient _passedIngredient;
        private Medicine _medicine;
        private AddOrEditMedicineDialogViewModel _activeDialog;

        private bool _isAdder;

        private MedicineService _medicineService;

        #endregion

        #region Properties

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public Ingredient Ingredient
        {
            get => _ingredient;
            set
            {
                _ingredient = value;
                OnPropertyChanged();
            }
        }

        public Medicine Medicine
        {
            get => _medicine;
        }

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public AddOrEditIngredientDialogViewModel(Medicine medicine, Ingredient passedIngredient, AddOrEditMedicineDialogViewModel activeDialog, InjectorDTO injector)
        {
            _medicine = medicine;
            _activeDialog = activeDialog;
            _passedIngredient = passedIngredient;

            if (passedIngredient == null)
            {
                Title = "Adding ingredient";
                _ingredient = new Ingredient("");
                _isAdder = true;
            }
            else
            {
                Title = "Editing ingredient";
                _ingredient = new Ingredient(passedIngredient.IngredientName);
                _isAdder = false;
            }

            ConfirmCommand = new MyICommand(OnConfirm);

            _medicineService = new MedicineService(activeDialog, injector);
        }

        public void OnConfirm()
        {
            if (_isAdder)
            {
                _medicineService.AddIngredientToMedicine(_ingredient, _medicine);
            }
            else
            {
                _medicineService.EditIngredientInMedicine(_passedIngredient, _ingredient, _medicine);
            }
        }
    }
}
