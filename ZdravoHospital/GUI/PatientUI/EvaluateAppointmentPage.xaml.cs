using Model;
using Model.Repository;
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
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for EvaluateAppointmentPage.xaml
    /// </summary>
    public partial class EvaluateAppointmentPage : Page
    {
        public EvaluateAppointmentPage(Period period)
        {
            InitializeComponent();
            DataContext = new EvaluateAppointmentPageVM(period);
           
        }

    }
}
