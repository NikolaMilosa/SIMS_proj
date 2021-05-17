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
    /// Interaction logic for InventoryAdderSubtractor.xaml
    /// </summary>
    public partial class InventoryAdderSubtractor : Window
    {
        private InventoryAdderSubtractorViewModel currentViewModel;

        public InventoryAdderSubtractor(Inventory inventory)
        {
            InitializeComponent();
            currentViewModel = new InventoryAdderSubtractorViewModel(inventory);
            this.DataContext = currentViewModel;
        }

        private void RoomComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.Left)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                if (RoomComboBox.IsDropDownOpen == false)
                    RoomComboBox.IsDropDownOpen = true;
                else
                    RoomComboBox.IsDropDownOpen = false;
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                if (RoomComboBox.IsDropDownOpen == true)
                {
                    if (RoomComboBox.SelectedIndex + 1 < RoomComboBox.Items.Count)
                        RoomComboBox.SelectedIndex += 1;
                }
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                if (RoomComboBox.IsDropDownOpen == true)
                {
                    if (RoomComboBox.SelectedIndex - 1 >= 0)
                        RoomComboBox.SelectedIndex -= 1;
                }
                e.Handled = true;
            }
        }
    }
}
