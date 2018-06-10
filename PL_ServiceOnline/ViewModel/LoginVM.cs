using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using SPA_Datahandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL_ServiceOnline.ViewModel
{
    public class LoginVm : ViewModelBase
    {
        private IMessenger msg = Messenger.Default;
        public RelayCommand<object> BtnLogin { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        private Dataprovider Dp;
        public LoginVm()
        {
            BtnLogin = new RelayCommand<object>((o) =>
            {
                if(o != null)
                {
                    var x = o as System.Windows.Controls.PasswordBox;
                    Password = x.Password;


                    //Hier steht die Funktion, die die Anmeldedaten dem Synctool übergibt.
                    Dp = new Dataprovider();

                    if(Dp.LogIn(Username, Password))
                    {
                        MessageBox.Show("Herzlich Willkommen " + Username);
                        msg.Send<GenericMessage<string>>(new GenericMessage<string>(Username), "userToken");
                        msg.Send<GenericMessage<string>>(new GenericMessage<string>("update"));
                        msg.Send<GenericMessage<bool>>(new GenericMessage<bool>(true));
                    }
                    else
                    {
                        MessageBox.Show("Kein Benutzer mit diesem Passwort bekannt");
                    }
                    

                    

                }
            },
            o => { return Username != null; });

        }
    }
}
