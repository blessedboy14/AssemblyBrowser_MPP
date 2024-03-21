using System.Windows.Input;

namespace WPFApp
{
    class OnClickCommand : ICommand
    {
        private Action _onClick;
        private Func<object, bool> _canExecute;

        public OnClickCommand(Action action, Func<object, bool> canExecute = null )
        {
            this._onClick = action;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);   
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            this._onClick();
        }
    }
}
