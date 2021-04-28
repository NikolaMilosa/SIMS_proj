using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PrescriptionPage.xaml
    /// </summary>
    public partial class PrescriptionPage : Page, INotifyPropertyChanged
    {
        public Thickness TopPanelMargin { get; set; }
        public Thickness ListViewMargin { get; set; }
        public double ListItemWidth { get; set; }
        public ObservableCollection<Therapy> Therapies { get; set; }
        public Prescription Prescription { get; set; }
        private Period period;
        private Patient patient;
        private Therapy therapy;
        private bool editing;

        public PrescriptionPage(Period period)
        {
            InitializeComponent();

            this.DataContext = this;

            this.period = period;
            this.patient = Model.Resources.patients[period.PatientUsername];
            this.editing = false;

            if (period.Prescription == null)
                Prescription = new Prescription();
            else
                Prescription = period.Prescription;

            Therapies = new ObservableCollection<Therapy>();

            foreach (Therapy therapy in Prescription.TherapyList)
            {
                Therapy t = new Therapy()
                {
                   Medicine = therapy.Medicine,
                    StartHours = therapy.StartHours,
                    TimesPerDay = therapy.TimesPerDay,
                    PauseInDays = therapy.PauseInDays,
                    EndDate = therapy.EndDate,
                    Instructions = therapy.Instructions
                };
                Therapies.Add(t);
            }

            TherapiesListView.ItemsSource = Therapies;
            Model.Resources.OpenMedicines();
            MedicinesComboBox.ItemsSource = Model.Resources.medicines;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopPanelMargin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 0);
            OnPropertyChanged("TopPanelMargin");
            ListViewMargin = new Thickness(this.ActualWidth * 0.17, 0, this.ActualWidth * 0.17, 0);
            OnPropertyChanged("ListViewMargin");
            ListItemWidth = TherapiesListView.ActualWidth;
            OnPropertyChanged("ListItemWidth");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            NewTherapyPopup.Visibility = Visibility.Visible;
            MedicinesComboBox.SelectedIndex = -1;
            StartHoursTextBox.Text = "00:00";
            TimesPerDayTextBox.Text = "0";
            PauseInDaysTextBox.Text = "0";
            EndDatePicker.SelectedDate = DateTime.Now.Date;
            InstructionsTextBox.Text = "";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Prescription.TherapyList = new List<Therapy>();

            foreach (Therapy t in Therapies)
                Prescription.TherapyList.Add(t);

            period.Prescription = Prescription;

            Model.Resources.SavePeriods();

            MessageBox.Show("Prescription successfully saved.", "Success");

            NavigationService.GoBack();
        }

        private void CancelTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            NewTherapyPopup.Visibility = Visibility.Hidden;
            editing = false;
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

            if (editing)
            {
                therapy.Medicine = MedicinesComboBox.SelectedItem as Medicine;
                therapy.StartHours = start;
                therapy.TimesPerDay = Int32.Parse(TimesPerDayTextBox.Text);
                therapy.PauseInDays = Int32.Parse(PauseInDaysTextBox.Text);
                therapy.EndDate = end;
                therapy.Instructions = InstructionsTextBox.Text;
            }
            else
            {
                Therapy therapy = new Therapy()
                {
                    Medicine = MedicinesComboBox.SelectedItem as Medicine,
                    StartHours = start,
                    TimesPerDay = Int32.Parse(TimesPerDayTextBox.Text),
                    PauseInDays = Int32.Parse(PauseInDaysTextBox.Text),
                    EndDate = end,
                    Instructions = InstructionsTextBox.Text
                };

                Therapies.Add(therapy);
            }

            OnPropertyChanged("Therapies");

            NewTherapyPopup.Visibility = Visibility.Hidden;
            editing = false;
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
            if (MedicinesComboBox.SelectedIndex == -1)
                return;

            Medicine medicine = MedicinesComboBox.SelectedItem as Medicine;

            foreach (string medicineAllergen in patient.MedicineAllergens)
                if (medicine.MedicineName.Equals(medicineAllergen))
                {
                    MessageBox.Show("Patient is allergic to selected medicine (" + medicine.MedicineName + ")", "Allergy detected");
                    MedicinesComboBox.SelectedIndex = -1;
                    return;
                }

            foreach (string ingredientAllergen in patient.IngredientAllergens)
                foreach (Ingredient ingredient in medicine.Ingredients)
                    if (ingredient.IngredientName.Equals(ingredientAllergen))
                    {
                        MessageBox.Show("Patient is allergic to an ingredient in selected medicine (" + ingredient.IngredientName + ")", "Allergy detected");
                        MedicinesComboBox.SelectedIndex = -1;
                        return;
                    }
        }

        private void EditTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            editing = true;
            therapy = (sender as Button).DataContext as Therapy;

            foreach (Medicine medicine in MedicinesComboBox.Items)
                if (medicine.MedicineName.Equals(therapy.Medicine.MedicineName))
                {
                    MedicinesComboBox.SelectedItem = medicine;
                    break;
                }
            StartHoursTextBox.Text = therapy.StartHours.ToString("HH:mm");
            TimesPerDayTextBox.Text = therapy.TimesPerDay.ToString();
            PauseInDaysTextBox.Text = therapy.PauseInDays.ToString();
            EndDatePicker.SelectedDate = therapy.EndDate;
            InstructionsTextBox.Text = therapy.Instructions;

            NewTherapyPopup.Visibility = Visibility.Visible;
        }

        private void RemoveTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            Therapies.Remove((sender as Button).DataContext as Therapy);
        }
    }
}
