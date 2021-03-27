using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Newtonsoft.Json;


namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : Window
    {
        public Dictionary<String, Patient> Patients { get; set; }
        public ObservableCollection<Patient> PatientsForTable { get; set; }
        public ObservableCollection<Patient>  dictionaryToList(Dictionary<String, Patient> Patients)
        {
            ObservableCollection<Patient> ret = new ObservableCollection<Patient>();
            foreach(KeyValuePair<string, Patient> pair in Patients)
            {
                ret.Add(pair.Value);
            }
            return ret;
        }
        public PatientsView()
        {
            InitializeComponent();
            this.DataContext = this;
            Patients = JsonConvert.DeserializeObject<Dictionary<string, Patient>>(File.ReadAllText(@"..\..\..\Resources\patients.json"));
            PatientsForTable = dictionaryToList(Patients);
        }

        private void btnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatient = ((Patient)patientsDataGrid.SelectedItem);

            if(selectedPatient == null)
            {
                MessageBox.Show("Nothing is selected. Please select a patient.");
            }
            else
            {
                if (selectedPatient.IsGuest)
                {
                    PatientsForTable.Remove(selectedPatient);
                    Patients.Remove("guest_" + selectedPatient.HealthCardNumber);
                    string patientsJson = JsonConvert.SerializeObject(Patients);
                    File.WriteAllText(@"..\..\..\Resources\patients.json", patientsJson);
                    return;
                }
                else
                {
                    ///////     DELETE FROM TABLE AND LIST OF PATIENTS      //////
                    PatientsForTable.Remove(selectedPatient);
                    Patients.Remove(selectedPatient.Username);
                    string patientsJson = JsonConvert.SerializeObject(Patients);
                    File.WriteAllText(@"..\..\..\Resources\patients.json", patientsJson);

                    ///////     DELETE FROM ACCOUNTS    ////////
                    Dictionary<string, Credentials> accounts = new Dictionary<string, Credentials>();
                    accounts = JsonConvert.DeserializeObject<Dictionary<string, Credentials>>(File.ReadAllText(@"..\..\..\Resources\accounts.json"));
                    accounts.Remove(selectedPatient.Username);
                    string accountsJson = JsonConvert.SerializeObject(accounts);
                    File.WriteAllText(@"..\..\..\Resources\accounts.json", accountsJson);
                }
                
            }
            
        }
    }
}
