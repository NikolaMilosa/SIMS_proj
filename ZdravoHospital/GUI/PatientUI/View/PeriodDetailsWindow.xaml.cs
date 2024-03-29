﻿using System;
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
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for PeriodDetailsWindow.xaml
    /// </summary>
    public partial class PeriodDetailsWindow : Window
    {
        public PeriodDetailsWindow(int id)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new PeriodDetailsWindowVM(id);
        }
    }
}
