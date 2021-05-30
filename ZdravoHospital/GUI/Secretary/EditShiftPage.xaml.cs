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
    /// Interaction logic for EditShiftPage.xaml
    /// </summary>
    public partial class EditShiftPage : Page
    {
        public Doctor SelectedDoctor { get; set; }
        public ShiftDTO ShiftDTO { get; set; }
        public ShiftService ShiftService { get; set; } 
        public EditShiftPage(Doctor selectedDoctor)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedDoctor = selectedDoctor;
            ShiftDTO = new ShiftDTO();
            ShiftService = new ShiftService();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            ShiftService.ProcessShiftCreation(SelectedDoctor, ShiftDTO);
            NavigationService.Navigate(new DoctorsView());
        }
    }
}
