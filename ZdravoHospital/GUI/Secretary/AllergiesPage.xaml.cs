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

        public AllergiesPage(Model.Patient selectedPatient)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedPatient = selectedPatient;

            var logFileMedical = File.ReadAllLines(@"..\..\..\Resources\drugs.txt");
            MedicalAllergens = new ObservableCollection<string>(logFileMedical);
            var logFileIngredient = File.ReadAllLines(@"..\..\..\Resources\ingredients.txt");
            IngredientAllergens = new ObservableCollection<string>(logFileIngredient);

            ICollectionView viewMedical = (ICollectionView)CollectionViewSource.GetDefaultView(MedicalAllergens);
            ICollectionView viewIngredient = (ICollectionView)CollectionViewSource.GetDefaultView(IngredientAllergens);

            viewMedical.Filter = UserFilterMedicalAllergens;
            viewIngredient.Filter = UserFilterIngredientAllergens;
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

        private bool isMedicineAllergenUnique(List<Medicine> allergens, Medicine newAllergen)
        {
            foreach(var allergen in allergens)
            {
                if (allergen.MedicineName.Equals(newAllergen.MedicineName))
                    return false;
            }
            return true;
        }
        private bool isIngredientAllergenUnique(List<Ingredient> allergens, Ingredient newAllergen)
        {
            foreach (var allergen in allergens)
            {
                if (allergen.IngredientName.Equals(newAllergen.IngredientName))
                    return false;
            }
            return true;
        }

        private void AddMedicalAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if(MedicalAllergensListBox.SelectedItem != null)
            {
                var selectedAllergen = (string)MedicalAllergensListBox.SelectedItem;
                Medicine medicalAllergen = new Medicine(selectedAllergen);

                if (SelectedPatient.MedicineAllergens == null)
                {
                    SelectedPatient.MedicineAllergens = new List<Medicine>();
                }

                if (isMedicineAllergenUnique(SelectedPatient.MedicineAllergens, medicalAllergen))
                {
                    SelectedPatient.MedicineAllergens.Add(medicalAllergen);

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
                        Random r = new Random();
                        Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                        MedicalSuccessLabel.Foreground = brush;
                        MedicalSuccessLabel.Content = "Added successfully.";
                        MedicalSuccessLabel.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    Random r = new Random();
                    Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                    MedicalSuccessLabel.Foreground = brush;
                    MedicalSuccessLabel.Content = "Already exists.";
                    MedicalSuccessLabel.Visibility = Visibility.Visible;
                }
                
            }

        }

        private void AddIngredientAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientAllergensListBox.SelectedItem != null)
            {
                var selectedAllergen = (string)IngredientAllergensListBox.SelectedItem;
                Ingredient ingredientAllergen = new Ingredient(selectedAllergen);

                if (SelectedPatient.IngredientAllergens == null)
                {
                    SelectedPatient.IngredientAllergens = new List<Ingredient>();
                }

                if(isIngredientAllergenUnique(SelectedPatient.IngredientAllergens, ingredientAllergen))
                {
                    SelectedPatient.IngredientAllergens.Add(ingredientAllergen);

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
                        Random r = new Random();
                        Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                        IngredientSuccessLabel.Foreground = brush;
                        IngredientSuccessLabel.Content = "Added successfully.";
                        IngredientSuccessLabel.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    Random r = new Random();
                    Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                    IngredientSuccessLabel.Foreground = brush;
                    IngredientSuccessLabel.Content = "Already exists.";
                    IngredientSuccessLabel.Visibility = Visibility.Visible;
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
                    SelectedPatient.MedicineAllergens = new List<Medicine>();
                }

                if (isMedicineAllergenUnique(SelectedPatient.MedicineAllergens, medicalAllergen))
                {
                    SelectedPatient.MedicineAllergens.Add(medicalAllergen);

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
                        Random r = new Random();
                        Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                        CustomSuccessLabel.Foreground = brush;
                        CustomSuccessLabel.Content = "Added successfully.";
                        CustomSuccessLabel.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    Random r = new Random();
                    Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                    CustomSuccessLabel.Foreground = brush;
                    CustomSuccessLabel.Content = "Already exists.";
                    CustomSuccessLabel.Visibility = Visibility.Visible;
                }
            }
            else if ((bool)IngredientRadioButton.IsChecked && CustomAllergenTextBox.Text != String.Empty)
            {
                Ingredient ingredientAllergen = new Ingredient(CustomAllergenTextBox.Text);

                if (SelectedPatient.IngredientAllergens == null)
                {
                    SelectedPatient.IngredientAllergens = new List<Ingredient>();
                }

                if(isIngredientAllergenUnique(SelectedPatient.IngredientAllergens, ingredientAllergen))
                {
                    SelectedPatient.IngredientAllergens.Add(ingredientAllergen);

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
                        Random r = new Random();
                        Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                        CustomSuccessLabel.Foreground = brush;
                        CustomSuccessLabel.Content = "Added successfully.";
                        CustomSuccessLabel.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    Random r = new Random();
                    Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(100, 233)));
                    CustomSuccessLabel.Foreground = brush;
                    CustomSuccessLabel.Content = "Already exists.";
                    CustomSuccessLabel.Visibility = Visibility.Visible;
                }
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PatientsView());
        }
    }
}
