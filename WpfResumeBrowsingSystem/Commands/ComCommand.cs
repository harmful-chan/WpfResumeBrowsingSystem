﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfResumeBrowsingSystem.Globe;

using System.Windows;
namespace WpfResumeBrowsingSystem.Commands
{
    class ComCommand : CommandBase
    {
        public ComCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            base.ExecuteProperty = execute;
            base.CanExecutProperty = canExecute;
        }
    }
}
