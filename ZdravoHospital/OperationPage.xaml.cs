using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for OperationPage.xaml
    /// </summary>
    public partial class OperationPage : Page
    {
        Operation editingOperation;

        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<OperatingRoom> OperatingRooms { get; set; }

        public OperationPage()
        {
            InitializeComponent();

            this.DataContext = this;
            Patients = new ObservableCollection<Patient>(Model.Resources.Patients.Values);
            OperatingRooms = new ObservableCollection<OperatingRoom>(Model.Resources.OperatingRooms.Values);
        }
        public OperationPage(Operation operation)
        {
            editingOperation = operation;

            InitializeComponent();

            this.DataContext = this;
            Patients = new ObservableCollection<Patient>(Model.Resources.Patients.Values);
            OperatingRooms = new ObservableCollection<OperatingRoom>(Model.Resources.OperatingRooms.Values);

            PatientsComboBox.SelectedItem = Model.Resources.Patients[operation.Patient.Username];
            OperationDatePicker.SelectedDate = operation.StartTime;
            StartTimeTextBox.Text = operation.StartTime.ToString("HH:mm");
            DurationTextBox.Text = operation.Duration.ToString();
            OperatingRoomComboBox.SelectedItem = Model.Resources.OperatingRooms[operation.OperatingRoom.Id];
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (editingOperation != null)
            {
                Model.Resources.Specialists[MainWindow.LoggedPersonUsername].Operation.Remove(editingOperation);

                foreach (Operation o in Model.Resources.Patients[editingOperation.Patient.Username].Operation)
                    if (editingOperation.StartTime == o.StartTime && editingOperation.OperatingRoom.Id == o.OperatingRoom.Id)
                    {
                        Model.Resources.Patients[editingOperation.Patient.Username].Operation.Remove(o);
                        break;
                    }

                foreach (Operation o in Model.Resources.OperatingRooms[editingOperation.OperatingRoom.Id].Operation)
                    if (editingOperation.OperatingRoom.Id == o.OperatingRoom.Id)
                    {
                        Model.Resources.OperatingRooms[editingOperation.OperatingRoom.Id].Operation.Remove(o);
                        break;
                    }
            }

            Operation operation = new Operation();
            operation.Specialist = Model.Resources.Specialists[MainWindow.LoggedPersonUsername];
            operation.Patient = PatientsComboBox.SelectedItem as Patient;
            string[] parts = StartTimeTextBox.Text.Split(":");
            string hours = parts[0];
            string minutes = parts[1];
            operation.StartTime = new DateTime(OperationDatePicker.SelectedDate.Value.Year, OperationDatePicker.SelectedDate.Value.Month,
                OperationDatePicker.SelectedDate.Value.Day, Int32.Parse(hours), Int32.Parse(minutes), 0, DateTimeKind.Utc);
            operation.Duration = Int32.Parse(DurationTextBox.Text);
            operation.OperatingRoom = OperatingRoomComboBox.SelectedItem as OperatingRoom;

            Model.Resources.Serialize();

            NavigationService.GoBack();
        }
    }
}
