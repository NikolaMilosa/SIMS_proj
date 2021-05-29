using Model;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PeriodDetailsPage.xaml
    /// </summary>
    public partial class PeriodDetailsPage : Page
    {
        private Period _period;

        public PeriodDetailsPage(Period period)
        {
            InitializeComponent();
            _period = period;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new PeriodDetailsViewModel(NavigationService, _period);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopDockPanel.Margin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 15);
        }
    }
}
