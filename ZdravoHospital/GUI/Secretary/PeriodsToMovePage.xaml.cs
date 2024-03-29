﻿using Model;
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PeriodsToMovePage.xaml
    /// </summary>
    public partial class PeriodsToMovePage : Page
    {
        public ObservableCollection<Period> Periods { get; set; }
        public PeriodsToMoveService PeriodsToMoveService { get; set; }
        private Period _selectedPeriod;
        public Period SelectedPeriod
        {
            get { return _selectedPeriod; }
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged("SelectedPeriod");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public PeriodsToMovePage(List<Period> periods)
        {
            Periods = new ObservableCollection<Period>(periods);

            IPeriodRepository periodRepository = RepositoryFactory.CreatePeriodRepository();
            IDoctorRepository doctorRepository = RepositoryFactory.CreateDoctorRepository();
            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();
            IRoomRepository roomRepository = RepositoryFactory.CreateRoomRepository();
            PeriodsToMoveService = new PeriodsToMoveService(doctorRepository, patientRepository, periodRepository, roomRepository);

            Periods = PeriodsToMoveService.GetSortedPeriods(Periods);
            this.DataContext = this;
            InitializeComponent();
        }


        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedPeriod != null && PeriodsListView.SelectedItem != null)
            {
                PeriodsToMoveService.ProcessMovePeriodSubmit(SelectedPeriod);
                NavigationService.Navigate(new UrgentPeriodSummaryPage(SelectedPeriod));
            }
        }

        
    }
}
