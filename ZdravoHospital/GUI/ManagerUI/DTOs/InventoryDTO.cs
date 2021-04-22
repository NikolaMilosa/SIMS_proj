using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.DTOs
{
    public class InventoryDTO : INotifyPropertyChanged
    {
        //Fields:
        private string _name;
        private int _quantity;
        private string _id;
        private InventoryType _inventoryType;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public InventoryType InventoryType
        {
            get { return _inventoryType; }
            set
            {
                _inventoryType = value;
                OnPropertyChanged("InventoryType");
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

        public InventoryDTO(string n, int q, string i, InventoryType it)
        {
            this.Name = n;
            this.Quantity = q;
            this.Id = i;
            this.InventoryType = it;
        }
    }
}
