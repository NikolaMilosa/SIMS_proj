using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Commands;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class HelpPageVM : ViewModel
    {
        #region Properties

        private string _shownText;
        public string ShownText
        {
            get => _shownText;
            set
            {
                _shownText = value;
                OnPropertyChanged("ShownText");
            }}

        #endregion

        #region Constructors

        public HelpPageVM()
        {
            SetCommands();
            PeriodsExecute(null);
        }

        #endregion

        #region Commands

        public RelayCommand PeriodsCommand { get; private set; }
        public RelayCommand AddPeriodCommand { get; private set; }
        public RelayCommand EditPeriodCommand { get; private set; }
        public RelayCommand RemovePeriodCommand { get; private set; }
        public RelayCommand HistoryPeriodCommand { get; private set; }
        public RelayCommand RatePeriodCommand { get; private set; }
        public RelayCommand TherapyCommand { get; private set; }
        public RelayCommand NotificationCommand { get; private set; }
        public RelayCommand NoteCommand { get; private set; }



        #endregion

        #region CommandActions

        private void NoteExecute(object paramter)
        {
            ShownText = @"If you wish to view your notes please select sixth item named 'Notes' in the dropping vertical menu which is located in the top left corner of the screen. 
Page containing notes will be shown in the right side of the screen. From where you can read every individual note by pressing them or add them using button 'Write note' located
in the right bottom corner of the screen";
        }

        private void NotificationExecute(object parameter)
        {
            ShownText = @"If you wish to view your notifications please select sixth item named 'Notifications' in the dropping vertical menu which is located in the top left corner of the screen. 
            Page containing notifications will be shown in the right side of the screen. From where you can read every individual notification by pressing them.";
        }

        private void HistoryExecute(object parameter)
        {
            ShownText = "If you wish to view your appoinments history please select third item named 'Appointments history' in the dropping vertical menu which is located in the top left corner of the screen."+ 
           " Page containing appointments history  will be shown in the right side of the screen. From where you can view all the information about previous appointments like anamnesis by clicking anamnesis button"+
           " located in the same row as appointment's informations.";
        }
        private void RemoveExecute(object parameter)
        {
            ShownText =
                "If you wish to remove appoinment please select first item named 'Appointments' in the dropping vertical menu which is located in the top left corner of the screen. " +
                " Page containing appointment view will be shown in the right side of the screen. Shown page is also a homepage for the application and it will be shown every time the application is opened." +
                "  In the shown page you can remove appointment by pressing cancel button located in the same row as appointment's informations and then confirming removal by pressing yes.";
        }
        private void PeriodsExecute(object parameter)
        {
            ShownText = @"If you wish to view your appoinments please select first item named 'Appointments' in the dropping vertical menu which is located in the top left corner of the screen. 
            Page containing appointments will be shown in the right side of the screen. Shown page is also a homepage for the application and it will be shown every time the application is opened.
          If you wish to manipulate with appointments you can:
            1.Add appointment
            2.Remove appointment
            3.Edit appointment
            4.View appointmets history";
        }

        private void AddExecute(object parameter)
        {
            ShownText =
                "If you wish to add appoinment please select second item named 'Add appointment' in the dropping vertical menu which is located in the top left corner of the screen. " +
                "Page containing form for adding appointment will be shown in the right side of the screen.From there you have to options:\n" +
                "\t1.1.Fill form yourself" +
                "\n\t1.2.Use suggested appointments based on your input\n" +
                "\n1.1" +
                "\n\tFirst select doctor, then you select date which suits you.And lastly you have to select exact time.After filling the form click confirm and you have succesfully" +
                " added new appointment." +
                "\n1.2" +
                "\n\tFirst you have to select doctor or time then press suggest button located in the bottom right corener of the screen.Page containing suggested appointments" +
                " will be shown from where you can confirm which appointment you want to add by clicking confirm button located in the same row as appointment's informations.";
        }

        private void EditExecute(object parameter)
        {
            ShownText = "If you wish to edit appoinment please select first item named 'Appointments' in the dropping vertical menu which is located in the top left corner of the screen." +
            "Page containing appointment view will be shown in the right side of the screen. Shown page is also a homepage for the application and it will be shown every time the application is opened." +
                "In the shown page you can remove appointment by pressing edit button located in the same row as appointment's informations which will show you add appointment page(look at section add appointment).";
        }

        private void RateExecute(object parameter)
        {
            ShownText =
                "If you wish to rate appointment please select fifth item named 'Appointments history' in the dropping vertical menu which is located in the top left corner of the screen. " +
                "Page containing form for rating will be shown in the right side of the screen when 'Rate' button is clicked which is located in the same row as the appointment." +
                "Then select grade that you want to give him(stars from 1 - 5) and lastly give your comment about the appointment. " +
                "You can submit your assesment by pressing confirm button located in the bottom of the screen.";
        }

        private void TherapyExecute(object parameter)
        {
            ShownText =
                @"If you wish to view your therapies please select fourth item named 'Therapies' in the dropping vertical menu which is located in the top left corner of the screen. 
            Page containing weekly calendar view of therapies on will be shown in the right side of the screen.";
        }

        #endregion

        #region Methods

        private void SetCommands()
        {
            PeriodsCommand = new RelayCommand(PeriodsExecute);
            AddPeriodCommand = new RelayCommand(AddExecute);
            EditPeriodCommand = new RelayCommand(EditExecute);
            RemovePeriodCommand = new RelayCommand(RemoveExecute);
            HistoryPeriodCommand = new RelayCommand(HistoryExecute);
            RatePeriodCommand = new RelayCommand(RateExecute);
            TherapyCommand = new RelayCommand(TherapyExecute);
            NotificationCommand = new RelayCommand(NotificationExecute);
            NoteCommand = new RelayCommand(NoteExecute);
        }

        #endregion

    }
}
