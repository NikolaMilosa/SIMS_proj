using System;
using System.Collections.Generic;
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
using ZdravoHospital.GUI.DoctorUI;
using ZdravoHospital.GUI.ManagerUI;
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
            Model.Resources.OpenAccounts();

            //Doctor d1 = new Doctor("Marko", "Pantelic", "pantela", "Doctor");
            //Doctor d2 = new Doctor("Nikola", "Zigic", "zigara", "Cardio surgent");
            //Model.Resources.doctors = new Dictionary<string, Doctor>();
            //Model.Resources.doctors[d1.Username] = d1;
            //Model.Resources.doctors[d2.Username] = d2;
            //Model.Resources.accounts[d1.Username] = new Credentials(d1.Username, "pantela", RoleType.DOCTOR);
            //Model.Resources.accounts[d2.Username] = new Credentials(d2.Username, "zigara", RoleType.DOCTOR);
            //Model.Resources.SerializeDoctors();
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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String username = UsernameTextBox.Text;
            String password = PasswordTextBox.Password;

            try
            {
                if (Model.Resources.accounts[username].Password.Equals(password))
                {
                    App.currentUser = username;
                    Window window = null;

                    switch (Model.Resources.accounts[username].Role)
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
