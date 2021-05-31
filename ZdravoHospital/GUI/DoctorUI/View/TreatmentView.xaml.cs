using Model;
using System;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI.View
{
    /// <summary>
    /// Interaction logic for TreatmentView.xaml
    /// </summary>
    public partial class TreatmentView : Page
    {
        private Period _period;

        public TreatmentView(Period period)
        {
            InitializeComponent();

            _period = period;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new TreatmentViewModel(NavigationService, _period);
        }
    }
}
