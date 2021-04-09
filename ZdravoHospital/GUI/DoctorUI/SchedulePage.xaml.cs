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
using Model;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page, INotifyPropertyChanged
    {
        public DateTime[] DaysDates { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public SchedulePage()
        {
            InitializeComponent();

            DataContext = this;

            DoctorsComboBox.ItemsSource = Model.Resources.doctors.Values;
            DoctorsComboBox.SelectedItem = Model.Resources.doctors[App.currentUser];

            int dayOfWeek = (int)DateTime.Now.DayOfWeek;

            if (dayOfWeek == 0) // because DayOfWeek.SUNDAY == 0
                dayOfWeek = 7;

            DaysDates = new DateTime[7];
            DaysDates[0] = DateTime.Now.AddDays(1 - dayOfWeek);
            DaysDates[1] = DateTime.Now.AddDays(2 - dayOfWeek);
            DaysDates[2] = DateTime.Now.AddDays(3 - dayOfWeek);
            DaysDates[3] = DateTime.Now.AddDays(4 - dayOfWeek);
            DaysDates[4] = DateTime.Now.AddDays(5 - dayOfWeek);
            DaysDates[5] = DateTime.Now.AddDays(6 - dayOfWeek);
            DaysDates[6] = DateTime.Now.AddDays(7 - dayOfWeek);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PreviousWeekButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 7; i++)
                DaysDates[i] = DaysDates[i].AddDays(-7);

            OnPropertyChanged("DaysDates");
        }

        private void NextWeekButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 7; i++)
                DaysDates[i] = DaysDates[i].AddDays(7);

            OnPropertyChanged("DaysDates");
        }

        private void NewAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewOperationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewPeriodPopUp.Visibility = Visibility.Hidden;
        }
    }
}
