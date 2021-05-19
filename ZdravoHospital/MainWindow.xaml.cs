using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Model;
using Model.Repository;
using Newtonsoft.Json;
using ZdravoHospital.GUI.DoctorUI;
using ZdravoHospital.GUI.ManagerUI;
using ZdravoHospital.GUI.ManagerUI.View;
using ZdravoHospital.GUI.PatientUI;
using ZdravoHospital.GUI.Secretary;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
            //Specialization s1 = new Specialization("Doctor");
            //Specialization s2 = new Specialization("Cardio surgent");
            //Model.Resources.OpenSpecializations();
            //Model.Resources.specializations.Add(s1);
            //Model.Resources.specializations.Add(s2);
            //Model.Resources.SaveSpecializations();

            //Doctor d1 = new Doctor("Marko", "Pantelic", "pantela", s1);
            //Doctor d2 = new Doctor("Nikola", "Zigic", "zigara", s2);
            //Model.Resources.doctors = new Dictionary<string, Doctor>();
            //Model.Resources.doctors[d1.Username] = d1;
            //Model.Resources.doctors[d2.Username] = d2;
            //Model.Resources.SerializeDoctors();
            //Model.Resources.accounts[d1.Username] = new Credentials(d1.Username, "pantela", RoleType.DOCTOR);
            //Model.Resources.accounts[d2.Username] = new Credentials(d2.Username, "zigara", RoleType.DOCTOR);
            //Model.Resources.SaveAccounts();

            //Patient p1 = new Patient("123", "Stefan", "Ljubovic", "ljuba@gmail.com", new DateTime(1999, 4, 23), "060123456",
            //    "ljuba", "Milojko", MaritalStatus.DIVORCED, Gender.MALE, "789", BloodType.A_POSITIVE);
            //Patient p2 = new Patient("456", "Nikolaj", "Satara", "saki@gmail.com", new DateTime(1963, 5, 17), "066987654",
            //    "saki", "Milojko", MaritalStatus.MARRIED, Gender.MALE, "624", BloodType.B_NEGATIVE);
            //Model.Resources.OpenPatients();
            //Model.Resources.patients[p1.Username] = p1;
            //Model.Resources.patients[p2.Username] = p2;
            //Model.Resources.SavePatients();

            //Model.Resources.accounts[p1.Username] = new Credentials(p1.Username, "ljuba", RoleType.PATIENT);
            //Model.Resources.accounts[p2.Username] = new Credentials(p2.Username, "saki", RoleType.PATIENT);
            //Model.Resources.SaveAccounts();

            //Period p1 = new Period(new DateTime(2021, 4, 12, 10, 0, 0), 45, PeriodType.APPOINTMENT, "patient1", "zigara", 201);
            //Period p2 = new Period(new DateTime(2021, 4, 12, 8, 0, 0), 25, PeriodType.OPERATION, "patient2", "zigara", 103);
            //Period p3 = new Period(new DateTime(2021, 4, 12, 12, 0, 0), 145, PeriodType.OPERATION, "patient1", "zigara", 103);
            //Period p4 = new Period(new DateTime(2021, 4, 19, 19, 0, 0), 145, PeriodType.APPOINTMENT, "patient2", "zigara", 201);
            //Period p5 = new Period(new DateTime(2021, 4, 21, 18, 0, 0), 30, PeriodType.APPOINTMENT, "patient1", "zigara", 201);
            //Period p6 = new Period(new DateTime(2021, 4, 13, 10, 0, 0), 20, PeriodType.APPOINTMENT, "patient2", "pantela", 201);
            //Period p7 = new Period(new DateTime(2021, 4, 16, 12, 0, 0), 30, PeriodType.APPOINTMENT, "patient1", "pantela", 201);
            //Model.Resources.OpenPeriods();
            //Model.Resources.periods.Add(p1);
            //Model.Resources.periods.Add(p2);
            //Model.Resources.periods.Add(p3);
            //Model.Resources.periods.Add(p4);
            //Model.Resources.periods.Add(p5);
            //Model.Resources.periods.Add(p6);
            //Model.Resources.periods.Add(p7);
            //Model.Resources.SavePeriods();

            //Ingredient i1 = new Ingredient("ingredient1");
            //Ingredient i2 = new Ingredient("ingredient2");
            //Ingredient i3 = new Ingredient("ingredient3");
            //Ingredient i4 = new Ingredient("ingredient4");
            //Ingredient i5 = new Ingredient("ingredient5");
            //Ingredient i6 = new Ingredient("ingredient6");
            //Medicine m1 = new Medicine("medicine1", "supplier1");
            //m1.Ingredients = new List<Ingredient>();
            //m1.Ingredients.Add(i1);
            //m1.Ingredients.Add(i2);
            //m1.Ingredients.Add(i3);
            //Medicine m2 = new Medicine("medicine2", "supplier1");
            //m2.Ingredients = new List<Ingredient>();
            //m2.Ingredients.Add(i2);
            //m2.Ingredients.Add(i4);
            //m2.Ingredients.Add(i5);
            //Medicine m3 = new Medicine("medicine3", "supplier2");
            //m3.Ingredients = new List<Ingredient>();
            //m3.Ingredients.Add(i2);
            //m3.Ingredients.Add(i5);
            //m3.Ingredients.Add(i6);
            //Model.Resources.OpenMedicines();
            //Model.Resources.medicines.Add(m1);
            //Model.Resources.medicines.Add(m2);
            //Model.Resources.medicines.Add(m3);
            //Model.Resources.SaveMedicines();

            //Model.Resources.OpenPatients();
            //foreach (Patient p in Model.Resources.patients.Values)
            //    p.Prescriptions = new List<Prescription>();
            //Model.Resources.SavePatients();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            var accountRepo = new AccountRepository();
            var account = accountRepo.GetById(username);

            try
            {
                if (account.Password.Equals(password))
                {
                    App.currentUser = username;
                    Window window = null;

                    switch (account.Role)
                    {
                        case RoleType.MANAGER:
                            window = new ManagerWindow(username);
                            break;
                        case RoleType.DOCTOR:
                            window = new DoctorWindow();
                            break;
                        case RoleType.SECERATRY:
                            window = new SecretaryWindow(username);
                            break;
                        case RoleType.PATIENT:
                            window = new PatientWindow(username);
                            break;
                    }

                    window.Show();
                    this.Close();
                } 
                else
                {
                    MessageBox.Show("Wrong password...");
                }
            } 
            catch
            {
                MessageBox.Show("Username not registered...");
            }
        }
    }
}
