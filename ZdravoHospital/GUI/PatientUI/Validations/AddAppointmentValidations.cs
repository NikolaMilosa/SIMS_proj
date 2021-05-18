using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZdravoHospital.GUI.PatientUI.ViewModel;
using System.Linq;

namespace ZdravoHospital.GUI.PatientUI.Validations
{
    public class AddAppointmentValidations
    {
        public AddAppointmentPage Page { get; set; }

        public AddAppointmentValidations(AddAppointmentPage addAppointmentPage)
        {
            Page = addAppointmentPage;
        }

        public bool CheckPeriodAvailibility()
        {
            bool available = true;

            if (Validate.TrollDetected() || !IsFormFilled())
                return false;

            FillOutPeriod();

            if (!Validate.CheckPeriodAvailability(Page.Period, true) || Page.Period.RoomId == -1)
                available = false;

            return available;
        }

        public bool IsFormFilled()
        {
            bool isFilled = true;
            if (Page.selectTime.SelectedItem == null || Page.selectDate.SelectedDate == null || Page.selectDoctor.SelectedItem == null)
            {
                isFilled = false;
                Validate.ShowOkDialog("Warning", "Please select doctor, date and time when you want to schedule appointment!");
            }
            return isFilled;
        }

        public void SerializePeriod()
        {
            if (Page.Mode)
                SerializeNewPeriod();
            else
                Validate.ShowOkDialog("Appointment", "Appointment is succesfully edited!");

            Resources.SavePeriods();
        }

        public void SerializeNewPeriod()
        {
            Resources.OpenPeriods();
            Resources.periods.Add(Page.Period);
            Validate.ShowOkDialog("Appointment", "Appointment is succesfully added!");
        }

        public void FillOutPeriod()
        {
            Page.Period.StartTime = Page.Period.StartTime.Date + (TimeSpan)Page.selectTime.SelectedItem;
            Page.Period.DoctorUsername = ((DoctorView)Page.selectDoctor.SelectedItem).Username;
            Page.Period.RoomId = Validate.GetFreeRoom(Page.Period);
        }

        public void FillDoctorList()
        {
            Resources.OpenDoctors();
            RestartDoctorList();

            foreach (Doctor doctor in Resources.doctors.Values)
            {
                if (doctor.SpecialistType.SpecializationName.Equals("Doctor"))
                    Page.DoctorList.Add(new DoctorView(doctor));
            }
        }

        public void RestartDoctorList()
        {
            if (Page.DoctorList == null)
                Page.DoctorList = new ObservableCollection<DoctorView>();
            else
                Page.DoctorList.Clear();
        }

        public void GeneratePeriod(Period period, string username)
        {
            if (Page.Mode)
                GenerateNewPeriod(username);
            else
                GenerateOldPeriod(period);

            Page.selectDate.DisplayDateStart = DateTime.Today.AddDays(3);
        }

        public void GenerateNewPeriod(string username)
        {
            Page.Period = new Period();
            Page.Period.PatientUsername = username;
            Page.Period.Duration = 30;
            Page.Period.PeriodId =GeneratePeriodId();
        }

        private int GeneratePeriodId()
        {
            PeriodRepository periodRepository = new PeriodRepository();
            if (periodRepository.GetValues().Count == 0)
                return 0;

            return periodRepository.GetValues().Last().PeriodId+1;//vrati vrednost za jedan vecu od poslednjeg id-a iz liste
        }

        public void GenerateOldPeriod(Period period)
        {
            Page.Period = period;
            Page.selectDate.SelectedDate = Page.Period.StartTime;
            Page.selectDoctor.SelectedItem = GetDoctor(Page.Period.DoctorUsername);
            Page.selectTime.SelectedItem = Page.Period.StartTime.TimeOfDay;
        }

        public DoctorView GetDoctor(string username)
        {
            foreach (DoctorView doctor in Page.DoctorList)
            {
                if (doctor.Username.Equals(username))
                    return doctor;
            }

            return null;
        }
    }


}
