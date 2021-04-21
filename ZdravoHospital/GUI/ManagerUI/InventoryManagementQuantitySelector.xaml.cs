using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for InventoryManagementQuantitySelector.xaml
    /// </summary>
    public partial class InventoryManagementQuantitySelector : Window, INotifyPropertyChanged
    {
        //Fields:
        private int _maxInventory;
        private string _definitionText;
        private int _enteredQuantity;

        public int EnteredQuantity
        {
            get { return _enteredQuantity; }
            set
            {
                _enteredQuantity = value;
                OnPropertyChanged("EnteredQuantity");
            }
        }

        public int MaxInventory
        {
            get { return _maxInventory; }
            set
            {
                _maxInventory = value;
                OnPropertyChanged("MaxInventory");
            }
        }

        public string DefinitionText
        {
            get { return _definitionText; }
            set
            {
                _definitionText = value;
                OnPropertyChanged("DefinitionText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        //Helpers:
        private Room firstRoom;
        private Room secondRoom;
        private ObservableCollection<InventoryDTO> firstRoomDTOs;
        private ObservableCollection<InventoryDTO> secondRoomDTOs;
        private string id;

        public InventoryManagementQuantitySelector(Room fr, Room sr, ObservableCollection<InventoryDTO> fri, ObservableCollection<InventoryDTO> sri, string id)
        {
            InitializeComponent();
            this.DataContext = this;

            this.firstRoom = fr;
            this.secondRoom = sr;
            this.firstRoomDTOs = fri;
            this.secondRoomDTOs = sri;
            this.id = id;

            /* TODO : */

            DefinitionText = "Out of '" + MaxInventory + "' possible how many '" + Model.Resources.inventory[id].Name + "' would you like to transfer?";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
           /* TODO : */
        }
    }
}
