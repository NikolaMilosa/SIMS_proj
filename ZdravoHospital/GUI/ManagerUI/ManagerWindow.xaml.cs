using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        Employee activeManager;
        Window dialog;
        DataGrid dataGrid;
        Grid grid;

        //Observable collections:
        public static ObservableCollection<Room> Rooms { get; set; }
        public static ObservableCollection<Person> Persons { get; set; }
        public static ObservableCollection<Inventory> Inventory { get; set; }

        public ManagerWindow(string au)
        {
            InitializeComponent();

            activeManager = Model.Resources.findManager(au);
            WelcomeLabel.Content += activeManager.Name;
            RoomsButton.Focus();

            this.DataContext = this;

            Model.Resources.OpenRooms();
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);
            Model.Resources.OpenInventory();
            Inventory = new ObservableCollection<Inventory>(Model.Resources.inventory.Values);
        }

        private void MainMenuGotFocus(object sender, RoutedEventArgs e)
        {
            TurnOffAllVisiblitiy();
            if (RoomsButton.IsFocused)
            {
                SubMenuRooms.Visibility = Visibility.Visible;
                RoomsMenuArrow.Visibility = Visibility.Visible;
            }
            else if (StaffButton.IsFocused)
            {
                StaffMenuArrow.Visibility = Visibility.Visible;
            }
            else if (InventoryButton.IsFocused)
            {
                SubMenuInventory.Visibility = Visibility.Visible;
                InventoryMenuArrow.Visibility = Visibility.Visible;
            }
            else if (NotificationsButton.IsFocused)
            {
                NotificationsMenuArrow.Visibility = Visibility.Visible;
            }
        }

        private void TurnOffAllVisiblitiy()
        {
            //As you add menus add here visibility turn off
            SubMenuRooms.Visibility = Visibility.Hidden;
            RoomsMenuArrow.Visibility = Visibility.Hidden;
            StaffMenuArrow.Visibility = Visibility.Hidden;
            SubMenuInventory.Visibility = Visibility.Hidden;
            InventoryMenuArrow.Visibility = Visibility.Hidden;
            NotificationsMenuArrow.Visibility = Visibility.Hidden;
        }

        private void ShowRooms_Click(object sender, RoutedEventArgs e)
        {
            ClearAllTables();
            RoomsTable.Visibility = Visibility.Visible;
        }

        private void ClearAllTables()
        {
            //As you add tables add here their turnoffers
            RoomsTable.Visibility = Visibility.Hidden;
            InventoryTable.Visibility = Visibility.Hidden;
            StaffTable.Visibility = Visibility.Hidden;
        }

        private void ShowInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllTables();
            InventoryTable.Visibility = Visibility.Visible;
        }

        private void SubMenuButtonHandler(object sender, KeyEventArgs e)
        {
            FindVisibleDataGrid();
            if (e.Key == Key.Right)
            {
                dataGrid.Focus();
                e.Handled = true;
            }
        }

        private void FindVisibleDataGrid()
        {
            if (RoomsTable.Visibility == Visibility.Visible)
                dataGrid = RoomsTable;
            else if (InventoryTable.Visibility == Visibility.Visible)
                dataGrid = InventoryTable;
            else
                dataGrid = InitialTable;
        }

        private void TableGotFocus(object sender, RoutedEventArgs e)
        {
            dataGrid.SelectedIndex = 0;
            dataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void TableKeyHandles(object sender, KeyEventArgs e)
        {
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
                    dataGrid.SelectedIndex += 1;
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
                    dataGrid.SelectedIndex -= 1;
                else if (dataGrid.SelectedIndex - 1 < 0)
                {
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                    dataGrid.SelectedIndex = dataGrid.Items.Count - 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (dataGrid.SelectedIndex != -1)
                {
                    if (RoomsTable.Visibility == Visibility.Visible)
                    {
                        dialog = new RoomAddOrEdit((Room)dataGrid.SelectedItem);
                        
                    }
                    else if (InventoryTable.Visibility == Visibility.Visible)
                    {
                        dialog = new InventoryAddOrEdit((Inventory)dataGrid.SelectedItem);
                    }
                    dialog.ShowDialog();
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                if (dataGrid.SelectedIndex != -1)
                {
                    dialog = new WarningDialog(dataGrid.SelectedItem);
                    dialog.ShowDialog();
                }
                    
            }
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            dialog = new RoomAddOrEdit();
            dialog.ShowDialog();
        }

        private void AddInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            dialog = new InventoryAddOrEdit();
            dialog.ShowDialog();
        }
    }
}
