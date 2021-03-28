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
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<OperatingRoom> OperatingRooms { get; set; }

        public OperationPage()
        {
            InitializeComponent();

            this.DataContext = this;
            Patients = new ObservableCollection<Patient>(Model.Resources.Patients.Values);
            OperatingRooms = new ObservableCollection<OperatingRoom>(Model.Resources.OperatingRooms.Values);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
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
