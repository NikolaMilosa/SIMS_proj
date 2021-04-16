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

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for customOkDialog.xaml
    /// </summary>
    public partial class customOkDialog : Window
    {
        public string DialogTitle { get; set; }

        public string DialogContent { get; set; }

        public customOkDialog(string title,string content)
        {
            InitializeComponent();
            DataContext = this;
            DialogTitle = title;
            DialogContent = content;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Topmost = true;
            Focus();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
