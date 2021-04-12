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

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for WarningDialog.xaml
    /// </summary>
    public partial class WarningDialog : Window
    {
        Object someObject;
        public WarningDialog(Object o)
        {
            InitializeComponent();
            this.someObject = o;

            switch (someObject.GetType().Name)
            {
                case nameof(InventoryAddOrEdit):
                    WarningTitle.Content = "Warning! No available rooms";
                    WarningText.Text = "There are no rooms where inventory would be stored...";
                    ConfirmButton.IsEnabled = false;
                    break;
                case nameof(Room):
                    WarningTitle.Content = "Warning! Deleting a room";
                    WarningText.Text = "You are about to delete a room! If you wish to conitnue press \"Confirm\"";
                    WarningElement.Text = "Room Id : " + ((Room)someObject).Id;
                    break;
                case nameof(Inventory):
                    WarningTitle.Content = "Warning! Deleting inventory";
                    WarningText.Text = "You are about to delete some inventory! If you wish to continue press \"Confirm\"";
                    WarningElement.Text = "Inventory Id : " + ((Inventory)someObject).Id;
                    break;
                default:
                    WarningTitle.Content = "Warning! Deleting staff";
                    WarningText.Text = "You are about to delete a member of staff! If you wish to continue press \"Confirm\"";
                    WarningElement.Text = "Staff username : " + ((Employee)someObject).Username;
                    break;
            }


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            object id;
            switch (someObject.GetType().Name)
            {
                case nameof(Room):
                    id = ((Room)someObject).Id;
                    Model.Resources.rooms.Remove((int)id);
                    ManagerWindow.Rooms.Remove((Room)someObject);
                    Model.Resources.SerializeRooms();
                    break;
                case nameof(Inventory):
                    id = ((Inventory)someObject).Id;
                    Model.Resources.inventory.Remove((string)id);
                    ManagerWindow.Inventory.Remove((Inventory)someObject);
                    Model.Resources.SerializeInventory();

                    foreach(Room room in Model.Resources.rooms.Values)
                    {
                        if (room.Inventory.ContainsKey(id.ToString()))
                            room.Inventory.Remove(id.ToString());
                    }
                    Model.Resources.SerializeRooms();

                    break;
                default:
                    //Code for staff deleting
                    break;
            }
            this.Close();
        }
    }
}
