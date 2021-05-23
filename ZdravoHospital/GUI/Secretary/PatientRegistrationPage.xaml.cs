using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using Model;
using Newtonsoft.Json;
using ZdravoHospital.GUI.Secretary.DTO;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PatientRegistrationPage.xaml
    /// </summary>
    public partial class PatientRegistrationPage : Page, INotifyPropertyChanged
    {
        private PatientDTO _patientDTO;
        public PatientDTO PatientDTO
        {
            get => _patientDTO;
            set
            {
                _patientDTO = value;
                OnPropertyChanged("PatientDTO");
            }
        }

        public PatientRegistrationService PatientService { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public PatientRegistrationPage()
        {
            InitializeComponent();
            this.DataContext = this;
            PatientDTO = new PatientDTO();
            PatientService = new PatientRegistrationService();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            PatientService.processPatientRegistration(PatientDTO);
            NavigationService.Navigate(new PatientsView());
        }

    }
}
