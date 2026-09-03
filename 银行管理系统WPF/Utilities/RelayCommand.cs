using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 银行管理系统WPF.Utilities
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged 
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Predicate<object> _canExecute;
        private Action<object> _execute;
        public RelayCommand(Action<object> c)
        {
            this._execute = c;
        }

        public RelayCommand(Action<object> c, Predicate<object> d)
        {
            this._execute = c;
            this._canExecute = d;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
