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
    /// Interaction logic for AddOrEditMedicine.xaml
    /// </summary>
    public partial class AddOrEditMedicine : Window, INotifyPropertyChanged
    {
        #region Fields
        //Fields
        private string _name;
        private string _supplier;
        private MedicineStatus _medicineStatus;
        private ObservableCollection<Ingredient> _ingredients;

        public string MedicineName
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                OnPropertyChanged("Supplier");
            }
        }
        public MedicineStatus MedicineStatus
        {
            get => _medicineStatus;
            set
            {
                _medicineStatus = value;
                OnPropertyChanged("MedicineStatus");
            }
        }

        public ObservableCollection<Ingredient> Ingredients
        {
            get
            {
                if (_ingredients == null)
                    _ingredients = new ObservableCollection<Ingredient>();
                return _ingredients;
            }
            set
            {
                _ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        //Helper
        Medicine _passedMedicine;
        List<Ingredient> _temporaryIngredients;
        bool _isAdder;

        public AddOrEditMedicine()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Title = "Add medicine";

            MedicineStatus = MedicineStatus.STAGED;

            this._passedMedicine = new Medicine();
            this.Ingredients = new ObservableCollection<Ingredient>();
            this._temporaryIngredients = new List<Ingredient>();

            this._isAdder = true;
        }
        
        
        public AddOrEditMedicine(Medicine m)
        {
            InitializeComponent();
            this.DataContext = this;

            this.Title = "Edit medicine";

            this.NameTextBox.IsEnabled = false;

            this.MedicineName = m.MedicineName;
            this.Supplier = m.Supplier;
            this.MedicineStatus = m.Status;
            this.Ingredients = new ObservableCollection<Ingredient>();
            this._passedMedicine = m;
            this._temporaryIngredients = new List<Ingredient>();

            m.Ingredients.ForEach(i => 
            {
                this._temporaryIngredients.Add(new Ingredient(i.IngredientName));
                this.Ingredients.Add(new Ingredient(i.IngredientName));
            });

            this._isAdder = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var medicineFunctions = new Logics.MedicineFunctions();
            var newMedicine = new Medicine() { MedicineName = MedicineName.Trim().ToLower(), Status = MedicineStatus, Note = "", Supplier = Supplier, Ingredients = _temporaryIngredients };
            if (_isAdder)
            {
                medicineFunctions.AddNewMedicine(newMedicine);
            }
            else
            {
                medicineFunctions.EditMedicine(_passedMedicine, newMedicine);
            }

            this.Close();
        }

        private void IngredientsTable_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Ingredients.Count > 0)
            {
                IngredientsTable.SelectedIndex = 0;
                IngredientsTable.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void IngredientsTable_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (IngredientsTable.SelectedIndex + 1 < IngredientsTable.Items.Count)
                {
                    IngredientsTable.SelectedIndex += 1;
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.SelectedIndex]);
                }
                else if (IngredientsTable.SelectedIndex + 1 == IngredientsTable.Items.Count)
                {
                    IngredientsTable.SelectedIndex = 0;
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.SelectedIndex]);
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (IngredientsTable.SelectedIndex - 1 >= 0)
                {
                    IngredientsTable.SelectedIndex -= 1;
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.SelectedIndex]);
                }
                else if (IngredientsTable.SelectedIndex - 1 < 0)
                {
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.Items.Count - 1]);
                    IngredientsTable.SelectedIndex = IngredientsTable.Items.Count - 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                if (ConfirmButton.IsEnabled)
                    ConfirmButton.Focus();
                else
                    CancelButton.Focus();

                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (IngredientsTable.SelectedIndex != -1)
                {
                    Window ingredientDialog = new IngredientEnteringDialog(_temporaryIngredients, Ingredients, (Ingredient)IngredientsTable.SelectedItem);
                    ingredientDialog.ShowDialog();
                }
            }
            else if (e.Key == Key.Delete)
            {
                if (IngredientsTable.SelectedIndex != -1)
                {
                    Window warningDialog = new WarningDialog((Ingredient)IngredientsTable.SelectedItem, _temporaryIngredients, Ingredients);
                    warningDialog.ShowDialog();
                }
            }
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            Window ingredientDialog = new IngredientEnteringDialog(_temporaryIngredients, Ingredients);
            ingredientDialog.ShowDialog();
        }
    }
}
