using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using Period = Model.Period;

namespace ZdravoHospital.GUI.PatientUI
{
    public partial class AddAppointmentPage : Page
    {
        public AddAppointmentPage(Period period)
        {
            InitializeComponent();
            DataContext = period==null ? new AddAppointmentPageVM() : new AddAppointmentPageVM(period);
        }

    }
}
