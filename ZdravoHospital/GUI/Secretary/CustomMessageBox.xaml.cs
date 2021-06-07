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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    
    public partial class CustomMessageBox : Window
    {
        public MessageBoxDTO MessageBoxContent { get; set; }
        public CustomMessageBox(string title, string content)
        {
            InitializeComponent();
            this.DataContext = this;
            //this.Owner = SecretaryWindowVM.SecretaryWindow;
            MessageBoxContent = new MessageBoxDTO(title, content);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
