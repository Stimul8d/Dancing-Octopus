using System;
using System.Diagnostics;
using System.Windows.Input;

namespace DancingOctopus.ViewModels
{
    //I don't like the MVVMLight impl. It's weak actions are unpredictable
    public class BindingCommand : ICommand
    {

        readonly Action _execute;
        readonly Func<bool> _canExecute;

        public BindingCommand(Action execute)
        : this(execute, null)
        {
        }

        public BindingCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object a)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object a)
        {
            _execute();
        }
    }
}
