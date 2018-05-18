using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using SPA_Datahandler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPA_Datahandler.Datamodel;

namespace PL_ServiceOnline.ViewModel
{
    public class DetailVm : ViewModelBase
    {
        private IMessenger msg = Messenger.Default;

        private OrderSummary selectedJob;

        //Im Moment wird die Order Summary, die ausgewählt wurde, übergeben, danach wird diese nochmal in der DB abgefragt, damit auch Bilder etc. abgefragt werden können. Das fehlt! Es gibt im Moment nur den selben Inhalt mit selber id zurück.
        public OrderSummary SelectedJob
        {
            get { return selectedJob; }
            set
            {
                selectedJob = value;
                
            }
        }

        public DetailedClass SelectedDetailed { get; set; }

        public RelayCommand BtnSyncWithBackend { get; set; }
        public ObservableCollection<OrderSummary> UpcomingOrders { get; set; }



        public long OrderItemId { get; set; }
        
        public string Customername { get; set; }
        
        public string Servicedescription { get; set; }
        
        public DateTime PreferedDate { get; set; }
        
        public int BookedItems { get; set; }
        
        public string Address { get; set; }
        
        public string Zip { get; set; }
        
        public string City { get; set; }
        
        public string Phone { get; set; }
        
        public string IsAllInclusive { get; set; }
        
        public string IsFinished { get; set; }



        private DetailedClass OS { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public DetailVm()
        {
            SelectedJob = new OrderSummary();

            msg.Register<GenericMessage<OrderSummary>>(this, ChangeSelected);

            BtnSyncWithBackend = new RelayCommand(() => startSync());
            MyClassInitialize();

            OS = new DetailedClass();

            

        }

        private void ChangeSelected(GenericMessage<OrderSummary> obj)
        {
            SelectedJob = obj.Content;
            //Todo, abfrage macht error
            if(SelectedJob != null)
            {
                SelectedDetailed = OS.GetDetailView(SelectedJob);
                OrderItemId = SelectedJob.OrderItemId;
                Customername = SelectedDetailed.Lastname;
                Servicedescription = SelectedDetailed.Servicedescription;
                PreferedDate = SelectedDetailed.PreferedDate;
                BookedItems = SelectedDetailed.BookedItems;
                Address = SelectedDetailed.Address;
                Zip = SelectedDetailed.Zip;
                City = SelectedDetailed.City;
                Phone = SelectedDetailed.Phone;
                IsAllInclusive = SelectedDetailed.IsAllInclusive;
                IsFinished = SelectedDetailed.IsFinished;
            }

        }

        public void startSync()
        {
            SyncFromBackend SFB = new SyncFromBackend();
            MessageBox.Show(SFB.StartSync().ToString());
        }

        public static void MyClassInitialize()      //Muss gemacht werden, weil in dem Projekt, welches die Datenbankabfrage anstößt, der Connectionstring im App.config hinterlegt werden muss. Durch das "Umleiten" des datadir kann auf die Datenbank im Schwesternprojekt zugegriffen werden. 
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            int index = baseDir.IndexOf("ServiceOnline_ServiceProviderApp");
            string dataDir = baseDir.Substring(0, index) + @"ServiceOnline_ServiceProviderApp";
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

        }
    }
}