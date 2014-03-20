using System;
using System.Windows.Input;

namespace UD2
{
    public class BasicCommand : ICommand
    {
        private readonly Action _action;

        public BasicCommand(Action action)
        {
            this._action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._action();
        }

        public event EventHandler CanExecuteChanged;
    }
}