using System;
using System.Windows.Input;

namespace Chicken.ViewModel
{
    public class DelegateCommand : ICommand
    {
        public Action<object> ExecuteAction { get; set; }
        public Func<object, bool> CanExecuteFunc { get; set; }

        public DelegateCommand(Action executeAction)
        {
            this.ExecuteAction = (obj) => executeAction();
        }

        public DelegateCommand(Action<object> executeAction)
        {
            this.ExecuteAction = executeAction;
        }
        public DelegateCommand(Action<object, EventArgs> executeAction)
        {
            this.ExecuteAction = (obj) => executeAction(obj, null);
        }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteFunc == null)            
                return true;            
            return this.CanExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.ExecuteAction == null)            
                return;            
            this.ExecuteAction(parameter);
        }
    }
}
