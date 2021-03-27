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
        ActiveTable active;

        public ManagerWindow(Resources r)
        {
            InitializeComponent();
            managerMainTable.CanUserResizeColumns = false;
            managerMainTable.CanUserResizeRows = false;
            this.Res = r;
            this.active = ActiveTable.NOTHING;
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
            if (e.Key == System.Windows.Input.Key.E)
            {
                e.Handled = true;
                RoomAddOrEditDialog editDialog = new RoomAddOrEditDialog(this,false);
            }
            else if (e.Key == Key.Right)
                e.Handled = true;
        }

        public void showRooms()
        {
            if(active != ActiveTable.ROOMS) 
            {
                active = ActiveTable.ROOMS;
                managerMainTable.Columns.Clear();
                DataGridTextColumn dgbs = new DataGridTextColumn();
                dgbs.Header = "Id";
                dgbs.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
                dgbs.Binding = new Binding("Id");
                managerMainTable.Columns.Add(dgbs);

                DataGridTextColumn dgts = new DataGridTextColumn();
                dgts.Header = "Type";
                dgts.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
                dgts.Binding = new Binding("RoomTypeText");
                managerMainTable.Columns.Add(dgts);

                DataGridTextColumn dgis = new DataGridTextColumn();
                dgis.Header = "Name";
                dgis.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                dgis.Binding = new Binding("Name");
                managerMainTable.Columns.Add(dgis);

                DataGridTextColumn dgds = new DataGridTextColumn();
                dgds.Header = "Avaliability";
                dgds.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToHeader);
                dgds.Binding = new Binding("AvaliableText");
                managerMainTable.Columns.Add(dgds);

                drawRooms();
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
