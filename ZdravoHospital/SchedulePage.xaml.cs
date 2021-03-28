﻿using Model;
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

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
