using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace ZdravoHospital.GUI.ManagerUI.Commands
{
    class MyBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ((DataGrid) sender).SelectedIndex = 0;
            }
            catch
            {
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }
    }

    class MyFocusBehaviour : Behavior<Button>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_GotFocus;
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Button)sender).Focus();
            }
            catch
            {
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_GotFocus;
        }
    }
}
