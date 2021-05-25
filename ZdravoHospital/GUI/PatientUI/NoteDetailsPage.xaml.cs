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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for NoteDetailsPage.xaml
    /// </summary>
    public partial class NoteDetailsPage : Page
    {

        public NoteDetailsPage(PatientNote patientNote)
        {
            InitializeComponent();
            DataContext = new NoteDetailsPageVM(patientNote);
        }

     
    }
}
