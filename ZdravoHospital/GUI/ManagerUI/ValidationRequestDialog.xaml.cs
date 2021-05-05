using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;
using ZdravoHospital.GUI.ManagerUI.Logics;

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for ValidationRequestDialog.xaml
    /// </summary>
    public partial class ValidationRequestDialog : Window, INotifyPropertyChanged
    {
        //fields:
        private Medicine _observedMedicine;
        private Doctor _selectedDocotor;
        private ObservableCollection<Doctor> _listOfDoctors;

        public Medicine ObservedMedicine
        {
            get => _observedMedicine;
            set
            {
                _observedMedicine = value;
                OnPropertyChanged("ObservedMedicine");
            }
        }

        public Doctor SelectedDoctor
        {
            get => _selectedDocotor;
            set
            {
                _selectedDocotor = value;
                OnPropertyChanged("SelectedDoctor");
            }
        }

        public ObservableCollection<Doctor> ListOfDoctors
        {
            get => _listOfDoctors;
            set
            {
                _listOfDoctors = value;
                OnPropertyChanged("ListOfDoctors");
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
        public ValidationRequestDialog(Medicine medicine)
        {
            InitializeComponent();
            this.DataContext = this;

            ObservedMedicine = medicine;
            ListOfDoctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values);
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoctorComboBox.IsDropDownOpen = DoctorComboBox.IsDropDownOpen == false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (DoctorComboBox.IsDropDownOpen == true)
                {
                    if (DoctorComboBox.SelectedIndex + 1 < DoctorComboBox.Items.Count)
                    {
                        DoctorComboBox.SelectedIndex += 1;
                    }
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (DoctorComboBox.IsDropDownOpen == true)
                {
                    if (DoctorComboBox.SelectedIndex - 1 >= 0)
                    {
                        DoctorComboBox.SelectedIndex -= 1;
                    }
                }

                e.Handled = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var medicineFunctions = new MedicineFunctions();
            medicineFunctions.SendMedicineOnRecension(ObservedMedicine, SelectedDoctor);
            this.Close();
        }
    }
}
