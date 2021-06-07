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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for CustomYesNoDialog.xaml
    /// </summary>
    public partial class CustomYesNoDialog : Window
    {
        public MessageBoxDTO MessageBoxContent { get; set; }

        public CustomYesNoDialog(string title, string content)
        {
            InitializeComponent();
            this.DataContext = this;
            MessageBoxContent = new MessageBoxDTO(title, content);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void NotOkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
