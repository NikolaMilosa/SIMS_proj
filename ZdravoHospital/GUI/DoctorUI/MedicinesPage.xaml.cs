using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for MedicinesPage.xaml
    /// </summary>
    public partial class MedicinesPage : Page, INotifyPropertyChanged
    {
        private List<Medicine> medicines;

        public Thickness ListViewPadding { get; set; }
        public double ListItemWidth { get; set; }

        public MedicinesPage()
        {
            InitializeComponent();

            this.DataContext = this;

            Model.Resources.OpenMedicineRecensions();
            Model.Resources.OpenMedicines();

            medicines = new List<Medicine>();

            foreach (MedicineRecension mr in Model.Resources.medicineRecensions)
                if (mr.DoctorUsername.Equals(App.currentUser))
                    foreach (Medicine m in Model.Resources.medicines)
                        if (mr.MedicineName.Equals(m.MedicineName))
                        {
                            medicines.Add(m);
                            break;
                        }

            MedicinesListView.ItemsSource = medicines;
            MedicinesListView.Items.Filter = Filter;

            StatusComboBox.Items.Add("All");
            StatusComboBox.Items.Add("Pending");
            StatusComboBox.Items.Add("Approved");
            StatusComboBox.Items.Add("Rejected");
            StatusComboBox.SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListViewPadding = new Thickness(this.ActualWidth * 0.17, 0, this.ActualWidth * 0.17, 0);
            OnPropertyChanged("ListViewPadding");
            ListItemWidth = MedicinesListView.ActualWidth;
            OnPropertyChanged("ListItemWidth");
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            Medicine medicine = (sender as Button).DataContext as Medicine;
            NavigationService.Navigate(new MedicineInfoPage(medicine));
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = StatusComboBox.SelectedValue.ToString();

            if (selection.Equals("All"))
                MedicinesListView.ItemsSource = medicines;
            else if (selection.Equals("Pending"))
                MedicinesListView.ItemsSource = medicines.Where(m => m.Status == MedicineStatus.PENDING);
            else if (selection.Equals("Approved"))
                MedicinesListView.ItemsSource = medicines.Where(m => m.Status == MedicineStatus.APPROVED);
            else if (selection.Equals("Rejected"))
                MedicinesListView.ItemsSource = medicines.Where(m => m.Status == MedicineStatus.REJECTED);
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            CollectionViewSource.GetDefaultView(MedicinesListView.ItemsSource).Refresh();
        }

        private bool Filter(object item)
        {
            return (item as Medicine).MedicineName.Contains(SearchTextBox.Text);
        }
    }
}
