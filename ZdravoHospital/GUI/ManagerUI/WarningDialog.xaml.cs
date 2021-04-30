using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        object _someObject;
        object[] _otherParams;

        Logics.RoomFunctions _roomFunctions;
        Logics.InventoryFunctions _inventoryFunctions;
        Logics.MedicineFunctions _medicineFunctions;

        public WarningDialog(Object o, params object[] otherParams)
        {
            InitializeComponent();
            this._someObject = o;
            this._otherParams = otherParams;

            switch (_someObject.GetType().Name)
            {
                case nameof(InventoryAddOrEdit):
                    WarningTitle.Content = "Warning! No available rooms";
                    WarningText.Text = "There are no rooms where inventory would be stored...";
                    ConfirmButton.IsEnabled = false;
                    break;
                case nameof(Room):
                    WarningTitle.Content = "Warning! Deleting a room";
                    WarningText.Text = "You are about to delete a room! If you wish to conitnue press \"Confirm\"";
                    WarningElement.Text = "Room Id : " + ((Room)_someObject).Id;
                    _roomFunctions = new Logics.RoomFunctions();
                    break;
                case nameof(Inventory):
                    WarningTitle.Content = "Warning! Deleting inventory";
                    WarningText.Text = "You are about to delete some inventory! If you wish to continue press \"Confirm\"";
                    WarningElement.Text = "Inventory Id : " + ((Inventory)_someObject).Id;
                    _inventoryFunctions = new Logics.InventoryFunctions();
                    break;
                case nameof(Ingredient):
                    WarningTitle.Content = "Warning! Deleting ingredient";
                    WarningText.Text = "You are about to delete some ingredients from medicine, but it won't be permanent just yet!";
                    WarningElement.Text = "Ingredient name : " + ((Ingredient)_someObject).IngredientName;
                    _medicineFunctions = new Logics.MedicineFunctions();
                    break;
                case nameof(Medicine):
                    WarningTitle.Content = "Warning! Deleting medicine";
                    WarningText.Text = "You are about to delete some medicine! If you wish to continue press \"Confirm\"";
                    WarningElement.Text = "Medicine name : " + ((Medicine)_someObject).MedicineName;
                    _medicineFunctions = new Logics.MedicineFunctions();
                    break;
                default:
                    WarningTitle.Content = "Warning! Deleting staff";
                    WarningText.Text = "You are about to delete a member of staff! If you wish to continue press \"Confirm\"";
                    WarningElement.Text = "Staff username : " + ((Employee)_someObject).Username;
                    break;
            }


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            object id;
            switch (_someObject.GetType().Name)
            {
                case nameof(Room):
                    result = _roomFunctions.DeleteRoom((Room)_someObject);
                    break;
                case nameof(Inventory):
                    result = _inventoryFunctions.DeleteInventory((Inventory)_someObject);
                    break;
                case nameof(Ingredient):
                    result = _medicineFunctions.DeleteIngredientFromMedicine((Ingredient)_someObject, (List<Ingredient>)_otherParams[0], (ObservableCollection<Ingredient>)_otherParams[1]);
                    break;
                case nameof(Medicine):
                    result = _medicineFunctions.DeleteMedicine((Medicine)_someObject);
                    break;
                default:
                    //Code for staff deleting
                    break;
            }

            if (!result)
            {
                MessageBox.Show("Cannot delete the room since there aren't any available rooms to store the inventory...");
            }
            this.Close();
        }
    }
}
