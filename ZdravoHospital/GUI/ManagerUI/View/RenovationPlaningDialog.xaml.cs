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
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.View
{
    /// <summary>
    /// Interaction logic for RenovationPlaningDialog.xaml
    /// </summary>
    public partial class RenovationPlaningDialog : Window
    {
        private RenovationPlanningDialogViewModel currentViewModel;

        public RenovationPlaningDialog(InjectorDTO injector)
        {
            InitializeComponent();
            currentViewModel = new RenovationPlanningDialogViewModel(injector);
            this.DataContext = currentViewModel;
        }

        private void FirstPicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!FirstPicker.IsDropDownOpen)
            {
                if (e.Key == Key.Enter)
                {
                    FirstPicker.IsDropDownOpen = true;
                    e.Handled = true;
                }
                else if (e.Key == Key.Tab) { }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    currentViewModel.StartDate = (DateTime)FirstPicker.SelectedDate;
                    FirstPicker.IsDropDownOpen = false;
                    e.Handled = true;
                }
                else if (e.Key == Key.Left) { }
                else if (e.Key == Key.Right) { }
                else if (e.Key == Key.Up) { }
                else if (e.Key == Key.Down) { }
                else if (e.Key == Key.Tab)
                {
                    FirstPicker.IsDropDownOpen = false;
                    e.Handled = true;
                    CancelButton.Focus();
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void SecondPicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!SecondPicker.IsDropDownOpen)
            {
                if (e.Key == Key.Enter)
                {
                    SecondPicker.IsDropDownOpen = true;
                    e.Handled = true;
                }
                else if (e.Key == Key.Tab) { }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    currentViewModel.EndDate = (DateTime)SecondPicker.SelectedDate;
                    SecondPicker.IsDropDownOpen = false;
                    e.Handled = true;
                }
                else if (e.Key == Key.Left) { }
                else if (e.Key == Key.Right) { }
                else if (e.Key == Key.Up) { }
                else if (e.Key == Key.Down) { }
                else if (e.Key == Key.Tab)
                {
                    SecondPicker.IsDropDownOpen = false;
                    e.Handled = true;
                    CancelButton.Focus();
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
    }
}
