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
        public RelayCommand BtnApplyChanges { get; set; }

        public Dataprovider dp { get; set; }


        public RelayCommand BtnSyncWithBackend { get; set; }
        public ObservableCollection<OrderSummary> UpcomingOrders { get; set; }


        /*
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
        */


        public int CustomerId { get; set; }
        
        public string Firstname { get; set; }
        
        public string Lastname { get; set; }
        
        public string Address { get; set; }
        
        public string Zip { get; set; }
        
        public string City { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public int OrderItemId { get; set; }
        
        public long OrderId { get; set; }
        
        public DateTime PreferedDate { get; set; }
        
        public string Servicedescription { get; set; }
        
        public int BookedItems { get; set; }
        
        public string IsAllInclusive { get; set; }
        
        public double Finalprice { get; set; }
        
        public DateTime OrderedDateTime { get; set; }
        
        public string CustomerNotice { get; set; }
        
        public string IsFinished { get; set; }
        
        public string IsConfirmed { get; set; }
        
        public double? AddittionalCost { get; set; }
        
        public string ServiceProviderComment { get; set; }
        
        public List<OrderItemReport> OrderItemReports { get; set; }



        private DetailedClass OS { get; set; }




        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public DetailVm()
        {
            SelectedJob = new OrderSummary();

            msg.Register<GenericMessage<OrderSummary>>(this, ChangeSelected);

            BtnSyncWithBackend = new RelayCommand(() => StartSync());
            MyClassInitialize();

            OS = new DetailedClass();

            dp = new Dataprovider();

            BtnApplyChanges = new RelayCommand(() => ApplyChanges());

        }

        private void ApplyChanges()
        {
            //TODO: update db
            OS.AddittionalCost = AddittionalCost;

            dp.UpdateOrderItemData(OS);
        }

        private void ChangeSelected(GenericMessage<OrderSummary> obj)
        {
            SelectedJob = obj.Content;
            //Todo: folgendes soll Änderbar (DB-Query gibts dafür bereits!) sein:

            //AddittionalCost;
            //Finalprice;
            //IsAllInclusive;
            //sConfirmed;
            //IsFinished;
            //PreferedDate;
            //ServiceProviderComment;




            if (SelectedJob != null)
            {
                SelectedDetailed = OS.GetDetailView(SelectedJob);

                CustomerId = SelectedDetailed.CustomerId;
                Firstname = SelectedDetailed.Firstname;
                Lastname = SelectedDetailed.Lastname;
                Address = SelectedDetailed.Address;
                Zip = SelectedDetailed.Zip;
                City = SelectedDetailed.City;
                Phone = SelectedDetailed.Phone;
                Email = SelectedDetailed.Email;
                OrderItemId = SelectedDetailed.OrderItemId;
                OrderId = SelectedDetailed.OrderId;
                PreferedDate = SelectedDetailed.PreferedDate;
                Servicedescription = SelectedDetailed.Servicedescription;
                BookedItems = SelectedDetailed.BookedItems;
                IsAllInclusive = SelectedDetailed.IsAllInclusive;
                Finalprice = SelectedDetailed.Finalprice;
                OrderedDateTime = SelectedDetailed.OrderedDateTime;
                CustomerNotice = SelectedDetailed.CustomerNotice;
                IsFinished = SelectedDetailed.IsFinished;
                IsConfirmed = SelectedDetailed.IsConfirmed;
                AddittionalCost = SelectedDetailed.AddittionalCost;
                ServiceProviderComment = SelectedDetailed.ServiceProviderComment;
                OrderItemReports = SelectedDetailed.OrderItemReports;
                
                //OrderItemId = SelectedJob.OrderItemId;
                //Customername = SelectedDetailed.Lastname;
                //Servicedescription = SelectedDetailed.Servicedescription;
                //PreferedDate = SelectedDetailed.PreferedDate;
                //BookedItems = SelectedDetailed.BookedItems;
                //Address = SelectedDetailed.Address;
                //Zip = SelectedDetailed.Zip;
                //City = SelectedDetailed.City;
                //Phone = SelectedDetailed.Phone;
                //IsAllInclusive = SelectedDetailed.IsAllInclusive;
                //IsFinished = SelectedDetailed.IsFinished;
            }

        }

        public void StartSync()
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