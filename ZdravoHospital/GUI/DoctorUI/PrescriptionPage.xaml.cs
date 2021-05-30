using Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Logics;
using ZdravoHospital.GUI.DoctorUI.Validations;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PrescriptionPage.xaml
    /// </summary>
    public partial class PrescriptionPage : Page, INotifyPropertyChanged
    {
        private readonly PrescriptionLogic _prescriptionLogic;
        private readonly Period _period;
        private readonly Patient _patient;
        private Therapy _therapy;
        private bool _editingTherapy;

        public ObservableCollection<Therapy> Therapies { get; set; }
        public Thickness TopPanelMargin { get; set; }
        public Thickness ListViewMargin { get; set; }
        public double ListItemWidth { get; set; }

        public PrescriptionPage(Period period)
        {
            InitializeComponent();

            DataContext = this;
            
            // fields initialization
            _prescriptionLogic = new PrescriptionLogic();
            _period = period;

            if (_period.Prescription == null)
                _period.Prescription = new Prescription();

            _patient = Model.Resources.patients[_period.PatientUsername];
            _editingTherapy = false;

            // TherapiesListView setup
            Therapies = new ObservableCollection<Therapy>();
            foreach (Therapy therapy in _period.Prescription.TherapyList)
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
            
            // MedicinesComboBox setup
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
            TopPanelMargin = new Thickness(this.ActualWidth * 0.075, 0, this.ActualWidth * 0.075, 0);
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
            // Setup TherapyPopup for new therapy and visualize it
            MedicinesComboBox.Text = "";
            MedicinesComboBox.ItemsSource = Model.Resources.medicines;
            MedicinesComboBox.SelectedIndex = -1;
            StartHoursTextBox.Text = "00:00";
            TimesPerDayTextBox.Text = "0";
            PauseInDaysTextBox.Text = "0";
            EndDatePicker.SelectedDate = DateTime.Now.Date;
            InstructionsTextBox.Text = "";
            TherapyPopup.Visibility = Visibility.Visible;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            _prescriptionLogic.GenerateTherapies(_period.Prescription, Therapies);
            _prescriptionLogic.SaveChanges();
            MessageBox.Show("Prescription successfully saved.", "Success");
            NavigationService.GoBack();
        }

        private void CancelTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            TherapyPopup.Visibility = Visibility.Hidden;
            _editingTherapy = false;
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

            if (_editingTherapy)
            {
                _therapy.Medicine = MedicinesComboBox.SelectedItem as Medicine;
                _therapy.StartHours = start;
                _therapy.TimesPerDay = Int32.Parse(TimesPerDayTextBox.Text);
                _therapy.PauseInDays = Int32.Parse(PauseInDaysTextBox.Text);
                _therapy.EndDate = end;
                _therapy.Instructions = InstructionsTextBox.Text;

                TherapiesListView.Items.Refresh();
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
            
            TherapyPopup.Visibility = Visibility.Hidden;
            _editingTherapy = false;
        }

        private bool IsInputValid()
        {
            if (MedicinesComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select medicine.", "Invalid input");
                return false;
            }

            if (!BasicValidation.IsTimeFromTextFormatValid(StartHoursTextBox.Text))
            {
                MessageBox.Show("Please enter start hours parameter in correct format (HH:mm).", "Invalid input");
                return false;
            }

            if (!BasicValidation.IsTimeFromTextValueValid(StartHoursTextBox.Text))
            {
                MessageBox.Show("Please enter valid start hours.", "Invalid input");
                return false;
            }

            if (!BasicValidation.IsIntegerFromTextValid(TimesPerDayTextBox.Text))
            {
                MessageBox.Show("Please enter times per day parameter in correct format (numbers only).", "Invalid input");
                return false;
            }

            if (!BasicValidation.IsIntegerFromTextValid(PauseInDaysTextBox.Text))
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

            // checking medicine allergens
            if (_prescriptionLogic.IsPatientAllergicToMedicine(_patient, medicine))
            {
                MessageBox.Show("Patient is allergic to selected medicine (" + medicine.MedicineName + ")", "Allergen detected");
                MedicinesComboBox.SelectedIndex = -1;
                return;
            }

            // checking ingredient allergens
            Ingredient ingredientAllergen = _prescriptionLogic.DetectIngredientAllegren(_patient, medicine);
            if (ingredientAllergen != null)
            {
                MessageBox.Show("Patient is allergic to an ingredient in selected medicine (" + ingredientAllergen.IngredientName + ")", "Allergen detected");
                MedicinesComboBox.SelectedIndex = -1;
                return;
            }
        }

        private void EditTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            _editingTherapy = true;
            _therapy = (sender as Button).DataContext as Therapy;

            foreach (Medicine medicine in MedicinesComboBox.Items)
                if (medicine.MedicineName.Equals(_therapy.Medicine.MedicineName))
                {
                    MedicinesComboBox.SelectedItem = medicine;
                    break;
                }
            StartHoursTextBox.Text = _therapy.StartHours.ToString("HH:mm");
            TimesPerDayTextBox.Text = _therapy.TimesPerDay.ToString();
            PauseInDaysTextBox.Text = _therapy.PauseInDays.ToString();
            EndDatePicker.SelectedDate = _therapy.EndDate;
            InstructionsTextBox.Text = _therapy.Instructions;

            TherapyPopup.Visibility = Visibility.Visible;
        }

        private void RemoveTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            Therapies.Remove((sender as Button).DataContext as Therapy);
        }

        private void MedicinesComboBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string text = MedicinesComboBox.Text;
            MedicinesComboBox.SelectedIndex = -1;
            MedicinesComboBox.Text = text;
            TextBox textBox = MedicinesComboBox.Template.FindName("PART_EditableTextBox", MedicinesComboBox) as TextBox;
            textBox.CaretIndex = text.Length;
            MedicinesComboBox.IsDropDownOpen = true;
            MedicinesComboBox.ItemsSource = 
                Model.Resources.medicines.Where(m => m.MedicineName.Contains(MedicinesComboBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private void MedicinesComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            MedicinesComboBox.IsDropDownOpen = true;
        }
    }
}
