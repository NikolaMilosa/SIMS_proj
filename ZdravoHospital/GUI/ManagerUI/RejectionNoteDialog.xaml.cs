using System;
using System.Collections.Generic;
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
    /// Interaction logic for RejectionNoteDialog.xaml
    /// </summary>
    public partial class RejectionNoteDialog : Window, INotifyPropertyChanged
    {
        //fields : 
        private Medicine _medicine;
        private string _rejectionNote;
        private string _medicationName;

        public string MedicationName
        {
            get => _medicationName;
            set
            {
                _medicationName = value;
                OnPropertyChanged("MedicationName");
            }
        }

        public Medicine Medicine
        {
            get => _medicine;
            set
            {
                _medicine = value;

                MedicationName = Medicine.MedicineName;

                var medicineFunctions = new MedicineFunctions();
                RejectionNote = medicineFunctions.FindMedicineRecension(Medicine).RecensionNote;

                OnPropertyChanged("Medicine");
            }
        }

        public string RejectionNote
        {
            get => _rejectionNote;
            set
            {
                _rejectionNote = value;
                OnPropertyChanged("RejectionNote");
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

        public RejectionNoteDialog(Medicine medicine)
        {
            InitializeComponent();
            this.DataContext = this;

            Medicine = medicine;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
