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

        /// <summary>
        /// Generates a binding command for any controller that suports Commands. This method can be used to pass parameters.
        /// </summary>
        /// <param name="execute">Name of the Callback Method that will be called when the controller is used by the user</param>
        /// <param name="canExecute">Predicate Method or Lamdba Expression that will determinate if the Callback Method should be executed or not</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BindingCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _executeWithParameter = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Generates a binding command for any controller that suports Commands. This methods do not use parameters.
        /// </summary>
        /// <param name="execute">Name of the Callback Method that will be called when the controller is used by the user</param>
        /// <param name="canExecute">Predicate Method or Lamdba Expression that will determinate if the Callback Method should be executed or not</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BindingCommand(Action execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Execute the Method or Lambda Expression that determinates if the Callback should be called.
        /// </summary>
        /// <param name="parameter">Method or Lambda Expression</param>
        /// <returns>TRUE if the Callback should be called, FALSE otherwise</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || this._canExecute(parameter);
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Calls the Callback Method of the Command and if the parameter is diferent than null, this will also be passed
        /// </summary>
        /// <param name="parameter">The parameter that will be passed to the Callback Method</param>
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
