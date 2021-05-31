using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ZdravoHospital.GUI.Secretary.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> executeAction;
        private Predicate<object> canExecute;

        public RelayCommand(Action<object> execute)
        {
            executeAction = execute;
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExec)
        {
            executeAction = execute;
            canExecute = canExec;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;
            return canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
    }
}
