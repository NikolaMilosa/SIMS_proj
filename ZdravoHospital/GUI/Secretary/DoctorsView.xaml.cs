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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for DoctorsView.xaml
    /// </summary>
    public partial class DoctorsView : Page
    {
        public WorkTimeService WorkTimeService { get; set; }
        public ObservableCollection<DoctorShiftsViewDTO> Doctors { get; set; }
        public DoctorShiftsViewDTO SelectedDoctorView { get; set; }
        public DoctorsView()
        {
            InitializeComponent();
            this.DataContext = this;
            WorkTimeService = new WorkTimeService();
            initDoctorsView();
        }
        private void initDoctorsView()
        {
            Doctors = new ObservableCollection<DoctorShiftsViewDTO>();
            List<Doctor> doctors = WorkTimeService.GetAllDoctors();
            foreach(var doctor in doctors)
            {
                Doctors.Add(new DoctorShiftsViewDTO(doctor));
            }
        }

        private void ShiftButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedDoctorView != null)
                NavigationService.Navigate(new EditShiftPage(SelectedDoctorView.Doctor));

        }

        private void VacationButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedDoctorView != null)
                NavigationService.Navigate(new EditVacationPage(SelectedDoctorView.Doctor));
        }
    }
}
