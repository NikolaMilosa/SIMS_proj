using Model;
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
            PeriodsToMoveService = new PeriodsToMoveService();
            this.sortPeriods();
            this.DataContext = this;
            InitializeComponent();
        }

        private void sortPeriods()
        {
            Periods = new ObservableCollection<Period>(Periods.OrderBy(x => x.MovePeriods.Count).ThenBy(x => x.findSumOfMovePeriods()).ToList<Period>());
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
