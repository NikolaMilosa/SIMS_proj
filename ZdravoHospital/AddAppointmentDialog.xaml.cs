using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for AddAppointmentDialog.xaml
    /// </summary>
    public partial class AddAppointmentDialog : Window
    {
        public ObservableCollection<Doctor> DoctorList { get; set; }
        public AddAppointmentDialog()
        {
            InitializeComponent();
            DoctorList = new ObservableCollection<Doctor>();
            Doctor d = new Doctor("Doca", "Aleksijevic", "sad@gmail.com", new DateTime(), "2", "kolja", "dsada", MaritalStatus.MARRIED, Gender.MALE);
            DoctorList.Add(d);
            DataContext = this;
        }
    }
}
