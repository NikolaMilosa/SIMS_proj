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
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.View
{
    /// <summary>
    /// Interaction logic for ValidationRequestDialog.xaml
    /// </summary>
    public partial class ValidationRequestDialog : Window
    {

        private ValidationRequestDialogViewModel currentViewModel;
        public ValidationRequestDialog(Medicine medicine, InjectorDTO injector)
        {
            InitializeComponent();
            currentViewModel = new ValidationRequestDialogViewModel(medicine, injector);
            this.DataContext = currentViewModel;
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoctorComboBox.IsDropDownOpen = DoctorComboBox.IsDropDownOpen == false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (DoctorComboBox.IsDropDownOpen == true)
                {
                    if (DoctorComboBox.SelectedIndex + 1 < DoctorComboBox.Items.Count)
                    {
                        DoctorComboBox.SelectedIndex += 1;
                    }
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (DoctorComboBox.IsDropDownOpen == true)
                {
                    if (DoctorComboBox.SelectedIndex - 1 >= 0)
                    {
                        DoctorComboBox.SelectedIndex -= 1;
                    }
                }

                e.Handled = true;
            }
        }
    }
}
