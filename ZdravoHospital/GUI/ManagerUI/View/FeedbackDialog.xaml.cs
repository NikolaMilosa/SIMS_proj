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
using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.View
{
    /// <summary>
    /// Interaction logic for FeedbackDialog.xaml
    /// </summary>
    public partial class FeedbackDialog : Window
    {
        private FeedbackDialogViewModel currentViewModel;
        public FeedbackDialog(InjectorDTO injector, string currentUser)
        {
            InitializeComponent();
            currentViewModel = new FeedbackDialogViewModel(injector, currentUser);
            this.DataContext = currentViewModel;
        }
    }
}
