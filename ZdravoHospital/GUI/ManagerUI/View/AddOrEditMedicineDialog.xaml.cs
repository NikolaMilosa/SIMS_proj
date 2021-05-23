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
    /// Interaction logic for AddOrEditMedicineDialog.xaml
    /// </summary>
    public partial class AddOrEditMedicineDialog : Window
    {
        private AddOrEditMedicineDialogViewModel currentViewModel;
        public AddOrEditMedicineDialog(Medicine medicine, InjectorDTO injector)
        {
            InitializeComponent();
            currentViewModel = new AddOrEditMedicineDialogViewModel(medicine, injector);
            this.DataContext = currentViewModel;
        }

        private void IngredientsTable_GotFocus(object sender, RoutedEventArgs e)
        {
            if (currentViewModel.GetIngredientsCount() > 0)
            {
                IngredientsTable.SelectedIndex = 0;
                IngredientsTable.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void IngredientsTable_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (IngredientsTable.SelectedIndex + 1 < IngredientsTable.Items.Count)
                {
                    IngredientsTable.SelectedIndex += 1;
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.SelectedIndex]);
                }
                else if (IngredientsTable.SelectedIndex + 1 == IngredientsTable.Items.Count)
                {
                    IngredientsTable.SelectedIndex = 0;
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.SelectedIndex]);
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (IngredientsTable.SelectedIndex - 1 >= 0)
                {
                    IngredientsTable.SelectedIndex -= 1;
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.SelectedIndex]);
                }
                else if (IngredientsTable.SelectedIndex - 1 < 0)
                {
                    IngredientsTable.ScrollIntoView(IngredientsTable.Items[IngredientsTable.Items.Count - 1]);
                    IngredientsTable.SelectedIndex = IngredientsTable.Items.Count - 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                if (ConfirmButton.IsEnabled)
                    ConfirmButton.Focus();
                else
                    CancelButton.Focus();

                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                currentViewModel.HandleEnter();
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                currentViewModel.HandleDelete();
                e.Handled = true;
            }
        }
    }
}
