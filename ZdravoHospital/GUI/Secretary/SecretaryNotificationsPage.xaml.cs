﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryNotificationsPage.xaml
    /// </summary>
    public partial class SecretaryNotificationsPage : Page
    {
        public SecretaryNotificationsPage()
        {
            InitializeComponent();
            this.DataContext = new SecretaryNotificationsVM();
        }

    }
}
