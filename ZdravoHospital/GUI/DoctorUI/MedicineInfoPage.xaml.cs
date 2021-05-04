using Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for MedicineInfoPage.xaml
    /// </summary>
    public partial class MedicineInfoPage : Page, INotifyPropertyChanged
    {
        public Medicine Medicine { get; set; }
        public double NameSupplierWidth { get; set; }
        public Thickness TopSectionsMargin { get; set; }
        private TextBlock relevantTextBlock;

        public MedicineInfoPage(Medicine medicine)
        {
            InitializeComponent();

            DataContext = this;
            Medicine = medicine;

            if (NameTextBlock.Width > StatusTextBlock.Width)
                relevantTextBlock = NameTextBlock;
            else
                relevantTextBlock = StatusTextBlock;

            if (medicine.Status == MedicineStatus.REJECTED)
                RejectButton.IsEnabled = false;
            else if (medicine.Status == MedicineStatus.APPROVED)
                ApproveButton.IsEnabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopSectionsMargin = new Thickness(this.ActualWidth * 0.075, 0, this.ActualWidth * 0.075, 20);
            OnPropertyChanged("TopSectionsMargin");

            double calculatedWidth = this.ActualWidth * 0.7  - 100 - 250; // BackButton width = 100 and status part width = 250, margins take 0.3 left 
            
            if (calculatedWidth > relevantTextBlock.ActualWidth)
                NameSupplierWidth = calculatedWidth;
            else
                NameSupplierWidth = relevantTextBlock.ActualWidth;
            
            if (NameSupplierWidth > ActualWidth)
                NameSupplierWidth = ActualWidth;

            OnPropertyChanged("NameSupplierWidth");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            Medicine.Status = MedicineStatus.REJECTED;
            OnPropertyChanged("Medicine");
            RejectButton.IsEnabled = false;
            ApproveButton.IsEnabled = true;
            Model.Resources.SaveMedicines();
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            Medicine.Status = MedicineStatus.APPROVED;
            OnPropertyChanged("Medicine");
            ApproveButton.IsEnabled = false;
            RejectButton.IsEnabled = true;
            Model.Resources.SaveMedicines();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            StatusGrid.Visibility = Visibility.Collapsed;
            EditButton.Visibility = Visibility.Collapsed;
            ConfirmChangesButton.Visibility = Visibility.Visible;
            IngredientsGrid.RowDefinitions[2].Height = GridLength.Auto;
            ReplacementsGrid.RowDefinitions[2].Height = GridLength.Auto;
            NotesGrid.RowDefinitions[0].Height = GridLength.Auto;
            NotesGrid.RowDefinitions[1].Height = GridLength.Auto;
        }

        private void ConfirmChangesButton_Click(object sender, RoutedEventArgs e)
        {
            StatusGrid.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Visible;
            ConfirmChangesButton.Visibility = Visibility.Collapsed;
            IngredientsGrid.RowDefinitions[2].Height = new GridLength(0);
            ReplacementsGrid.RowDefinitions[2].Height = new GridLength(0);
            NotesGrid.RowDefinitions[0].Height = new GridLength(0);
            NotesGrid.RowDefinitions[1].Height = new GridLength(0);
        }
    }
}
