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
    /// Interaction logic for InventoryFilteringDialog.xaml
    /// </summary>
    public partial class InventoryFilteringDialog : Window
    {
        private InventoryFilteringDialogViewModel currentViewModel;
        public InventoryFilteringDialog()
        {
            InitializeComponent();
            currentViewModel = new InventoryFilteringDialogViewModel();
            this.DataContext = currentViewModel;
        }
    }
}
