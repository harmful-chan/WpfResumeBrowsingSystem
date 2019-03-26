using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfResumeBrowsingSystem.Globe
{
    class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (null == this.CanExecutProperty)
            {
                return true;
            }
            this.CanExecutProperty(parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            if (null == this.ExecuteProperty)
            {
                return;
            }
            this.ExecuteProperty(parameter);
        }

        public Action<object> ExecuteProperty;
        public Func<object, bool> CanExecutProperty;
    }
}
