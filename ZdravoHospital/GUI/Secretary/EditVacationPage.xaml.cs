using Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditVacationPage.xaml
    /// </summary>
    public partial class EditVacationPage : Page
    {
        public Doctor SelectedDoctor { get; set; }
        public VacationDTO VacationDTO { get; set; }
        public VacationService VacationService { get; set; }

        public EditVacationPage(Doctor selectedDoctor)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedDoctor = selectedDoctor;
            VacationService = new VacationService();
            VacationDTO = new VacationDTO();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            VacationService.ProcessVacationCreation(VacationDTO, SelectedDoctor);
            NavigationService.Navigate(new DoctorsView());
        }

        private void ClearVacationsButton_Click(object sender, RoutedEventArgs e)
        {
            VacationService.ProcessVacationDeletion(SelectedDoctor);
            MessageBox.Show("Deleted successfully!");
        }
    }
}
