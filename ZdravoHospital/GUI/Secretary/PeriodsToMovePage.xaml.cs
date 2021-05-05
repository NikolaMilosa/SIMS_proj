using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public PeriodsToMovePage(List<Model.Period> periods)
        {
            Periods = new ObservableCollection<Model.Period>(periods);
            /*Model.Period period1 = new Model.Period(DateTime.Now.AddMinutes(60), 30, "saki", "zigara", true);
            Model.MovePeriod movePeriod1 = new Model.MovePeriod("zigara", "ljuba", 0, DateTime.Now, DateTime.Now.AddHours(2));
            Model.MovePeriod movePeriod2 = new Model.MovePeriod("zigara", "ljuba", 0, DateTime.Now, DateTime.Now.AddHours(2));
            Model.MovePeriod movePeriod3 = new Model.MovePeriod("zigara", "ljuba", 0, DateTime.Now, DateTime.Now.AddHours(2));
            period1.MovePeriods.Add(movePeriod1);
            period1.MovePeriods.Add(movePeriod2);
            period1.MovePeriods.Add(movePeriod3);
            Periods.Add(period1);*/

            this.DataContext = this;
            InitializeComponent();
        }
    }
}
