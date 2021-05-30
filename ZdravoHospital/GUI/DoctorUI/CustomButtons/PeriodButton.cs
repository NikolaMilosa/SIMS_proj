using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.DoctorUI
{
    public class PeriodButton : Button
    {
        public Period Period { get; set; }
        public TextBlock UpperText { get; set; }
        public TextBlock LowerText { get; set; }

        public PeriodButton()
        {
            Style = Application.Current.Resources["BlueButton"] as Style;
            StackPanel panel = new StackPanel();
            UpperText = new TextBlock()
            {
                FontSize = 17,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            LowerText = new TextBlock()
            {
                FontSize = 17,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            panel.Children.Add(UpperText);
            panel.Children.Add(LowerText);
            Content = panel;
        }
    }
}
