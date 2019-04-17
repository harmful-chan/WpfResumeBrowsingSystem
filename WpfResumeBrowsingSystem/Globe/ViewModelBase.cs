using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfResumeBrowsingSystem.Commands;

namespace WpfResumeBrowsingSystem.Globe
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public ICommand Closed { get; } =
            new ComCommand(p =>
            {
                ((Window)p).Close();
            });

        public ICommand Minimize { get; } =
            new ComCommand(p =>
            {
                ((Window)p).WindowState = WindowState.Minimized;
            });

        public ICommand Change { get; } =
            new ComCommand(p =>
            {
                Window tmp = p as Window;
                if (tmp.WindowState == WindowState.Maximized)
                    tmp.WindowState = WindowState.Normal;
                else tmp.WindowState = WindowState.Maximized;
            });

        public ICommand Move { get; } =
            new ComCommand(p =>
            {
                ((Window)p).DragMove();
            });
    }
}
