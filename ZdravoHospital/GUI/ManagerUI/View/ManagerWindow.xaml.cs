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
        private Window dialog;

        public ManagerWindow(string au)
        {
            InitializeComponent();
            currentViewModel = new ManagerWindowViewModel(au);
            this.DataContext = currentViewModel;
            RoomsButton.Focus();
        }

        private void TableGotFocus(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = FindDataGrid();

            if (dataGrid.Items.Count > 0)
            {
                try
                {
                    dataGrid.SelectedIndex = 0;
                    dataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                catch { }

            }
        }

        private void TableKeyHandles(object sender, KeyEventArgs e)
        {
            DataGrid dataGrid = FindDataGrid();

            if (e.Key == Key.Left)
            {
                if (SubMenuRooms.Visibility == Visibility.Visible)
                    ShowRoomsButton.Focus();
                else if (SubMenuInventory.Visibility == Visibility.Visible)
                    ShowInventoryButton.Focus();
                else
                    RoomsButton.Focus();

                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (dataGrid.SelectedIndex + 1 < dataGrid.Items.Count)
                {
                    dataGrid.SelectedIndex += 1;
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.SelectedIndex]);
                }
                else if (dataGrid.SelectedIndex + 1 == dataGrid.Items.Count)
                {
                    dataGrid.ScrollIntoView(dataGrid.Items[0]);
                    dataGrid.SelectedIndex = 0;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (dataGrid.SelectedIndex - 1 >= 0)
                {
                    dataGrid.SelectedIndex -= 1;
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.SelectedIndex]);
                }
                else if (dataGrid.SelectedIndex - 1 < 0)
                {
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                    dataGrid.SelectedIndex = dataGrid.Items.Count - 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                currentViewModel.HandleEnterClick();
                e.Handled = true;
            }
        }

        private void SubMenuButtonHandler(object sender, KeyEventArgs e)
        {
            DataGrid dataGrid = FindDataGrid();
            
            if (e.Key == Key.Right)
            {
                dataGrid.Focus();
                e.Handled = true;
            }
        }

        private DataGrid FindDataGrid()
        {
            string visibleTable = currentViewModel.FindVisibleDataGrid();

            if (visibleTable.Contains("room"))
                return RoomsTable;
            else if (visibleTable.Contains("inventory"))
                return InventoryTable;
            else if (visibleTable.Contains("medicine"))
                return MedicineTable;
            else
                return InitialTable;
        }
    }
}
