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
        //Fields:
        private string _name;
        private MedicineStatus _medicineStatus;
        private ObservableCollection<Ingredient> _ingredients;

        public string MedicineName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public MedicineStatus MedicineStatus
        {
            get { return _medicineStatus; }
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

        public AddOrEditMedicine()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Title = "Add medicine";

            MedicineStatus = MedicineStatus.STAGED;
        }

        public AddOrEditMedicine(Medicine m)
        {
            InitializeComponent();
            this.DataContext = this;

            this.Title = "Edit medicine";

            this.MedicineName = m.MedicineName;
            this.MedicineStatus = m.Status;
            this.Ingredients = new ObservableCollection<Ingredient>(m.Ingredients);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
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
        }
    }
}
