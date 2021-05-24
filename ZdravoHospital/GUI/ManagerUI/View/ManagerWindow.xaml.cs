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
using Model;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.View
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private ManagerWindowViewModel currentViewModel;

        public ManagerWindow(string au)
        {
            InitializeComponent();
            currentViewModel = new ManagerWindowViewModel(au);
            this.DataContext = currentViewModel;
            RoomsButton.Focus();
        }
    }
}
