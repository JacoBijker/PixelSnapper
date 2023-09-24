using System;
using System.Windows.Input;

namespace PixelSnapper.WPF.Models
{
    public class DelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<T> toExecute;

        public DelegateCommand(Action<T> toExecute)
        {
            this.toExecute = toExecute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            toExecute.Invoke((T)parameter);
        }
    }
}
