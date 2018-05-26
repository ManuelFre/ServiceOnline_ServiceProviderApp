using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using SPA_Datahandler;
using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PL_ServiceOnline.ViewModel
{
    public class JobsVm : ViewModelBase
    {
        private string last = ""; //Wird benutzt, um zu überprüfen ob VM neu reingeladen werden muss - dient also dazu, dass ein ausgewähltes Element so bleibt.

        private IMessenger msg = Messenger.Default;

        private OrderSummary selectedJob;


        public Dictionary<string,bool> OrderDirection { get; set; }
        public RelayCommand<string> ClmOrder { get; set; }
        public OrderSummary SelectedJob
        {
            get { return selectedJob; }
            set
            {
                selectedJob = value;
                //msg.Send<GenericMessage<OrderSummary>>(new GenericMessage<OrderSummary>(SelectedJob));
            }
        }
        public RelayCommand BtnSyncWithBackend { get; set; }
        public RelayCommand BtnDetailView { get; set; }
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
            OrderDirection = new Dictionary<string, bool>()
            {
                { "OrderItemId", false },
                { "Customername", false },
                { "Servicedescription", false },
                { "IsAllInclusive", false },
                { "PreferedDate", false },
                { "BookedItems", false },
                { "Address", false },
                { "Zip", false },
                { "City", false },
                { "Phone", false },
                { "Status", false }
            };


            BtnSyncWithBackend = new RelayCommand(() => StartSync());
            MyClassInitialize();

            OS = new OrderSummary();

            Orders = new ObservableCollection<OrderSummary>(OS.GetPastOrderSummaries());

            msg.Register<GenericMessage<string>>(this, ChangeOrder);

            BtnDetailView = new RelayCommand(execute: () => {
                msg.Send<GenericMessage<OrderSummary>>(new GenericMessage<OrderSummary>(SelectedJob));
            }, canExecute: () =>
            {
                return (SelectedJob != null);
            });

            // Befehl bei Klick auf eine Column
            ClmOrder = new RelayCommand<string>(OrderColumns());


        

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



        private Action<string> OrderColumns()
        {
            return (para) =>
            {
                IEnumerable<OrderSummary> query;

                if(para != "Status")
                {
                    if (!OrderDirection[para])
                    {
                        query = (from x in Orders
                                     orderby x.GetType().GetProperty(para).GetValue(x, null) ascending
                                     select x);
                    }
                    else
                    {
                        query = (from x in Orders
                                     orderby x.GetType().GetProperty(para).GetValue(x, null) descending
                                     select x);
                    }
                }
                else
                {
                    if (!OrderDirection[para])
                    {
                        query = (from x in Orders
                                     orderby x.IsFinished ascending, x.IsConfirmed ascending
                                     select x);
                    }
                    else
                    {
                        query = (from x in Orders
                                     orderby x.IsFinished descending, x.IsConfirmed descending
                                     select x);
                    }


                }
                Orders = new ObservableCollection<OrderSummary>(query);
                RaisePropertyChanged("Orders");
                OrderDirection[para] = !OrderDirection[para];

            };
        }

        private void ChangeOrder(GenericMessage<string> obj)
        {

            if (last != "past" && obj.Content == "past")
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

        public void StartSync()
        {
            //SyncFromBackend SFB = new SyncFromBackend();
            //MessageBox.Show(SFB.StartSync().ToString());
            MessageBox.Show("Aus Sicherheitsgründen deaktiviert von Freischlager");

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