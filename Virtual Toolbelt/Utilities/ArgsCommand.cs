using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Virtual_Toolbelt.Utilities
{
    //ICommand implementation with generic arguments
    public class ArgsCommand<T> : ICommand
    {

        private Predicate<T> condition;
        private Action<T> method;

        public event EventHandler CanExecuteChanged;

        public ArgsCommand(Action<T> m, Predicate<T> c)
        {
            method = m;
            condition = c;
        }

        public ArgsCommand(Action<T> m) : this(m, null) { }

        public bool CanExecute(object parameter)
        {
            return (condition == null) ? true : condition.Invoke((T)parameter);
        }

        public void Execute(object parameter)
        {
            method.Invoke((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}