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
        public RelayCommand<object> BtnLogin { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginVm()
        {
            BtnLogin = new RelayCommand<object>((o) =>
            {
                if(o != null)
                {
                    var x = o as System.Windows.Controls.PasswordBox;
                    Password = x.Password;

                    //Hier sollte die Funktion stehen, die die Anmeldedaten dem Synctool übergibt.

                    

                }
            },
            o => { return Username != null; });

        }
    }
}
