using Model;
using Repository.MedicinePersistance;
using Repository.PatientPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;
using ZdravoHospital.Repository.IngredientPersistance;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for AllergiesPage.xaml
    /// </summary>
    public partial class AllergiesPage : Page
    {
        public AllergiesService AllergiesService { get; set; }
        public ObservableCollection<string> MedicalAllergens { get; set; }
        public ObservableCollection<string> IngredientAllergens { get; set; }
        public Patient SelectedPatient { get; set; }
        public string SelectedMedicalAllergen { get; set; }
        public string SelectedIngredientAllergen { get; set; }
        public string CustomAllergen { get; set; }
        public bool IsCustomAllergenMedicine { get; set; }
        public bool IsCustomAllergenIngredient { get; set; }
        public ObservableCollection<string> AddedMedicalAllergens { get; set; }
        public ObservableCollection<string> AddedIngredientAllergens { get; set; }
        public ObservableCollection<string> AddedCustomAllergens { get; set; }

        public AllergiesPage(Model.Patient selectedPatient)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedPatient = selectedPatient;


            IIngredientRepository ingredientRepository = RepositoryFactory.CreateIngredientRepository();
            IMedicineRepository medicineRepository = RepositoryFactory.CreateMedicineRepository();
            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();

            AllergiesService = new AllergiesService(ingredientRepository, medicineRepository, patientRepository);
            AddedCustomAllergens = new ObservableCollection<string>();

            initAddedIngredientAllergens();
            initAddedMedicalAllergens();
            medicineListToStringList(AllergiesService.GetAllMedicines());
            ingredientListToStringList(AllergiesService.GetAllIngredients());

            ICollectionView viewMedical = (ICollectionView)CollectionViewSource.GetDefaultView(MedicalAllergens);
            ICollectionView viewIngredient = (ICollectionView)CollectionViewSource.GetDefaultView(IngredientAllergens);

            viewMedical.Filter = UserFilterMedicalAllergens;
            viewIngredient.Filter = UserFilterIngredientAllergens;
        }

        private void initAddedMedicalAllergens()
        {
            AddedMedicalAllergens = new ObservableCollection<string>();
            if (SelectedPatient.MedicineAllergens != null)
            {
                foreach(var allergen in SelectedPatient.MedicineAllergens)
                {
                    AddedMedicalAllergens.Add(allergen);
                }
            }
        }
        private void initAddedIngredientAllergens()
        {
            AddedIngredientAllergens = new ObservableCollection<string>();
            if (SelectedPatient.IngredientAllergens != null)
            {
                foreach (var allergen in SelectedPatient.IngredientAllergens)
                {
                    AddedIngredientAllergens.Add(allergen);
                }
            }
        }
        private void medicineListToStringList(List<Model.Medicine> meds)
        {
            this.MedicalAllergens = new ObservableCollection<string>();
            foreach(var med in meds)
            {
                this.MedicalAllergens.Add(med.MedicineName);
            }
        }
        private void ingredientListToStringList(List<Model.Ingredient> ings)
        {
            this.IngredientAllergens = new ObservableCollection<string>();
            foreach (var ing in ings)
            {
                this.IngredientAllergens.Add(ing.IngredientName);
            }
        }

        private bool UserFilterMedicalAllergens(object item)
        {
            if (String.IsNullOrEmpty(SearchMedicalAllergensTextBox.Text))
                return true;
            else
                return ((item as string).IndexOf(SearchMedicalAllergensTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool UserFilterIngredientAllergens(object item)
        {
            if (String.IsNullOrEmpty(SearchIngredientAllergensTextBox.Text))
                return true;
            else
                return ((item as string).IndexOf(SearchIngredientAllergensTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void SearchMedicalAllergensTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(MedicalAllergensListBox.ItemsSource).Refresh();
        }

        private void SearchIngredientAllergensTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(IngredientAllergensListBox.ItemsSource).Refresh();
        }

        private void AddMedicalAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedMedicalAllergen != null)
            {
                bool success = AllergiesService.AddMedicalAllergen(SelectedPatient, SelectedMedicalAllergen);
                if(success)
                    AddedMedicalAllergens.Add(SelectedMedicalAllergen);
                else
                {
                    SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Sorry", "Medicine already exists.");
                    SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                    SecretaryWindowVM.CustomMessageBox.Show();
                }
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select allergen first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }

        private void AddIngredientAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedIngredientAllergen != null)
            {
                bool success = AllergiesService.AddIngredientAllergen(SelectedPatient, SelectedIngredientAllergen);
                if (success)
                    AddedIngredientAllergens.Add(SelectedIngredientAllergen);
                else
                {
                    SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Sorry", "Ingredient already exists.");
                    SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                    SecretaryWindowVM.CustomMessageBox.Show();
                }
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select allergen first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }

        private void AddCustomAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsCustomAllergenMedicine && CustomAllergen != String.Empty)
            {
                bool success = AllergiesService.AddMedicalAllergen(SelectedPatient, CustomAllergen);
                if (success)
                    AddedCustomAllergens.Add(CustomAllergen);
            }
            else if (IsCustomAllergenIngredient && CustomAllergen != String.Empty)
            {
                bool success = AllergiesService.AddIngredientAllergen(SelectedPatient, CustomAllergen);
                if (success)
                    AddedCustomAllergens.Add(CustomAllergen);
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Write allergen first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PatientDetailsPage(SelectedPatient));
        }

        private void RemoveMedicalAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddedMedicalAllergensListBox.SelectedItem != null)
            {
                string selectedAllergen = AddedMedicalAllergensListBox.SelectedItem as string;
                bool success = AllergiesService.RemoveMedicineAllergen(SelectedPatient, selectedAllergen);
                if(success)
                    AddedMedicalAllergens.Remove(selectedAllergen);
                
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select allergen first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }

        private void RemoveIngredientAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddedIngredientAllergensListBox.SelectedItem != null)
            {
                string selectedAllergen = AddedIngredientAllergensListBox.SelectedItem as string;
                bool success = AllergiesService.RemoveIngredientAllergen(SelectedPatient, selectedAllergen);
                if (success)
                    AddedIngredientAllergens.Remove(selectedAllergen);
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select allergen first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }

        private void RemoveCustomAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddedCustomAllergensListBox.SelectedItem != null)
            {
                string selectedAllergen = AddedCustomAllergensListBox.SelectedItem as string;
                bool isAllergenMedicine = AllergiesService.IsAllergenMedicine(SelectedPatient, selectedAllergen);
                if (isAllergenMedicine)
                    AllergiesService.RemoveMedicineAllergen(SelectedPatient, selectedAllergen);
                else
                    AllergiesService.RemoveIngredientAllergen(SelectedPatient, selectedAllergen);

                AddedCustomAllergens.Remove(selectedAllergen);
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select allergen first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }
    }
}
