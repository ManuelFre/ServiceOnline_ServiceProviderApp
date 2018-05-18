using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using SPA_Datahandler;
using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL_ServiceOnline.ViewModel
{
    public class JobsVm : ViewModelBase
    {
        private IMessenger msg = Messenger.Default;

        private OrderSummary selectedJob;

        public OrderSummary SelectedJob
        {
            get { return selectedJob; }
            set
            {
                selectedJob = value;
                msg.Send<GenericMessage<OrderSummary>>(new GenericMessage<OrderSummary>(SelectedJob));
            }
        }
        public RelayCommand BtnSyncWithBackend { get; set; }
        public ObservableCollection<OrderSummary> Orders { get; set; }
        public string CountryName { get; set; }
        public string CountryIso2 { get; set; }
        public string CountryIso3 { get; set; }

        private OrderSummary OS { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public JobsVm()
        {


            BtnSyncWithBackend = new RelayCommand(() => startSync());
            MyClassInitialize();

            OS = new OrderSummary();

            Orders = new ObservableCollection<OrderSummary>(OS.GetPastOrderSummaries());

            msg.Register<GenericMessage<string>>(this, ChangeOrder);

            //Countries = new ObservableCollection<country>();

            //Dataprovider = new Dataprovider();
            //startSync();

            //SimpleDatabaseFunctions<service_provider> sp = new SimpleDatabaseFunctions<service_provider>();
            //var tst = sp.QueryAll();

            //SimpleDatabaseFunctions<country> cntry = new SimpleDatabaseFunctions<country>();
            //foreach (var item in cntry.QueryAll())
            //{
            //    Countries.Add(item);
            //}
        }

        private void ChangeOrder(GenericMessage<string> obj)
        {
            string last = "";


            if(last != "past" && obj.Content == "past")
            {
                Orders = new ObservableCollection<OrderSummary>(OS.GetPastOrderSummaries());
                RaisePropertyChanged("Orders");
            }
            else if(last != "future" && obj.Content == "future")
            {
                Orders = new ObservableCollection<OrderSummary>(OS.GetUpcomingOrderSummaries());
                RaisePropertyChanged("Orders");
            }

            last = obj.Content;
            
        }

        public void startSync()
        {
            SyncFromBackend SFB = new SyncFromBackend();
            MessageBox.Show(SFB.StartSync().ToString());

        }

        //public void InsertNewCountry()
        //{
        //    Countries.Clear();
        //    foreach (var item in Dataprovider.InsertAndShowCountry(CountryName, CountryIso2, CountryIso3))
        //    {
        //        Countries.Add(item);
        //    }
        //}


        public static void MyClassInitialize()      //Muss gemacht werden, weil in dem Projekt, welches die Datenbankabfrage anstößt, der Connectionstring im App.config hinterlegt werden muss. Durch das "Umleiten" des datadir kann auf die Datenbank im Schwesternprojekt zugegriffen werden. 
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            int index = baseDir.IndexOf("ServiceOnline_ServiceProviderApp");
            string dataDir = baseDir.Substring(0, index) + @"ServiceOnline_ServiceProviderApp";
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

        }
    }
}