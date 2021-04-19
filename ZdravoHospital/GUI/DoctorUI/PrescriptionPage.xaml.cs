using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PrescriptionPage.xaml
    /// </summary>
    public partial class PrescriptionPage : Page, INotifyPropertyChanged
    {
        public Prescription Prescription { get; set; }
        private Period period;
        private Patient patient;
        private Therapy therapy;

        public PrescriptionPage(Period period)
        {
            InitializeComponent();
            
            this.period = period;
            this.patient = Model.Resources.patients[period.PatientUsername];

            if (period.PrescriptionId == -1)
            {
                Prescription = new Prescription();
                Prescription.DoctorUsername = period.DoctorUsername;
                Prescription.Id = Model.Resources.patients[period.PatientUsername].Prescriptions.Count;
            }
            else
            {
                foreach (Prescription prescription in patient.Prescriptions)
                    if (prescription.Id == period.PrescriptionId)
                        this.Prescription = prescription;
            }

            TherapiesListView.ItemsSource = Prescription.TherapyList;
            Model.Resources.OpenMedicines();
            MedicinesComboBox.ItemsSource = Model.Resources.medicines;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            NewTherapyPopup.Visibility = Visibility.Visible;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (period.PrescriptionId != -1) // ako se edituje
                foreach (Prescription prescription in patient.Prescriptions)
                    if (prescription.Id == period.PrescriptionId)
                    {
                        patient.Prescriptions.Remove(prescription);
                        break;
                    }

            patient.Prescriptions.Add(Prescription);
            period.PrescriptionId = Prescription.Id;

            Model.Resources.SavePatients();
            Model.Resources.SavePeriods();

            MessageBox.Show("Prescription successfully saved.", "Success");

            NavigationService.GoBack();
        }

        private void CancelTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            NewTherapyPopup.Visibility = Visibility.Hidden;
        }

        private void ConfirmTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
                return;

            string time = StartHoursTextBox.Text;
            string[] parts = time.Split(':');
            int hours = Int32.Parse(parts[0]);
            int minutes = Int32.Parse(parts[1]);
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, 0);
            DateTime end = new DateTime(EndDatePicker.SelectedDate.Value.Year,
                                    EndDatePicker.SelectedDate.Value.Month,
                                    EndDatePicker.SelectedDate.Value.Day);

            therapy = new Therapy()
            {
                Medicine = MedicinesComboBox.SelectedItem as Medicine,
                StartHours = start,
                TimesPerDay = Int32.Parse(TimesPerDayTextBox.Text),
                PauseInDays = Int32.Parse(PauseInDaysTextBox.Text),
                EndDate = end
            };

            this.Prescription.TherapyList.Add(therapy);
            CollectionViewSource.GetDefaultView(TherapiesListView.ItemsSource).Refresh();

            NewTherapyPopup.Visibility = Visibility.Hidden;
        }

        private bool IsInputValid()
        {
            if (MedicinesComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select medicine.", "Invalid input");
                return false;
            }

            Regex regex = new Regex(@"^\d{2}:\d{2}$");

            if (!regex.IsMatch(StartHoursTextBox.Text))
            {
                MessageBox.Show("Please enter start hours parameter in correct format (HH:mm).", "Invalid input");
                return false;
            }

            regex = new Regex(@"^\d+$");

            if (!regex.IsMatch(TimesPerDayTextBox.Text))
            {
                MessageBox.Show("Please enter times per day parameter in correct format (numbers only).", "Invalid input");
                return false;
            }

            if (!regex.IsMatch(PauseInDaysTextBox.Text))
            {
                MessageBox.Show("Please enter pause in days parameter in correct format (numbers only).", "Invalid input");
                return false;
            }

            return true;
        }

        private void MedicinesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Medicine medicine = MedicinesComboBox.SelectedItem as Medicine;

            foreach (Medicine medicineAllergen in patient.MedicineAllergens)
                if (medicine.MedicineName.Equals(medicineAllergen.MedicineName))
                {
                    MessageBox.Show("Patient is allergic to selected medicine (" + medicine.MedicineName + ")", "Allergy detected");
                    return;
                }

            foreach (Ingredient ingredientAllergen in patient.IngredientAllergens)
                foreach (Ingredient ingredient in medicine.Ingredients)
                    if (ingredient.IngredientName.Equals(ingredientAllergen.IngredientName))
                    {
                        MessageBox.Show("Patient is allergic to an ingredient in selected medicine (" + ingredient.IngredientName + ")", "Allergy detected");
                        return;
                    }
        }
    }
}
