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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for SubMenuRooms.xaml
    /// </summary>
    public partial class SubMenuRooms : Page
    {

        Resources res;
        DataGrid managerMainTable;
        public SubMenuRooms(Resources r, DataGrid mmt)
        {
            InitializeComponent();
            this.res = r;
            this.managerMainTable = mmt;
        }

        private void showButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridTextColumn dgbs = new DataGridTextColumn();
            dgbs.Header = "Broj sobe";
            dgbs.Binding = new Binding("Id");
            managerMainTable.Columns.Add(dgbs);

            DataGridTextColumn dgts = new DataGridTextColumn();
            dgts.Header = "Tip sobe";
            dgts.Binding = new Binding("RoomTypeText");
            managerMainTable.Columns.Add(dgts);

            DataGridTextColumn dgis = new DataGridTextColumn();
            dgis.Header = "Ime sobe";
            dgis.Binding = new Binding("Name");
            managerMainTable.Columns.Add(dgis);

            DataGridTextColumn dgds = new DataGridTextColumn();
            dgds.Header = "Dostupnost";
            dgds.Binding = new Binding("AvaliableText");
            managerMainTable.Columns.Add(dgds);

            foreach (AppointmentRoom ap in res.AppointmentRooms.Values)
                managerMainTable.Items.Add(ap);
            foreach (OperatingRoom op in res.OperatingRooms.Values)
                managerMainTable.Items.Add(op);
            foreach (Room r in res.StorageAndBedRooms.Values)
                managerMainTable.Items.Add(r);
        }

        private void addRoomButton_Click(object sender, RoutedEventArgs e)
        {
            RoomAddOrEditDialog addDialog = new RoomAddOrEditDialog(this.res,true);
            addDialog.roomIdTextBox.IsEnabled = true;
            addDialog.Show();
        }
    }
}
