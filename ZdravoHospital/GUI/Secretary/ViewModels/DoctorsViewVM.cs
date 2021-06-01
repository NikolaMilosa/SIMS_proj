using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class DoctorsViewVM
    {
        public WorkTimeService WorkTimeService { get; set; }
        public ObservableCollection<DoctorShiftsViewDTO> Doctors { get; set; }
        public DoctorShiftsViewDTO SelectedDoctorView { get; set; }
        public DoctorsViewVM()
        {
            WorkTimeService = new WorkTimeService();
            initDoctorsView();

            ShiftCommand = new RelayCommand(shiftExecute);
            VacationCommand = new RelayCommand(vacationExecute);
        }

        private void initDoctorsView()
        {
            Doctors = new ObservableCollection<DoctorShiftsViewDTO>();
            List<Doctor> doctors = WorkTimeService.GetAllDoctors();
            foreach (var doctor in doctors)
            {
                Doctors.Add(new DoctorShiftsViewDTO(doctor));
            }
        }

        public ICommand ShiftCommand { get; set; }
        public ICommand VacationCommand { get; set; }

        private void shiftExecute(object parameter)
        {
            if (SelectedDoctorView != null)
                SecretaryWindowVM.NavigationService.Navigate(new EditShiftPage(SelectedDoctorView.Doctor));
        }
        private void vacationExecute(object parameter)
        {
            if (SelectedDoctorView != null)
                SecretaryWindowVM.NavigationService.Navigate(new EditVacationPage(SelectedDoctorView.Doctor));
        }
    }
}
