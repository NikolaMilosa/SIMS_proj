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
            someObject = o;
            Type objectType = someObject.GetType();

            switch (objectType.Name)
            {
                case nameof(Room):
                    TitleLabel.Content = "Warning! Deleting a room";
                    MainText.Text = "You are deleting a room with ID : " + ((Room)someObject).Id.ToString() + "! If you wish to continue press \"Confirm\"";
                    break;
                case nameof(Inventory):
                    TitleLabel.Content = "Warning! Deleting inventory!";
                    MainText.Text = "You are deleting inventory with ID : " + ((Inventory)someObject).Name + "! If you wish to continue press \"Confirm\"";
                    break;
                default:
                    TitleLabel.Content = "Warning! Deleting staff!";
                    MainText.Text = "You are deleting staff member with USERNAME : " + ((Person)someObject).Username + "! If you wish to continue press \"Confirm\"";
                    break;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Type objectType = someObject.GetType();
            switch (objectType.Name)
            {
                case nameof(Room):
                    int id = ((Room)someObject).Id;
                    Model.Resources.rooms.Remove(id);
                    ManagerWindow.oRooms.Remove((Room)someObject);
                    Model.Resources.SerializeRooms();
                    this.Close();
                    break;
                case nameof(Inventory):
                    string name = ((Inventory)someObject).Name;
                    Model.Resources.inventory.Remove(name);
                    ManagerWindow.oInventory.Remove((Inventory)someObject);
                    Model.Resources.SerializeInventory();
                    this.Close();
                    break;
                default:
                    //Code for Staff deleting
                    break;
            }
        }
    }
}
