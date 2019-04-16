using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfResumeBrowsingSystem.Commands;
using WpfResumeBrowsingSystem.Globe;

namespace WpfResumeBrowsingSystem.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        private static readonly HttpClient _client = new HttpClient();

        public LoginViewModel()
        {
            LoginIn = new ComCommand(async p => {
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>(this.UserId, this.Password);
                
            });
        }

        #region //binding属性
        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                GetUserMessage(this.UserId);
                this.RaisePropertyChanged("UserId");
            }
        }
        private async void GetUserMessage(string str)
        {
            //数据库搜索UserId，自动补全
            string result = await _client.GetStringAsync("http://***");
            if ("" != result)
            {
                Dictionary<string, string> pair = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                this.UserId = pair.Keys.ToString();
                this.Password = pair.Values.ToString();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; this.RaisePropertyChanged("Password"); }
        }
        #endregion


        #region //命令
        public ICommand LoginIn { get; }

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
        #endregion


    }
}
