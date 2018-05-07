using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
//using SPA_DataStorage;
//using SPA_DataStorage.Database;
using SPA_Datahandler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SPA_Datahandler.BackendDatabaseHandler;
using SPA_Datahandler.Datamodel;

namespace ServiceOnline_ServiceProviderApp.ViewModel
{
    /// <summary>
    /// This class contains 
    /// ies that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand BtnSyncWithBackend { get; set; }
        public ObservableCollection<OrderSummary> OrderSummaries { get; set; }
        public string CountryName { get; set; }
        public string CountryIso2 { get; set; }
        public string CountryIso3 { get; set; }

        private Dataprovider Dataprovider { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {


            BtnSyncWithBackend = new RelayCommand(()=> startSync());
            MyClassInitialize();
            OrderSummary os = new OrderSummary();
            OrderSummaries = new ObservableCollection<OrderSummary>();

            foreach (var item in os.GetAllOrderSummaries())
            {
                OrderSummaries.Add(item);
            }
            

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