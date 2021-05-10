using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        Employee activeManager;
        public static Window dialog;
        DataGrid dataGrid;

        public static ObservableCollection<Room> Rooms { get; set; }
        public static ObservableCollection<Person> Persons { get; set; }
        public static ObservableCollection<Inventory> Inventory { get; set; }
        public static ObservableCollection<Medicine> Medicines { get; set; }

        Logics.TransferRequestsFunctions transferRequestsFunctions;
        Logics.RoomScheduleFunctions roomScheduleFunctions;


        public ManagerWindow(string au)
        {
            InitializeComponent();
            /* Account greeting setting */
            activeManager = Model.Resources.findManager(au);
            WelcomeLabel.Content += activeManager.Name;
            
            /* Initial focus */
            RoomsButton.Focus();

            this.DataContext = this;

            this.transferRequestsFunctions = new Logics.TransferRequestsFunctions();
            this.roomScheduleFunctions = new Logics.RoomScheduleFunctions();

            /* Opening database */
            Model.Resources.OpenRoomInventory();
            Model.Resources.OpenRooms();
            Model.Resources.OpenInventory();
            Model.Resources.OpenTransferRequests();
            Model.Resources.OpenMedicines();
            Model.Resources.OpenPeriods();
            Model.Resources.OpenRoomSchedule();
            Model.Resources.OpenMedicineRecensions();
            Model.Resources.OpenDoctors();

            /* Handling visuals */
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);
            Inventory = new ObservableCollection<Inventory>(Model.Resources.inventory.Values);
            Medicines = new ObservableCollection<Medicine>(Model.Resources.medicines);

            /* Handling transfer requests */
            transferRequestsFunctions.RunOrExecute();

            /* Handling room renovation */
            roomScheduleFunctions.RunOrExecute();
        }

        private void TurnOffTables()
        {
            RoomsTable.Visibility = Visibility.Hidden;
            StaffTable.Visibility = Visibility.Hidden;
            InventoryTable.Visibility = Visibility.Hidden;
            MedicineTable.Visibility = Visibility.Hidden;
        }

        private void ShowRooms_Click(object sender, RoutedEventArgs e)
        {
            TurnOffTables();
            RoomsTable.Visibility = Visibility.Visible;
        }

        private void ShowInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            TurnOffTables();
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
            else if (MedicineTable.Visibility == Visibility.Visible)
                dataGrid = MedicineTable;
            else
                dataGrid = InitialTable;
        }

        private void TableGotFocus(object sender, RoutedEventArgs e)
        {
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
                    else if (MedicineTable.Visibility == Visibility.Visible)
                    {
                        dialog = new AddOrEditMedicine((Medicine)dataGrid.SelectedItem);
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
                    e.Handled = true;
                }
                    
            }
            else if (e.Key == Key.F)
            {
                if (Model.Resources.inventory.Count != 0 && InventoryTable.Visibility == Visibility.Visible)
                {
                    dialog = new FilterDialog();
                    dialog.ShowDialog();
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Add)
            {
                if (InventoryTable.Visibility == Visibility.Visible && InventoryTable.SelectedIndex != -1)
                {
                    dialog = new InventoryAdderSubtractor((Inventory)InventoryTable.SelectedItem);
                    dialog.ShowDialog();
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.S)
            {
                if (MedicineTable.Visibility == Visibility.Visible)
                {
                    Medicine selectedMedicine = (Medicine)MedicineTable.SelectedItem;
                    if (selectedMedicine.Status != MedicineStatus.APPROVED && selectedMedicine.Status != MedicineStatus.PENDING)
                    {
                        dialog = new ValidationRequestDialog(selectedMedicine);
                        dialog.ShowDialog();
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.R)
            {
                if (MedicineTable.Visibility == Visibility.Visible)
                {
                    Medicine selectedMedicine = (Medicine) MedicineTable.SelectedItem;
                    if (selectedMedicine.Status == MedicineStatus.REJECTED)
                    {
                        dialog = new RejectionNoteDialog(selectedMedicine);
                        dialog.ShowDialog();
                        e.Handled = true;
                    }
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

        private void ManageInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            dialog = new InventoryManagementWindow();
            dialog.ShowDialog();
        }

        private void ShowMedicineButton_Click(object sender, RoutedEventArgs e)
        {
            TurnOffTables();
            MedicineTable.Visibility = Visibility.Visible;
        }

        private void AddMedicineButton_Click(object sender, RoutedEventArgs e)
        {
            dialog = new AddOrEditMedicine();
            dialog.ShowDialog();
        }

        private void PlanRenovation_Click(object sender, RoutedEventArgs e)
        {
            dialog = new RoomRenovation();
            dialog.ShowDialog();
        }
    }
}
