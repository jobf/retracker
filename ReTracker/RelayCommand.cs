using System;
using System.Windows.Input;

namespace ReTracker
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        private readonly Action<object> _execute;
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        private readonly Func<object, bool> _canExecute;
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}