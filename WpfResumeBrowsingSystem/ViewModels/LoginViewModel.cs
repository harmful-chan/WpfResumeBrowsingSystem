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
        private  HttpClient _client = new HttpClient();
        private struct UserInfoResult
        {
            public string UserName;
            public string UserPassword;
        }


        public LoginViewModel()
        {
            LoginIn = new ComCommand(async p => {

                string resultStr = this._client.GetStringAsync("http://47.94.162.230:80/api/Data/1?tbname=UserInfo").Result;
                UserInfoResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoResult>(resultStr);
                if (this.UserId.Equals(result.UserName) && this.Password.Equals(result.UserPassword));
            });
        }

        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                this.RaisePropertyChanged("UserId");
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; this.RaisePropertyChanged("Password"); }
        }

        public ICommand LoginIn { get; }



    }
}
