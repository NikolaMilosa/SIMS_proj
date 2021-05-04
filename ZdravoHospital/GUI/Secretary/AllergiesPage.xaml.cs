using Model;
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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for AllergiesPage.xaml
    /// </summary>
    public partial class AllergiesPage : Page
    {
        public ObservableCollection<string> MedicalAllergens { get; set; }
        public ObservableCollection<string> IngredientAllergens { get; set; }
        public Patient SelectedPatient { get; set; }
        public string CustomAllergen { get; set; }
        public ObservableCollection<string> AddedMedicalAllergens { get; set; }
        public ObservableCollection<string> AddedIngredientAllergens { get; set; }
        public ObservableCollection<string> AddedCustomAllergens { get; set; }

        public AllergiesPage(Model.Patient selectedPatient)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedPatient = selectedPatient;
            Model.Resources.OpenMedicines();
            Model.Resources.OpenIngredients();
            AddedMedicalAllergens = new ObservableCollection<string>();
            AddedIngredientAllergens = new ObservableCollection<string>();
            AddedCustomAllergens = new ObservableCollection<string>();

            initAddedIngredientAllergens();
            initAddedMedicalAllergens();

            //var logFileMedical = File.ReadAllLines(@"..\..\..\Resources\drugs.txt");
            //MedicalAllergens = new ObservableCollection<string>(logFileMedical);
            //var logFileIngredient = File.ReadAllLines(@"..\..\..\Resources\ingredients.txt");
            //IngredientAllergens = new ObservableCollection<string>(logFileIngredient);
            //MedicalAllergens = new ObservableCollection<Model.Medicine>(Model.Resources.medicines);
            //IngredientAllergens = new ObservableCollection<Model.Ingredient>(Model.Resources.ingredients);
            medicineListToStringList(Model.Resources.medicines);
            ingredientListToStringList(Model.Resources.ingredients);

            ICollectionView viewMedical = (ICollectionView)CollectionViewSource.GetDefaultView(MedicalAllergens);
            ICollectionView viewIngredient = (ICollectionView)CollectionViewSource.GetDefaultView(IngredientAllergens);

            viewMedical.Filter = UserFilterMedicalAllergens;
            viewIngredient.Filter = UserFilterIngredientAllergens;
        }

        private void initAddedMedicalAllergens()
        {
            if(SelectedPatient.MedicineAllergens != null)
            {
                foreach(var allergen in SelectedPatient.MedicineAllergens)
                {
                    AddedMedicalAllergens.Add(allergen);
                }
            }
        }
        private void initAddedIngredientAllergens()
        {
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

        private bool isMedicineAllergenUnique(List<string> allergens, Medicine newAllergen)
        {
            foreach(var allergen in allergens)
            {
                if (allergen.Equals(newAllergen.MedicineName))
                    return false;
            }
            return true;
        }
        private bool isIngredientAllergenUnique(List<string> allergens, Ingredient newAllergen)
        {
            foreach (var allergen in allergens)
            {
                if (allergen.Equals(newAllergen.IngredientName))
                    return false;
            }
            return true;
        }

        private void AddMedicalAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if(MedicalAllergensListBox.SelectedItem != null)
            {
                var selectedAllergenName = (MedicalAllergensListBox.SelectedItem) as string;
                Medicine medicalAllergen = null;
                foreach (var med in Model.Resources.medicines)
                {
                    if (med.MedicineName.Equals(selectedAllergenName))
                        medicalAllergen = med;
                }
                

                if (SelectedPatient.MedicineAllergens == null)
                {
                    SelectedPatient.MedicineAllergens = new List<string>();
                }

                if (isMedicineAllergenUnique(SelectedPatient.MedicineAllergens, medicalAllergen))
                {
                    SelectedPatient.MedicineAllergens.Add(medicalAllergen.MedicineName);
                    AddedMedicalAllergens.Add(medicalAllergen.MedicineName);

                    ///////////// UPDATING THE RESOURCES ////////////////
                    if (File.Exists(@"..\..\..\Resources\patients.json"))
                    {
                        Model.Resources.OpenPatients();
                        foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                        {
                            if (item.Key.Equals(SelectedPatient.Username))
                            {
                                Model.Resources.patients[item.Key] = SelectedPatient;
                                break;
                            }
                        }
                        Model.Resources.SavePatients();

                        //////////// FEEDBACK MESSAGE //////////////////////////

                    }
                }
                else
                {
                    MessageBox.Show("Medical allergen already added.");
                }
                
            }
        }

        private void AddIngredientAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientAllergensListBox.SelectedItem != null)
            {
                var selectedAllergen = (IngredientAllergensListBox.SelectedItem) as string;
                Ingredient ingredientAllergen = new Ingredient(selectedAllergen);

                if (SelectedPatient.IngredientAllergens == null)
                {
                    SelectedPatient.IngredientAllergens = new List<string>();
                }

              
                if(isIngredientAllergenUnique(SelectedPatient.IngredientAllergens, ingredientAllergen))
                {
                    SelectedPatient.IngredientAllergens.Add(ingredientAllergen.IngredientName);
                    AddedIngredientAllergens.Add(ingredientAllergen.IngredientName);

                    ///////////// UPDATING THE RESOURCES ////////////////
                    if (File.Exists(@"..\..\..\Resources\patients.json"))
                    {
                        Model.Resources.OpenPatients();
                        foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                        {
                            if (item.Key.Equals(SelectedPatient.Username))
                            {
                                Model.Resources.patients[item.Key] = SelectedPatient;
                                break;
                            }
                        }
                        Model.Resources.SavePatients();

                        //////////// FEEDBACK MESSAGE //////////////////////////

                    }
                }
                else
                {
                    MessageBox.Show("Ingredient allergen already added.");
                }
                
            }
        }

        private void AddCustomAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)MedicalRadioButton.IsChecked && CustomAllergenTextBox.Text != String.Empty)
            {
                Medicine medicalAllergen = new Medicine(CustomAllergenTextBox.Text);

                if (SelectedPatient.MedicineAllergens == null)
                {
                    SelectedPatient.MedicineAllergens = new List<string>();
                }

                if (isMedicineAllergenUnique(SelectedPatient.MedicineAllergens, medicalAllergen))
                {
                    SelectedPatient.MedicineAllergens.Add(medicalAllergen.MedicineName);
                    AddedCustomAllergens.Add(medicalAllergen.MedicineName);

                    ///////////// UPDATING THE RESOURCES ////////////////
                    if (File.Exists(@"..\..\..\Resources\patients.json"))
                    {
                        Model.Resources.OpenPatients();
                        foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                        {
                            if (item.Key.Equals(SelectedPatient.Username))
                            {
                                Model.Resources.patients[item.Key] = SelectedPatient;
                                break;
                            }
                        }
                        Model.Resources.SavePatients();

                        //////////// FEEDBACK MESSAGE //////////////////////////

                    }
                }
                else
                {
                    MessageBox.Show("Medical allergen already added.");
                }
            }
            else if ((bool)IngredientRadioButton.IsChecked && CustomAllergenTextBox.Text != String.Empty)
            {
                Ingredient ingredientAllergen = new Ingredient(CustomAllergenTextBox.Text);

                if (SelectedPatient.IngredientAllergens == null)
                {
                    SelectedPatient.IngredientAllergens = new List<string>();
                }

                if(isIngredientAllergenUnique(SelectedPatient.IngredientAllergens, ingredientAllergen))
                {
                    SelectedPatient.IngredientAllergens.Add(ingredientAllergen.IngredientName);
                    AddedCustomAllergens.Add(ingredientAllergen.IngredientName);

                    ///////////// UPDATING THE RESOURCES ////////////////
                    if (File.Exists(@"..\..\..\Resources\patients.json"))
                    {
                        Model.Resources.OpenPatients();
                        foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                        {
                            if (item.Key.Equals(SelectedPatient.Username))
                            {
                                Model.Resources.patients[item.Key] = SelectedPatient;
                                break;
                            }
                        }
                        Model.Resources.SavePatients();

                        //////////// FEEDBACK MESSAGE //////////////////////////

                    }
                }
                else
                {
                    MessageBox.Show("Ingredient allergen already added.");
                }
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
                SelectedPatient.MedicineAllergens.Remove((string)AddedMedicalAllergensListBox.SelectedItem);
                AddedMedicalAllergens.Remove((string)AddedMedicalAllergensListBox.SelectedItem);
                if (File.Exists(@"..\..\..\Resources\patients.json"))
                {
                    Model.Resources.OpenPatients();
                    foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                    {
                        if (item.Key.Equals(SelectedPatient.Username))
                        {
                            Model.Resources.patients[item.Key] = SelectedPatient;
                            break;
                        }
                    }
                    Model.Resources.SavePatients();

                    if (CollectionViewSource.GetDefaultView(AddedMedicalAllergensListBox.ItemsSource) != null)
                        CollectionViewSource.GetDefaultView(AddedMedicalAllergensListBox.ItemsSource).Refresh();
                }
            }
        }

        private void RemoveIngredientAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddedIngredientAllergensListBox.SelectedItem != null)
            {
                SelectedPatient.IngredientAllergens.Remove((string)AddedIngredientAllergensListBox.SelectedItem);
                AddedIngredientAllergens.Remove((string)AddedIngredientAllergensListBox.SelectedItem);
                if (File.Exists(@"..\..\..\Resources\patients.json"))
                {
                    Model.Resources.OpenPatients();
                    foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                    {
                        if (item.Key.Equals(SelectedPatient.Username))
                        {
                            Model.Resources.patients[item.Key] = SelectedPatient;
                            break;
                        }
                    }
                    Model.Resources.SavePatients();
                    if (CollectionViewSource.GetDefaultView(AddedIngredientAllergensListBox.ItemsSource) != null)
                        CollectionViewSource.GetDefaultView(AddedIngredientAllergensListBox.ItemsSource).Refresh();
                }
            }
        }

        private void RemoveCustomAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddedCustomAllergensListBox.SelectedItem != null)
            {
                SelectedPatient.IngredientAllergens.Remove((string)AddedCustomAllergensListBox.SelectedItem);
                SelectedPatient.MedicineAllergens.Remove((string)AddedCustomAllergensListBox.SelectedItem);
                AddedCustomAllergens.Remove((string)AddedCustomAllergensListBox.SelectedItem);
                if (File.Exists(@"..\..\..\Resources\patients.json"))
                {
                    Model.Resources.OpenPatients();
                    foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                    {
                        if (item.Key.Equals(SelectedPatient.Username))
                        {
                            Model.Resources.patients[item.Key] = SelectedPatient;
                            break;
                        }
                    }
                    Model.Resources.SavePatients();
                    if (CollectionViewSource.GetDefaultView(AddedCustomAllergensListBox.ItemsSource) != null)
                        CollectionViewSource.GetDefaultView(AddedCustomAllergensListBox.ItemsSource).Refresh();
                }
            }
        }
    }
}
