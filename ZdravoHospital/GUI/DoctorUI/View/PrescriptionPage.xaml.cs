using Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.DoctorUI.Controllers;
using ZdravoHospital.GUI.DoctorUI.Exceptions;
using ZdravoHospital.GUI.DoctorUI.Validations;

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

        private PrescriptionController _prescriptionController;
        private Period _period;
        private Patient _patient;
        private Therapy _therapy;
        private bool _editingTherapy;

        public ObservableCollection<Therapy> Therapies { get; set; }

        private Visibility _messagePopUpVisibility;
        public Visibility MessagePopUpVisibility
        {
            get
            {
                return _messagePopUpVisibility;
            }
            set
            {
                _messagePopUpVisibility = value;
                OnPropertyChanged("MessagePopUpVisibility");
            }
        }

        private string _messageText;
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                _messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        public MyICommand CloseMessagePopUpCommand { get; set; }

        public void Executed_CloseMessagePopUpCommand()
        {
            MessagePopUpVisibility = Visibility.Collapsed;

            if (MessageText.Equals("Prescription saved successfully."))
                NavigationService.GoBack();
        }

        public bool CanExecute_CloseMessagePopUpCommand()
        {
            return true;
        }

        public PrescriptionPage(Period period)
        {
            InitializeComponent();

            DataContext = this;

            // fields initialization
            _prescriptionController = new PrescriptionController();
            _period = period;

            if (_period.Prescription == null)
                _period.Prescription = new Prescription();

            _patient = new PatientController().GetPatient(_period.PatientUsername);
            _editingTherapy = false;

            // TherapiesListView setup
            Therapies = _prescriptionController.CollectTherapies(_period.Prescription);
            TherapiesListView.ItemsSource = Therapies;
            
            // MedicinesComboBox setup
            MedicinesComboBox.ItemsSource = new MedicineController().GetApprovedMedicines();

            InitializeCommands();
            MessagePopUpVisibility = Visibility.Collapsed;
        }

        private void InitializeCommands()
        {
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            // Setup TherapyPopup for new therapy and visualize it
            MedicinesComboBox.Text = "";
            //MedicinesComboBox.ItemsSource = Model.Resources.medicines;
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
            _prescriptionController.GenerateTherapies(_period.Prescription, Therapies);
            _prescriptionController.SavePrescription(_period);
            MessageText = "Prescription saved successfully.";
            MessagePopUpVisibility = Visibility.Visible;
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
                MessageText = "Please select medicine.";
                MessagePopUpVisibility = Visibility.Visible;
                return false;
            }

            if (!BasicValidation.IsTimeFromTextFormatValid(StartHoursTextBox.Text))
            {
                MessageText = "Please enter start hours parameter in correct format (HH:mm).";
                MessagePopUpVisibility = Visibility.Visible;
                return false;
            }

            if (!BasicValidation.IsTimeFromTextValueValid(StartHoursTextBox.Text))
            {
                MessageText = "Please enter valid start hours.";
                MessagePopUpVisibility = Visibility.Visible;
                return false;
            }

            if (!BasicValidation.IsIntegerFromTextValid(TimesPerDayTextBox.Text))
            {
                MessageText = "Please enter times per day parameter in correct format (positive numbers only).";
                MessagePopUpVisibility = Visibility.Visible;
                return false;
            }

            if (!BasicValidation.IsIntegerFromTextValid(PauseInDaysTextBox.Text))
            {
                MessageText = "Please enter pause in days parameter in correct format (positive numbers only).";
                MessagePopUpVisibility = Visibility.Visible;
                return false;
            }

            return true;
        }

        private void MedicinesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MedicinesComboBox.SelectedIndex == -1)
                return;

            Medicine medicine = MedicinesComboBox.SelectedItem as Medicine;

            try
            {
                _prescriptionController.CheckAllergens(medicine, _patient);
                return;
            }
            catch (MedicineAllergenException exception)
            {
                MessageText = exception.Message;
            }
            catch (IngredientAllergenException exception)
            {
                MessageText = exception.Message;
            }

            MessagePopUpVisibility = Visibility.Visible;
            MedicinesComboBox.SelectedIndex = -1;
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
    }
}
