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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PeriodsToMovePage.xaml
    /// </summary>
    public partial class PeriodsToMovePage : Page
    {
        public ObservableCollection<Model.Period> Periods { get; set; }

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
        public PeriodsToMovePage(List<Model.Period> periods)
        {
            Periods = new ObservableCollection<Model.Period>(periods);
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
            Model.Resources.OpenPeriods();
            if(SelectedPeriod != null && PeriodsListView.SelectedItem != null)
            {
                foreach(var movePeriod in SelectedPeriod.MovePeriods)
                {
                    foreach(var period in Model.Resources.periods)
                    {
                        if(movePeriod.InitialStartTime == period.StartTime && movePeriod.RoomId == period.RoomId)
                        {
                            period.StartTime = movePeriod.MovedStartTime;
                        }
                    }
                }
                Model.Resources.periods.Add(SelectedPeriod);
                Model.Resources.SavePeriods();
                NavigationService.Navigate(new UrgentPeriodSummaryPage(SelectedPeriod));
            }
        }
    }
}
