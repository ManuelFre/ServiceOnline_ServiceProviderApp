using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using PL_ServiceOnline.Converter;
using SPA_Datahandler;
using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

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

        public Dataprovider Dp { get; set; }


        public RelayCommand BtnSyncWithBackend { get; set; }
        public ObservableCollection<OrderSummary> UpcomingOrders { get; set; }



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

            Dp = new Dataprovider();

            BtnApplyChanges = new RelayCommand(() => ApplyChanges());

            CreateDemoData();

        }

        private void CreateDemoData()
        {
            CustomerId = 123;
            Firstname = "TestVorname";
            Lastname = "TestNachname";
            Address = "Testadresse 4";
            Zip = "Zip1234";
            City = "TestStadt";
            Phone = "12345";
            Email = "test@test.test";
            OrderItemId = 55;
            OrderId = 100000000;
            PreferedDate = new DateTime(2018, 1, 1);
            Servicedescription = "Beschreibung des Services, sehr guter service. Sehr toll!!!!!";
            BookedItems = 444;
            IsAllInclusive = "Y";
            Finalprice = 76.43;
            OrderedDateTime = new DateTime(2018, 1, 1);
            CustomerNotice = "Sehr gute lange notitz\r\n funktioniert multiline?\n\nnoch mehr text";
            IsFinished = "N";
            IsConfirmed = "Y";
            AddittionalCost = 84.44;
            ServiceProviderComment = "ein kommentar des service providers\ngeht hier multiline? \n interessante frage";
            OrderItemReports = new List<OrderItemReport>()
            {
                new OrderItemReport()
                {
                    Comment = "Kommentar kksksksksk",
                    Id = 15,
                    OrderItemId = 94,
                    ReportDate = new DateTime(2018, 1, 1),
                    Appendix= new List<OrderItemReportAppendix>()
                    {
                        new OrderItemReportAppendix()
                        {
                            Id =939,
                            OrderItemReportId = 99,
                            Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture.jpg",UriKind.Relative)))
                        },
                        new OrderItemReportAppendix()
                        {
                            Id =112,
                            OrderItemReportId = 12,
                            Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture2.jpg",UriKind.Relative)))
                        },
                        new OrderItemReportAppendix()
                        {
                            Id =934,
                            OrderItemReportId = 59,
                            Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture.jpg",UriKind.Relative)))
                        }
                    }

                },
                new OrderItemReport()
                {
                    Comment = "2. Kommentar kkasksk",
                    Id = 16,
                    OrderItemId = 345,
                    ReportDate = new DateTime(2018, 2, 1),
                    Appendix= new List<OrderItemReportAppendix>()
                    {
                        new OrderItemReportAppendix()
                        {
                            Id =111,
                            OrderItemReportId = 22,
                            Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture2.jpg",UriKind.Relative)))
                        }
                    }

                }
            };

        }


        private void ApplyChanges()
        {
            //TODO: update db and test if it works
            OS.AddittionalCost = AddittionalCost;

            if (Dp.UpdateOrderItemData(SelectedDetailed))
                MessageBox.Show("Update erfolgreich!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Update fehlgeschlagen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ChangeSelected(GenericMessage<OrderSummary> obj)
        {
            //TODO: proper view for OrderItemReport


            SelectedJob = obj.Content;


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