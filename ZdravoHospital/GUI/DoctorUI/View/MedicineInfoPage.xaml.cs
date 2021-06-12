using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for MedicineInfoPage.xaml
    /// </summary>
    public partial class MedicineInfoPage : Page, INotifyPropertyChanged
    {
        private MedicineService _medicineService;
        private MedicineRecensionService _medicineRecensionService;
        private IngredientService _ingredientService;
        private TextBlock relevantTextBlock;
        public Medicine Medicine { get; set; }
        public double NameSupplierWidth { get; set; }
        public Thickness TopSectionsMargin { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public ObservableCollection<Ingredient> AvailableIngredients { get; set; }
        public ObservableCollection<string> Replacements { get; set; }
        public ObservableCollection<string> AvailableReplacements { get; set; }

        public MedicineInfoPage(Medicine medicine)
        {
            InitializeComponent();

            DataContext = this;
            Medicine = medicine;
            _medicineService = new MedicineService();
            _medicineRecensionService = new MedicineRecensionService();
            _ingredientService = new IngredientService();

            if (NameTextBlock.Width > StatusTextBlock.Width)
                relevantTextBlock = NameTextBlock;
            else
                relevantTextBlock = StatusTextBlock;

            if (medicine.Status == MedicineStatus.REJECTED)
                RejectButton.IsEnabled = false;
            else if (medicine.Status == MedicineStatus.APPROVED)
                ApproveButton.IsEnabled = false;

            AvailableIngredients = _medicineService.GetAvailableIngredients(medicine);
            AvailableIngredientsListBox.ItemsSource = AvailableIngredients;

            Ingredients = new ObservableCollection<Ingredient>();

            foreach (Ingredient ingredient in medicine.Ingredients)
                Ingredients.Add(ingredient);

            AvailableReplacements = _medicineService.GetAvailableReplacements(medicine);
            AvailableReplacementsListBox.ItemsSource = AvailableReplacements;

            Replacements = new ObservableCollection<string>();

            foreach (string replacement in medicine.Replacements)
                Replacements.Add(replacement);

            NotesTextBox.Text = medicine.Note;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopSectionsMargin = new Thickness(this.ActualWidth * 0.075, 0, this.ActualWidth * 0.075, 20);
            OnPropertyChanged("TopSectionsMargin");

            double calculatedWidth = this.ActualWidth * 0.7  - 100 - 250; // BackButton width = 100 and status part width = 250, margins take 0.3 left 
            
            if (calculatedWidth > relevantTextBlock.ActualWidth)
                NameSupplierWidth = calculatedWidth;
            else
                NameSupplierWidth = relevantTextBlock.ActualWidth;
            
            if (NameSupplierWidth > ActualWidth)
                NameSupplierWidth = ActualWidth;

            OnPropertyChanged("NameSupplierWidth");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            StatusGrid.Visibility = Visibility.Collapsed;
            RejectionPopUp.Visibility = Visibility.Visible;
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            Medicine.Status = MedicineStatus.APPROVED;
            OnPropertyChanged("Medicine");
            ApproveButton.IsEnabled = false;
            RejectButton.IsEnabled = true;

            _medicineService.UpdateMedicine(Medicine);
            _medicineRecensionService.ApproveMedicine(Medicine.MedicineName);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            StatusGrid.Visibility = Visibility.Collapsed;
            EditButton.Visibility = Visibility.Collapsed;
            ConfirmChangesButton.Visibility = Visibility.Visible;
            IngredientsGrid.RowDefinitions[2].Height = GridLength.Auto;
            ReplacementsGrid.RowDefinitions[2].Height = GridLength.Auto;
            NotesGrid.RowDefinitions[0].Height = GridLength.Auto;
            NotesGrid.RowDefinitions[1].Height = GridLength.Auto;

            MedicineNameTextBox.Text = Medicine.MedicineName;
            SupplierTextBox.Text = Medicine.Supplier;
        }

        private void ConfirmChangesButton_Click(object sender, RoutedEventArgs e)
        {
            StatusGrid.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Visible;
            ConfirmChangesButton.Visibility = Visibility.Collapsed;
            IngredientsGrid.RowDefinitions[2].Height = new GridLength(0);
            ReplacementsGrid.RowDefinitions[2].Height = new GridLength(0);
            NotesGrid.RowDefinitions[0].Height = new GridLength(0);
            NotesGrid.RowDefinitions[1].Height = new GridLength(0);

            Medicine.Ingredients = new List<Ingredient>();
            foreach (Ingredient i in IngredientsListBox.Items)
                Medicine.Ingredients.Add(i);

            Medicine.Replacements = new List<string>();
            foreach (string r in ReplacementsListBox.Items)
                Medicine.Replacements.Add(r);

            if (!Medicine.MedicineName.Equals(MedicineNameTextBox.Text))
            {
                _medicineService.RenameMedicine(Medicine.MedicineName, MedicineNameTextBox.Text);
                _medicineRecensionService.RenameMedicine(Medicine.MedicineName, MedicineNameTextBox.Text);
            }

            Medicine.MedicineName = MedicineNameTextBox.Text;
            Medicine.Supplier = SupplierTextBox.Text;
            Medicine.Note = NotesTextBox.Text;

            OnPropertyChanged("Medicine");

            _medicineService.UpdateMedicine(Medicine);
        }

        private void RemoveIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            List<Ingredient> selection = new List<Ingredient>();

            foreach (Ingredient ingredient in IngredientsListBox.SelectedItems)
                selection.Add(ingredient);

            foreach (Ingredient ingredient in selection)
            {
                Ingredients.Remove(ingredient);
                AvailableIngredients.Add(ingredient);
            }
        }

        private void AddIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            IngredientsPopUp.Visibility = Visibility.Visible;
        }

        private void RemoveReplacementsButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> selection = new List<string>();

            foreach (string replacement in ReplacementsListBox.SelectedItems)
                selection.Add(replacement);

            foreach (string replacement in selection)
            {
                Replacements.Remove(replacement);
                AvailableReplacements.Add(replacement);
            }
        }

        private void AddReplacmentsButton_Click(object sender, RoutedEventArgs e)
        {
            ReplacementsPopUp.Visibility = Visibility.Visible;
        }

        private void AddNewIngredientButton_Click(object sender, RoutedEventArgs e)
        {

            string ingredientName = IngredientTextBox.Text;

            if (ingredientName.Equals(""))
                return;
            
            if (Ingredients.ToList().Find(ing => ing.IngredientName.Equals(ingredientName)) != null)
            {
                MessageBox.Show("Already contained");
                return;
            }    

            Ingredients.Add(new Ingredient(IngredientTextBox.Text));
            IngredientTextBox.Text = "";

            Ingredient availableIngredient = AvailableIngredients.ToList().Find(ing => ing.IngredientName.Equals(ingredientName));
            if (availableIngredient != null)
                AvailableIngredients.Remove(availableIngredient);

            if (_ingredientService.GetIngredient(ingredientName) != null)
                return;

            _ingredientService.CreateNewIngredient(new Ingredient(ingredientName));
        }

        private void CancelIngredientsPopUpButton_Click(object sender, RoutedEventArgs e)
        {
            AvailableIngredientsListBox.SelectedIndex = -1;
            IngredientsPopUp.Visibility = Visibility.Collapsed;
        }

        private void AddSelectedIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            List<Ingredient> selection = new List<Ingredient>();

            foreach (Ingredient ingredient in AvailableIngredientsListBox.SelectedItems)
                selection.Add(ingredient);

            foreach (Ingredient ingredient in selection)
            {
                Ingredients.Add(ingredient);
                AvailableIngredients.Remove(ingredient);
            }

            AvailableIngredientsListBox.Items.Refresh();
            IngredientsPopUp.Visibility = Visibility.Collapsed;
        }

        private void CancelReplacementsPopUpButton_Click(object sender, RoutedEventArgs e)
        {
            AvailableReplacementsListBox.SelectedIndex = -1;
            ReplacementsPopUp.Visibility = Visibility.Collapsed;
        }

        private void AddSelectedReplacementsButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> selection = new List<string>();

            foreach (string replacement in AvailableReplacementsListBox.SelectedItems)
                selection.Add(replacement);

            foreach (string replacement in selection)
            {
                Replacements.Add(replacement);
                AvailableReplacements.Remove(replacement);
            }

            ReplacementsPopUp.Visibility = Visibility.Collapsed;
        }

        private void CancelRejectionButton_Click(object sender, RoutedEventArgs e)
        {
            RejectionPopUp.Visibility = Visibility.Collapsed;
            StatusGrid.Visibility = Visibility.Visible;
        }

        private void ConfirmRejectionButton_Click(object sender, RoutedEventArgs e)
        {
            RejectionPopUp.Visibility = Visibility.Collapsed;
            StatusGrid.Visibility = Visibility.Visible;

            Medicine.Status = MedicineStatus.REJECTED;
            OnPropertyChanged("Medicine");
            ApproveButton.IsEnabled = true;
            RejectButton.IsEnabled = false;

            _medicineService.UpdateMedicine(Medicine);
            _medicineRecensionService.RejectMedicine(Medicine.MedicineName, RecensionNoteTextBox.Text);
        }
    }
}
