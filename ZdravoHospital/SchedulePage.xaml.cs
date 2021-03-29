using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Specialist> Specialists { get; set; }

        public SchedulePage()
        {
            InitializeComponent();

            if (MainWindow.ActiveRole != RoleType.SPECIALIST)
                NewOperationButton.Visibility = Visibility.Collapsed;

            PeriodDataGrid.AutoGenerateColumns = false;

            this.DataContext = this;
            Doctors = new ObservableCollection<Doctor>(Model.Resources.Doctors.Values);
            Specialists = new ObservableCollection<Specialist>(Model.Resources.Specialists.Values);
        }

        private void NewAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentPage());
        }

        private void NewOperationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OperationPage());
        }

        private void DoctorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpecialistsComboBox.SelectionChanged -= SpecialistsComboBox_SelectionChanged;
            SpecialistsComboBox.SelectedItem = null;
            SpecialistsComboBox.SelectionChanged += SpecialistsComboBox_SelectionChanged;

            PeriodDataGrid.Items.Clear();

            foreach (Appointment appointment in (DoctorsComboBox.SelectedItem as Doctor).Appointment)
                PeriodDataGrid.Items.Add(appointment as Period);
        }

        private void SpecialistsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoctorsComboBox.SelectionChanged -= DoctorsComboBox_SelectionChanged;
            DoctorsComboBox.SelectedItem = null;
            DoctorsComboBox.SelectionChanged += DoctorsComboBox_SelectionChanged;

            PeriodDataGrid.Items.Clear();

            foreach (Appointment appointment in (SpecialistsComboBox.SelectedItem as Specialist).Appointment)
                PeriodDataGrid.Items.Add(appointment as Period);

            foreach (Operation operation in (SpecialistsComboBox.SelectedItem as Specialist).Operation)
                PeriodDataGrid.Items.Add(operation as Period);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if ((PeriodDataGrid.SelectedItem as Period).PeriodType == PeriodType.APPOINTMENT)
            {
                Appointment appointment = PeriodDataGrid.SelectedItem as Appointment;
                NavigationService.Navigate(new AppointmentPage(appointment));
            }
            else
            {
                if (MainWindow.ActiveRole == RoleType.SPECIALIST && (SpecialistsComboBox.SelectedItem as Specialist).Username.Equals(MainWindow.LoggedPersonUsername))
                {
                    Operation operation = PeriodDataGrid.SelectedItem as Operation;
                    NavigationService.Navigate(new OperationPage(operation));
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if ((PeriodDataGrid.SelectedItem as Period).PeriodType == PeriodType.APPOINTMENT)
            {
                Appointment appointment = PeriodDataGrid.SelectedItem as Appointment;

                if (DoctorsComboBox.SelectedItem as Doctor != null)
                    (DoctorsComboBox.SelectedItem as Doctor).Appointment.Remove(appointment);
                else
                    (SpecialistsComboBox.SelectedItem as Doctor).Appointment.Remove(appointment);

                foreach (Appointment a in Model.Resources.Patients[appointment.Patient.Username].Appointment)
                    if (appointment.StartTime == a.StartTime && appointment.AppointmentRoom.Id == a.AppointmentRoom.Id)
                    {
                        Model.Resources.Patients[appointment.Patient.Username].Appointment.Remove(a);
                        break;
                    }

                foreach (Appointment a in Model.Resources.AppointmentRooms[appointment.AppointmentRoom.Id].Appointment)
                    if (appointment.AppointmentRoom.Id == a.AppointmentRoom.Id)
                    {
                        Model.Resources.AppointmentRooms[appointment.AppointmentRoom.Id].Appointment.Remove(a);
                        break;
                    }
            }
            else
            {
                Operation operation = PeriodDataGrid.SelectedItem as Operation;

                (SpecialistsComboBox.SelectedItem as Specialist).Operation.Remove(operation);

                foreach (Operation o in Model.Resources.Patients[operation.Patient.Username].Operation)
                    if (operation.StartTime == o.StartTime && operation.OperatingRoom.Id == o.OperatingRoom.Id)
                    {
                        Model.Resources.Patients[operation.Patient.Username].Operation.Remove(o);
                        break;
                    }

                foreach (Operation o in Model.Resources.OperatingRooms[operation.OperatingRoom.Id].Operation)
                    if (operation.OperatingRoom.Id == o.OperatingRoom.Id)
                    {
                        Model.Resources.OperatingRooms[operation.OperatingRoom.Id].Operation.Remove(o);
                        break;
                    }
            }

            PeriodDataGrid.Items.Remove(PeriodDataGrid.SelectedItem);

            Model.Resources.Serialize();
        }
    }
}
