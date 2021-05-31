using Model;
using System;
using System.Collections.Generic;
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
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for UrgentPeriodSummaryPage.xaml
    /// </summary>
    public partial class UrgentPeriodSummaryPage : Page
    {
        public UrgentPeriodSummaryPage(Period selectedPeriod)
        {
            this.DataContext = new PeriodSummaryVM(selectedPeriod);
            InitializeComponent();
        }

    }
}
