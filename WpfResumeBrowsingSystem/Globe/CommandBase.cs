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
            if (null == CanExecutFunc)
            {
                return true;
            }
            this.CanExecutFunc(parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            if (null == ExecuteAction)
            {
                return;
            }
            this.ExecuteAction(parameter);
        }

        public Action<object> ExecuteAction;
        public Func<object, bool> CanExecutFunc;
    }
}
