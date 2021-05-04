using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for IngredientEnteringDialog.xaml
    /// </summary>
    public partial class IngredientEnteringDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<Ingredient> ExistingNames { get; set; }

        private string _enteredName;

        public string EnteredName
        {
            get => _enteredName;
            set
            {
                _enteredName = value;
                OnPropertyChanged("EnteredName");
            }
        }

        //Helper
        ObservableCollection<Ingredient> _viewableIngredients;
        Ingredient _passedIngredient;
        bool _isAdder;

        public IngredientEnteringDialog(List<Ingredient> en, ObservableCollection<Ingredient> oc)
        {
            InitializeComponent();
            this.DataContext = this;

            ExistingNames = en;

            this._viewableIngredients = oc;

            this._isAdder = true;
        }

        public IngredientEnteringDialog(List<Ingredient> en, ObservableCollection<Ingredient> oc, Ingredient i)
        {
            InitializeComponent();
            this.DataContext = this;

            ExistingNames = en;

            this._viewableIngredients = oc;
            this._passedIngredient = i;

            this._isAdder = false;

            EnteredName = i.IngredientName;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var medicineFunctions = new Logics.MedicineFunctions();
            if (_isAdder)
            {
                var i = new Ingredient(EnteredName.Trim().ToLower());
                medicineFunctions.AddIngredientToMedicine(i, ExistingNames, _viewableIngredients);
            }
            else
            {
                if (!_passedIngredient.IngredientName.Equals(EnteredName.Trim().ToLower()))
                {
                    medicineFunctions.EditIngredientInMedicine(_passedIngredient, EnteredName, ExistingNames, _viewableIngredients);
                }   
            }
            
            this.Close();
        }
    }
}
