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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for WorkTimePage.xaml
    /// </summary>
    public partial class WorkTimePage : Page
    {
        public WorkTimeService WorkService { get; set; }
        public Doctor SelectedDoctor { get; set; }
        public Shift SelectedShift { get; set; }
        public DateTime VacationStart { get; set; }
        public DateTime ShiftStart { get; set; }
        public int NumberOfDays { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }
        public WorkTimePage()
        {
            InitializeComponent();
            this.DataContext = this;

            WorkService = new WorkTimeService();
            Doctors = new ObservableCollection<Doctor>(WorkService.GetAllDoctors());
            ShiftStart = DateTime.Now.Date;
            VacationStart = DateTime.Now.Date;

            ICollectionView viewDoctors = (ICollectionView)CollectionViewSource.GetDefaultView(Doctors);
            viewDoctors.Filter = UserFilterDoctors;
        }

        private void DoctorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DoctorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(DoctorsListBox.ItemsSource).Refresh();
        }

        private bool UserFilterDoctors(object item)
        {
            if (String.IsNullOrEmpty(DoctorTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(DoctorTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDoctor == null)
                return;
            DoctorWorkDTO doctorWorkDTO = new DoctorWorkDTO(SelectedDoctor, SelectedShift, ShiftStart, VacationStart, NumberOfDays);
            if (ShiftComboBox.SelectedIndex == -1)
            {
                WorkService.SetDoctorHolidaySchedule(doctorWorkDTO);
            }
            else
            {
                WorkService.SetDoctorWorkSchedule(doctorWorkDTO);
            }
            NavigationService.Navigate(new DoctorsView());
        }
    }
}
