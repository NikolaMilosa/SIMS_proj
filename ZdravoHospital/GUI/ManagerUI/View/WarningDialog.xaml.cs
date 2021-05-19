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
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.View
{
    /// <summary>
    /// Interaction logic for WarningDialog.xaml
    /// </summary>
    public partial class WarningDialog : Window
    {
        private WarningDialogViewModel currentViewModel;
        public WarningDialog(object someObject, params object[] otherParams) 
        {
            InitializeComponent();
            currentViewModel = new WarningDialogViewModel(someObject, otherParams);
            this.DataContext = currentViewModel;
        }
    }
}
