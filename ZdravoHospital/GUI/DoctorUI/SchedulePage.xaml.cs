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

        private StackPanel[] stackPanels;
        private DockPanel[] dockPanels;
        
        public SchedulePage()
        {
            InitializeComponent();

            DataContext = this;

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

            stackPanels = new StackPanel[7];
            stackPanels[0] = MondayStackPanel;
            stackPanels[1] = TuesdayStackPanel;
            stackPanels[2] = WednesdayStackPanel;
            stackPanels[3] = ThursdayStackPanel;
            stackPanels[4] = FridayStackPanel;
            stackPanels[5] = SaturdayStackPanel;
            stackPanels[6] = SundayStackPanel;

            dockPanels = new DockPanel[7];
            dockPanels[0] = MondayDockPanel;
            dockPanels[1] = TuesdayDockPanel;
            dockPanels[2] = WednesdayDockPanel;
            dockPanels[3] = ThursdayDockPanel;
            dockPanels[4] = FridayDockPanel;
            dockPanels[5] = SaturdayDockPanel;
            dockPanels[6] = SundayDockPanel;

            DoctorsComboBox.ItemsSource = Model.Resources.doctors.Values;
            DoctorsComboBox.SelectedItem = Model.Resources.doctors[App.currentUser]; // Triger poziva prvo popunjavanje kalendara
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void PeriodButton_Click(Object sender, RoutedEventArgs e)
        {

        }

        private void PreviousWeekButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 7; i++)
                DaysDates[i] = DaysDates[i].AddDays(-7);

            OnPropertyChanged("DaysDates");

            PopulateCalendar();
        }

        private void NextWeekButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 7; i++)
                DaysDates[i] = DaysDates[i].AddDays(7);

            OnPropertyChanged("DaysDates");

            PopulateCalendar();
        }

        private void DoctorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateCalendar();
        }

        private void PopulateCalendar()
        {
            Doctor selectedDoctor = DoctorsComboBox.SelectedItem as Doctor;

            Model.Resources.OpenPeriods();
            Model.Resources.OpenPatients();

            for (int i = 0; i < stackPanels.Length; i++)
            {
                stackPanels[i].Children.Clear();

                List<Period> periods = new List<Period>();

                foreach (Period period in Model.Resources.periods)
                    if (period.StartTime.Date == DaysDates[i].Date && period.DoctorUsername.Equals(selectedDoctor.Username))
                        periods.Add(period);

                SortPeriods(periods);

                for (int j = 0; j < periods.Count; j++)
                {
                    Period period = periods[j];

                    Button emptyPeriodButton = new Button
                    {
                        Focusable = false,
                        Background = Brushes.Transparent,
                        Cursor = Cursors.Cross,
                        ToolTip = "New appointment/operation",
                        BorderThickness = new Thickness(0)
                    };
                    stackPanels[i].Children.Add(emptyPeriodButton);

                    if (j == 0)
                        emptyPeriodButton.Height = (period.StartTime.Hour * 60 + period.StartTime.Minute) * 4;
                    else
                        emptyPeriodButton.Height = ((periods[j].StartTime - periods[j - 1].StartTime).TotalMinutes - periods[j - 1].Duration) * 4;

                    Patient patient = Model.Resources.patients[period.PatientUsername];

                    PeriodButton periodButton = new PeriodButton
                    {
                        Period = period,
                        Height = period.Duration * 4
                    };
                    periodButton.UpperText.Text = period.StartTime.ToString("HH:mm") + "-" + period.StartTime.AddMinutes(period.Duration).ToString("HH:mm") + " " +
                                                  period.PeriodType.ToString().Substring(0, 1) + period.PeriodType.ToString().ToLower().Remove(0, 1) + " [" + period.RoomId + "]";
                    periodButton.LowerText.Text = patient.Name + " " + patient.Surname;
                    periodButton.Click += PeriodButton_Click;
                    stackPanels[i].Children.Add(periodButton);
                }
            }
        }

        private void SortPeriods(List<Period> periods)
        {
            for (int i = 0; i < periods.Count - 1; i++)
                for (int j = 1; j < periods.Count - i - 1; j++)
                    if (periods[j].StartTime < periods[i].StartTime)
                    {
                        Period temp = periods[i];
                        periods[i] = periods[j];
                        periods[j] = temp;
                    }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
