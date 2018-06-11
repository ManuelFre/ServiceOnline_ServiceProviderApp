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
using System.Windows.Threading;

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
                        MessageBox.Show("Herzlich Willkommen " + Username, "Anmeldung erfolgreich", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        UpdateSyncDate();
                        msg.Send<GenericMessage<string>>(new GenericMessage<string>(Username), "userToken");
                        msg.Send<GenericMessage<string>>(new GenericMessage<string>("update"));
                        msg.Send<GenericMessage<bool>>(new GenericMessage<bool>(true));

                        DispatcherTimer dt = new DispatcherTimer();
                        dt.Tick += new EventHandler(PartlySynchronisation);
                        dt.Interval = new TimeSpan(0, 0, 30);
                        dt.Start();

                    }
                    else
                    {
                        MessageBox.Show("Kein Benutzer mit diesem Passwort bekannt oder keine Verbindung mit Internet hergestellt.","Anmeldung fehlgeschlagen",MessageBoxButton.OK,MessageBoxImage.Warning);
                    }
                    

                    

                }
            },
            o => { return Username != null; });

        }

        private void PartlySynchronisation(object sender, EventArgs e)
        {
            if (Dp.StartSynchronisation())
            {
                Application.Current.Dispatcher.Invoke(UpdateSyncDate);
            }
        }

        private void UpdateSyncDate()
        {
            msg.Send<GenericMessage<DateTime>>(new GenericMessage<DateTime>(DateTime.Now));
        }

    }
}
