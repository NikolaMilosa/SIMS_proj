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
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.View
{
    /// <summary>
    /// Interaction logic for RejectionNoteDialog.xaml
    /// </summary>
    public partial class RejectionNoteDialog : Window
    {
        private RejectionNoteDialogViewModel currentViewModel;

        public RejectionNoteDialog(Medicine medicine)
        {
            InitializeComponent();
            currentViewModel = new RejectionNoteDialogViewModel(medicine);
            this.DataContext = currentViewModel;
        }
    }
}