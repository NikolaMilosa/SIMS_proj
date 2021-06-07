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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for WeeklyReport.xaml
    /// </summary>
    public partial class WeeklyReport : Page, INotifyPropertyChanged
    {
        public ObservableCollection<WeeklyReportDTO> Periods { get; set; }
        private DateTime _selectedDate;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set 
            { 
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }


        public WeeklyReportService WeeklyReportService { get; set; }
        public WeeklyReport()
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedDate = DateTime.Now;
            
            WeeklyReportService = new WeeklyReportService();
            List<WeeklyReportDTO> periods = WeeklyReportService.GetDesiredPeriods(SelectedDate);
            periods = periods.OrderByDescending(p => p.StartTime).ToList();
            Periods = new ObservableCollection<WeeklyReportDTO>(periods);
        }

        private void ExportPdf_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedDate != null)
            {
                WeeklyReportService.GeneratePDF(SelectedDate, new List<WeeklyReportDTO>(Periods));
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            List<WeeklyReportDTO> periods = WeeklyReportService.GetDesiredPeriods(SelectedDate);
            periods = periods.OrderBy(p => p.StartTime).ToList();
            Periods = new ObservableCollection<WeeklyReportDTO>(periods);
            PeriodsListView.ItemsSource = Periods;
        }
    }
}
