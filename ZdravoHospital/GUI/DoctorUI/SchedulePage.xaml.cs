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
        private EmptyPeriodButton[] lastEmptyPeriodButtons;
        private EmptyPeriodButton selectedEmptyPeriodButton;
        
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

            lastEmptyPeriodButtons = new EmptyPeriodButton[7];
            lastEmptyPeriodButtons[0] = MondayLastEmptyPeriodButton;
            lastEmptyPeriodButtons[1] = TuesdayLastEmptyPeriodButton;
            lastEmptyPeriodButtons[2] = WednesdayLastEmptyPeriodButton;
            lastEmptyPeriodButtons[3] = ThursdayLastEmptyPeriodButton;
            lastEmptyPeriodButtons[4] = FridayLastEmptyPeriodButton;
            lastEmptyPeriodButtons[5] = SaturdayLastEmptyPeriodButton;
            lastEmptyPeriodButtons[6] = SundayLastEmptyPeriodButton;

            DoctorsComboBox.ItemsSource = Model.Resources.doctors.Values;
            DoctorsComboBox.SelectedItem = Model.Resources.doctors[App.currentUser]; // Triger poziva prvo popunjavanje kalendara
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
                    EmptyPeriodButton emptyPeriodButton = new EmptyPeriodButton
                    {
                        Focusable = false,
                        Background = Brushes.Transparent,
                        Cursor = Cursors.Cross,
                        ToolTip = "New appointment/operation",
                        BorderThickness = new Thickness(0),
                        StartTime = j > 0 ? 
                                    periods[j - 1].StartTime.AddMinutes(periods[j - 1].Duration)
                                    : new DateTime(DaysDates[i].Year, // u slucaju prvog dugmeta postavi da pocinje u ponoc
                                                   DaysDates[i].Month,
                                                   DaysDates[i].Day,
                                                   0, 0, 0),
                    };
                    emptyPeriodButton.Duration = (int)(periods[j].StartTime - emptyPeriodButton.StartTime).TotalMinutes;
                    emptyPeriodButton.Click += EmptyPeriodButton_Click;
                    stackPanels[i].Children.Add(emptyPeriodButton);

                    if (j == 0)
                        emptyPeriodButton.Height = (periods[j].StartTime.Hour * 60 + periods[j].StartTime.Minute) * 4;
                    else
                        emptyPeriodButton.Height = ((periods[j].StartTime - periods[j - 1].StartTime).TotalMinutes - periods[j - 1].Duration) * 4;

                    Patient patient = Model.Resources.patients[periods[j].PatientUsername];

                    PeriodButton periodButton = new PeriodButton
                    {
                        Period = periods[j],
                        Height = periods[j].Duration * 4
                    };
                    periodButton.UpperText.Text = periods[j].StartTime.ToString("HH:mm") + "-" + periods[j].StartTime.AddMinutes(periods[j].Duration).ToString("HH:mm") + " " +
                                                  periods[j].PeriodType.ToString().Substring(0, 1) + periods[j].PeriodType.ToString().ToLower().Remove(0, 1) + " [" + periods[j].RoomId + "]";
                    periodButton.LowerText.Text = patient.Name + " " + patient.Surname;
                    periodButton.Click += PeriodButton_Click;
                    stackPanels[i].Children.Add(periodButton);
                }

                if (periods.Count > 0)
                {
                    Period lastPeriod = periods[periods.Count - 1];
                    lastEmptyPeriodButtons[i].StartTime = lastPeriod.StartTime.AddMinutes(lastPeriod.Duration);
                }
                else
                {
                    lastEmptyPeriodButtons[i].StartTime = new DateTime(DaysDates[i].Year, // u slucaju prvog dugmeta postavi da pocinje u ponoc
                                                                       DaysDates[i].Month,
                                                                       DaysDates[i].Day,
                                                                       0, 0, 0);
                }

                DateTime endTime = new DateTime(DaysDates[i].AddDays(1).Year,
                                                                 DaysDates[i].AddDays(1).Month,
                                                                 DaysDates[i].AddDays(1).Day,
                                                                 0, 0, 0);
                lastEmptyPeriodButtons[i].Duration = (int)(endTime - lastEmptyPeriodButtons[i].StartTime).TotalMinutes;
            }
        }

        private void SortPeriods(List<Period> periods)
        {
            for (int i = 0; i < periods.Count - 1; i++)
                for (int j = 0; j < periods.Count - i - 1; j++)
                    if (periods[j].StartTime > periods[j + 1].StartTime)
                    {
                        Period temp = periods[j + 1];
                        periods[j + 1] = periods[j];
                        periods[j] = temp;
                    }
        }

        public void PeriodButton_Click(Object sender, RoutedEventArgs e)
        {
            Period period = (sender as PeriodButton).Period;

            if (period.PeriodType == PeriodType.APPOINTMENT)
                NavigationService.Navigate(new AppointmentPage(period));
            else if ((DoctorsComboBox.SelectedItem as Doctor).Username == App.currentUser)
                NavigationService.Navigate(new OperationPage(period, false));
            else
                NavigationService.Navigate(new OperationPage(period, true));
        }

        private void EmptyPeriodButton_Click(object sender, RoutedEventArgs e)
        {
            EmptyPeriodButton emptyPeriodButton = sender as EmptyPeriodButton;
            Doctor selectedDoctor = DoctorsComboBox.SelectedItem as Doctor;

            if (selectedDoctor.Username == App.currentUser && !selectedDoctor.SpecialistType.Equals("Doctor"))
            {
                selectedEmptyPeriodButton = emptyPeriodButton;
                NewPeriodPopUp.Visibility = Visibility.Visible;
            }
            else
                NavigationService.Navigate(new NewAppointmentPage(DoctorsComboBox.SelectedItem as Doctor,
                                                                  emptyPeriodButton.StartTime, emptyPeriodButton.Duration));
        }

        private void NewAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewAppointmentPage(DoctorsComboBox.SelectedItem as Doctor,
                                                              selectedEmptyPeriodButton.StartTime, selectedEmptyPeriodButton.Duration));
            NewPeriodPopUp.Visibility = Visibility.Hidden;
        }

        private void NewOperationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewOperationPage(DoctorsComboBox.SelectedItem as Doctor,
                                                            selectedEmptyPeriodButton.StartTime, selectedEmptyPeriodButton.Duration));
            NewPeriodPopUp.Visibility = Visibility.Hidden;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewPeriodPopUp.Visibility = Visibility.Hidden;
        }
    }
}
