using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PL_ServiceOnline.ViewModel
{
    public class LoginVm : ViewModelBase
    {
        private IMessenger msg = Messenger.Default;
        public RelayCommand<object> LoginButton { get; set; }
        public string Username { get; set; }
        public SecureString Password { get; set; }

        public LoginVm()
        {
            LoginButton = new RelayCommand<object>((o) =>
            {
                if(o != null)
                {
                    Password = o as SecureString;
                }
            },
            o => { return Username != null; });

        }
    }
}
