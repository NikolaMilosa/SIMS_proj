using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for MedicineInfoPage.xaml
    /// </summary>
    public partial class MedicineInfoPage : Page, INotifyPropertyChanged
    {
        private TextBlock relevantTextBlock;

        public Medicine Medicine { get; set; }
        public double NameSupplierWidth { get; set; }
        public Thickness TopSectionsMargin { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public ObservableCollection<string> Replacements { get; set; }

        public MedicineInfoPage(Medicine medicine)
        {
            InitializeComponent();

            DataContext = this;
            Medicine = medicine;

            if (NameTextBlock.Width > StatusTextBlock.Width)
                relevantTextBlock = NameTextBlock;
            else
                relevantTextBlock = StatusTextBlock;

            if (medicine.Status == MedicineStatus.REJECTED)
                RejectButton.IsEnabled = false;
            else if (medicine.Status == MedicineStatus.APPROVED)
                ApproveButton.IsEnabled = false;

            Model.Resources.OpenIngredients();
            AvailableIngredientsListBox.ItemsSource = new List<Ingredient>();

            foreach (Ingredient i in Model.Resources.ingredients)
                if (Medicine.Ingredients.Find(ing => ing.IngredientName.Equals(i.IngredientName)) == null)
                    (AvailableIngredientsListBox.ItemsSource as List<Ingredient>).Add(i);

            Ingredients = new ObservableCollection<Ingredient>();

            foreach (Ingredient ingredient in medicine.Ingredients)
                Ingredients.Add(ingredient);

            AvailableReplacementsListBox.ItemsSource = new List<string>();

            foreach (Medicine m in Model.Resources.medicines)
                if (!m.MedicineName.Equals(medicine.MedicineName) && Medicine.Replacements.Find(medicineName => medicineName.Equals(m.MedicineName)) == null)
                    (AvailableReplacementsListBox.ItemsSource as List<string>).Add(m.MedicineName);

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
            Medicine.Status = MedicineStatus.REJECTED;
            OnPropertyChanged("Medicine");
            RejectButton.IsEnabled = false;
            ApproveButton.IsEnabled = true;
            Model.Resources.SaveMedicines();
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            Medicine.Status = MedicineStatus.APPROVED;
            OnPropertyChanged("Medicine");
            ApproveButton.IsEnabled = false;
            RejectButton.IsEnabled = true;
            Model.Resources.SaveMedicines();
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

            foreach (MedicineRecension mr in Model.Resources.medicineRecensions)
                if (mr.MedicineName.Equals(Medicine.MedicineName))
                    mr.MedicineName = MedicineNameTextBox.Text;

            foreach (Medicine m in Model.Resources.medicines)
                foreach (string r in m.Replacements)
                    if (r.Equals(Medicine.MedicineName))
                    {
                        m.Replacements.Remove(r);
                        m.Replacements.Add(MedicineNameTextBox.Text);
                        break;
                    }

            Medicine.MedicineName = MedicineNameTextBox.Text;
            Medicine.Supplier = SupplierTextBox.Text;
            Medicine.Note = NotesTextBox.Text;

            OnPropertyChanged("Medicine");

            Model.Resources.SaveMedicines();
            Model.Resources.SerializeMedicineRecensions();
        }

        private void RemoveIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            List<Ingredient> selection = new List<Ingredient>();

            foreach (Ingredient ingredient in IngredientsListBox.SelectedItems)
                selection.Add(ingredient);

            foreach (Ingredient ingredient in selection)
            {
                Ingredients.Remove(ingredient);
                (AvailableIngredientsListBox.ItemsSource as List<Ingredient>).Add(ingredient);
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
                (AvailableReplacementsListBox.ItemsSource as List<string>).Add(replacement);
            }
        }

        private void AddReplacmentsButton_Click(object sender, RoutedEventArgs e)
        {
            ReplacementsPopUp.Visibility = Visibility.Visible;
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
                (AvailableIngredientsListBox.ItemsSource as List<Ingredient>).Remove(ingredient);
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
                (AvailableReplacementsListBox.ItemsSource as List<string>).Remove(replacement);
            }

            AvailableReplacementsListBox.Items.Refresh();
            ReplacementsPopUp.Visibility = Visibility.Collapsed;
        }
    }
}
