using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.View
{
    /// <summary>
    /// Interaction logic for AddOrEditIngredientDialog.xaml
    /// </summary>
    public partial class AddOrEditIngredientDialog : Window
    {
        private AddOrEditIngredientDialogViewModel currentViewModel;
        public AddOrEditIngredientDialog(Medicine medicine, Ingredient passedIngredient, AddOrEditMedicineDialogViewModel activeDialog, InjectorDTO injector)
        {
            InitializeComponent();
            currentViewModel = new AddOrEditIngredientDialogViewModel(medicine, passedIngredient, activeDialog, injector);
            this.DataContext = currentViewModel;
        }
    }
}
