using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.ManagerUI.Interfaces
{
    public interface INotifiable
    {
        void OnInventoryChanged(object sender, EventArgs e);
    }
}
