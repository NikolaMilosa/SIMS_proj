using System;
using System.Collections.Generic;
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

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    
    public partial class ManagerWindow : Window
    {
        public enum ActiveTable { ROOMS, STAFF, EQUIPMENT, NOTHING }
        public Resources Res { get; set; }
        public ActiveTable Active { get; set; }

        public ManagerWindow(Resources r)
        {
            InitializeComponent();
            managerMainTable.CanUserResizeColumns = false;
            managerMainTable.CanUserResizeRows = false;
            this.Res = r;
            this.Active = ActiveTable.NOTHING;
        }

        private void roomButton_Click(object sender, RoutedEventArgs e)
        {
            SubMenu.Content = new SubMenuRooms(this);
        }

        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Res.serialize();
        }

        private void keyPressed(object sender, KeyEventArgs e)
        { 
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                RoomAddOrEditDialog editDialog = new RoomAddOrEditDialog(this,false);
            }
            else if (e.Key == Key.Right)
                e.Handled = true;
            else if (e.Key == Key.Left)
            {
                e.Handled = true;
                SubMenu.Focus();
            }
            else if (e.Key == Key.Delete)
            {
                e.Handled = true;
                Warning warnDialog = new Warning(this);
                warnDialog.Show();
            }
        }

        public void showRooms()
        {
            if(Active != ActiveTable.ROOMS) 
            {
                Active = ActiveTable.ROOMS;
                managerMainTable.Columns.Clear();
                DataGridTextColumn dgbs = new DataGridTextColumn();
                dgbs.Header = "Id";
                dgbs.Binding = new Binding("Id");
                
                DataGridTextColumn dgts = new DataGridTextColumn();
                dgts.Header = "Type";
                dgts.Binding = new Binding("RoomTypeText");
                
                DataGridTextColumn dgis = new DataGridTextColumn();
                dgis.Header = "Name";
                dgis.Binding = new Binding("Name");

                DataGridTextColumn dgds = new DataGridTextColumn();
                dgds.Header = "Avaliability";
                dgds.Binding = new Binding("AvaliableText");

                managerMainTable.Columns.Add(dgbs);
                managerMainTable.Columns.Add(dgts);
                managerMainTable.Columns.Add(dgds);
                managerMainTable.Columns.Add(dgis);

                drawRooms();

                dgis.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            
        }
        public void drawRooms()
        {
            managerMainTable.Items.Clear();
            foreach (AppointmentRoom ap in Res.AppointmentRooms.Values)
                managerMainTable.Items.Add(ap);
            foreach (OperatingRoom op in Res.OperatingRooms.Values)
                managerMainTable.Items.Add(op);
            foreach (Room r in Res.StorageAndBedRooms.Values)
                managerMainTable.Items.Add(r);
        }
    }
}
