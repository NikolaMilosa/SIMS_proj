﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.View;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class AddOrEditMedicineDialogViewModel : ViewModel
    {
        #region Fields

        private string _title;

        private Medicine _medicine;
        private bool _isAdder;

        private Window _dialog;
        private MedicineService _medicineService;
        private Medicine _passedMedicine;

        private bool _canEdit;

        private InjectorDTO _injector;
        
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

        public Medicine Medicine
        {
            get => _medicine;
            set
            {
                _medicine = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsAdder
        {
            get => _isAdder;
            set
            {
                _isAdder = value;
                OnPropertyChanged();
            }
        }

        public bool CanEdit
        {
            get => _canEdit;
            set
            {
                _canEdit = value;
                OnPropertyChanged();
            }
        }

        public Ingredient Ingredient { get; set; }

        public ObservableCollection<Ingredient> Ingredients { get; set; }
        #endregion

        #region Commands

        public MyICommand AddIngredientCommand { get; set; }
        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public AddOrEditMedicineDialogViewModel(Medicine? medicine, InjectorDTO injector)
        {
            if (medicine == null)
            {
                Title = "Adding medicine";
                IsAdder = true;
                Medicine = new Medicine();
                Ingredients = new ObservableCollection<Ingredient>();
                CanEdit = true;
            }
            else
            {
                Title = "Editing medicine";
                IsAdder = false;
                Medicine = new Medicine(medicine);
                _passedMedicine = medicine;
                Ingredients = new ObservableCollection<Ingredient>(medicine.Ingredients);

                CanEdit = true;

                if (medicine.Status == MedicineStatus.PENDING || medicine.Status == MedicineStatus.APPROVED)
                    CanEdit = false;
            }

            _injector = injector;

            AddIngredientCommand = new MyICommand(OnAddIngredient, CanAddIngredient);
            ConfirmCommand = new MyICommand(OnConfirm, CanConfirm);

            _medicineService = new MedicineService(null, injector);
        }

        #region Command functions

        private void OnAddIngredient()
        {
            Medicine.Ingredients = new List<Ingredient>(Ingredients);
            _dialog = new AddOrEditIngredientDialog(Medicine, null, this, _injector);
            _dialog.ShowDialog();
        }

        private bool CanAddIngredient()
        {
            return CanEdit;
        }

        private void OnConfirm()
        {
            if (IsAdder)
            {
                _medicineService.AddNewMedicine(Medicine);
            }
            else
            {
                _medicineService.EditMedicine(Medicine);
            }
        }

        private bool CanConfirm()
        {
            return CanEdit;
        }

        #endregion

        #region Public functions

        public int GetIngredientsCount()
        {
            return Medicine.Ingredients.Count;
        }

        public void HandleEnter()
        {
            if (Ingredient != null)
            {
                _dialog = new AddOrEditIngredientDialog(Medicine, Ingredient, this, _injector);
                _dialog.ShowDialog();
            }
        }

        public void HandleDelete()
        {
            if (Ingredient != null)
            {
                _dialog = new WarningDialog(_injector, Ingredient, this, Medicine);
                _dialog.ShowDialog();
            }
        }

        #endregion

        #region Events

        public void OnIngredientChanged(object sender, EventArgs e)
        {
            Ingredients = new ObservableCollection<Ingredient>(Medicine.Ingredients);
            OnPropertyChanged("Ingredients");
        }

        #endregion
    }
}
