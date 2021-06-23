using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace refactorSimpleSnake.WpfTest
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Action<object> execute { get; }
        private Func<object,bool> canExecute { get;}
        public DelegateCommand(Action<object> execute,Func<object,bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            if (canExecute != null && !canExecute.Invoke(parameter)) return false;
            return true;
        }

        public void Execute(object parameter)
        {                
            if (CanExecute(parameter)) execute.Invoke(parameter);
        }
    }
}
