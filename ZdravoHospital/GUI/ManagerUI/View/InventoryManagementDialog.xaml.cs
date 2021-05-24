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
    /// Interaction logic for InventoryManagementDialog.xaml
    /// </summary>
    public partial class InventoryManagementDialog : Window
    {
        private InventoryManagementDialogViewModel currentViewModel;
        public InventoryManagementDialog(InventoryManagementDialogViewModel passedViewModel)
        {
            InitializeComponent();
            currentViewModel = passedViewModel;
            this.DataContext = currentViewModel;
        }

        private void FirstRoomDataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FirstRoomDataGrid.Items.Count > 0)
            {
                FirstRoomDataGrid.SelectedIndex = 0;
                FirstRoomDataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void FirstRoomDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (FirstRoomDataGrid.SelectedIndex + 1 < FirstRoomDataGrid.Items.Count)
                    FirstRoomDataGrid.SelectedIndex += 1;
                else if (FirstRoomDataGrid.SelectedIndex + 1 == FirstRoomDataGrid.Items.Count)
                {
                    FirstRoomDataGrid.ScrollIntoView(FirstRoomDataGrid.Items[0]);
                    FirstRoomDataGrid.SelectedIndex = 0;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (FirstRoomDataGrid.SelectedIndex - 1 >= 0)
                    FirstRoomDataGrid.SelectedIndex -= 1;
                else if (FirstRoomDataGrid.SelectedIndex - 1 < 0)
                {
                    FirstRoomDataGrid.ScrollIntoView(FirstRoomDataGrid.Items[FirstRoomDataGrid.Items.Count - 1]);
                    FirstRoomDataGrid.SelectedIndex = FirstRoomDataGrid.Items.Count - 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                SecondRoomComboBox.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                FinishButton.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                FirstRoomComboBox.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                currentViewModel.HandleEnter();
                e.Handled = true;
            }
        }
       
    }
}
