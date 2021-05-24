using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
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
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for SurveyPage.xaml
    /// </summary>
    public partial class SurveyPage : Page
    {
      
        public SurveyPage(PatientWindowVM patientWindowVm)
        {
            InitializeComponent();
            DataContext = new SurveyPageVM(patientWindowVm);
        }

    }
}
