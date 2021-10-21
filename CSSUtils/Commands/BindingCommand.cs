using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSSUtils.Commands
{
    public class BindingCommand : ICommand
    {
        readonly Action<object> _executeWithParameter;
        readonly Action _execute;
        readonly Predicate<object> _canExecute;

        public BindingCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _executeWithParameter = execute;
            _canExecute = canExecute;
        }

        public BindingCommand(Action execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || this._canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter = null) 
        {
            if (parameter == null)
            {
                _execute();
            }
            else
            {
                _executeWithParameter(parameter);
            }
        }
    }
}
