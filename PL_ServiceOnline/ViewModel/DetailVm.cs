using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using PL_ServiceOnline.Converter;
using SPA_Datahandler;
using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PL_ServiceOnline.ViewModel
{
    //TODO: maybe add horizontal scrollbar into the reportitmes itemcontrol,... (already tried but issues with stackpannel approach)
    public class DetailVm : ViewModelBase
    {
        #region Properties
        private IMessenger msg = Messenger.Default;
        public OrderSummary SelectedJob { get; set; }
        public DetailedClass SelectedDetailed { get; set; }
        public RelayCommand BtnApplyChanges { get; set; }
        public RelayCommand BtnCreateReport { get; set; }
        public RelayCommand<OrderItemReport_> BtnAddPicture { get; set; }
        public Dataprovider Dp { get; set; }


        public RelayCommand BtnSyncWithBackend { get; set; }
        public ObservableCollection<OrderSummary> UpcomingOrders { get; set; }

        public RelayCommand BtnAppendDocuments { get; set; }



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

        private DateTime preferedDate;
        public DateTime PreferedDate
        {
            get { return preferedDate; }
            set { preferedDate = value; RaisePropertyChanged(); }
        }


        private string preferedDateTime;
        public string PreferedDateTime
        {
            get { return preferedDateTime; }
            set { preferedDateTime = value; RaisePropertyChanged(); }
        }

        public string Servicedescription { get; set; }

        public int BookedItems { get; set; }

        public string IsAllInclusive { get; set; }

        public double Finalprice { get; set; }

        public DateTime OrderedDateTime { get; set; }

        public string CustomerNotice { get; set; }

        public string IsFinished { get; set; }

        public string IsConfirmed { get; set; }

        public double? AddittionalCost { get; set; }



        public int Hour { get; set; }
        public int Minute { get; set; }

        private string serviceProviderComment;

        public string ServiceProviderComment
        {
            get { return serviceProviderComment; }
            set { serviceProviderComment = value; RaisePropertyChanged(); }
        }
        public string ServiceUnit { get; set; }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged(); }
        }

        public string[] AllStatuses { get; set; }

        public ObservableCollection<OrderItemReport_> OrderItemReports { get; set; }

        private DetailedClass OS { get; set; }

        private List<String> dropdownTimes;

        public List<String> DropdownTimes
        {
            get { return dropdownTimes; }
            set { dropdownTimes = value; RaisePropertyChanged(); }
        }


        #endregion


        public DetailVm()
        {

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");

            msg.Register<GenericMessage<OrderSummary>>(this, ChangeSelected);

            AllStatuses = new string[] { "Abgeschlossen", "Angenommen", "Nicht bestätigt", "Auftrag ablehnen" };

            SelectedJob = new OrderSummary();
            OS = new DetailedClass();

            Dp = new Dataprovider();


            BtnApplyChanges = new RelayCommand(() => ApplyChanges());
            BtnCreateReport = new RelayCommand(() => CreateReport());
            BtnAppendDocuments = new RelayCommand(() => AppendDocuments());

            BtnAddPicture = new RelayCommand<OrderItemReport_>(
                (p) =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Title = "Select a picture",
                        Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
                    };
                    if ((bool)openFileDialog.ShowDialog())
                    {
                        int i = 0;
                        foreach (var item in SelectedDetailed.OrderItemReports)
                        {
                            item.Visibility = "Collapsed";

                            if (p == item)
                            {
                                SelectedDetailed.OrderItemReports[i].Appendix.Add(new OrderItemReportAppendix()
                                {
                                    OrderItemReportId = item.Id ,
                                    Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute)))

                                });

                                item.Visibility = "Visible"; //TODO: Does it work?
                                RaisePropertyChanged(nameof(SelectedDetailed));

                            }
                            RaisePropertyChanged(nameof(item.Visibility));
                            i++;
                        }
                    }

                });
            CreateDateTime("4:45");
            //CreateDemoData();
        }


        private void CreateDateTime(string Nullvalue)
        {
            List<String> tmp = new List<string>();
            tmp.Add(Nullvalue);
            for (int i = 5; i < 22; i++)
            {
                tmp.Add(i + ":00");
                tmp.Add(i + ":15");
                tmp.Add(i + ":30");
                tmp.Add(i + ":45");
            }
            DropdownTimes = tmp;
        }

        private void AppendDocuments()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };
            ;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    OrderItemReport_ oir = new OrderItemReport_()
                    {
                        OrderItemId = SelectedDetailed.OrderItemId,
                        ReportDate = new DateTime(),
                        Appendix = new List<OrderItemReportAppendix>()
                            {
                                new OrderItemReportAppendix()
                                {
                                    Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute)))
                                }
                            }
                    };
                    SelectedDetailed.OrderItemReports.Add(oir);
                    Dp.AddOrderItemReport(oir);

                    if (Dp.UpdateOrderItemData(SelectedDetailed))
                        MessageBox.Show("Update erfolgreich!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Update fehlgeschlagen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Laden des Bildes fehlgeschlagen!\n" + e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateReport()
        {
            int CurrentMaxOrderItemReportId = 0;
            foreach (var item in SelectedDetailed.OrderItemReports)
            {
                if (item.OrderItemId > CurrentMaxOrderItemReportId)
                    CurrentMaxOrderItemReportId = item.OrderItemId;
            }
            CreateReportDialog reportDialog = new CreateReportDialog(CurrentMaxOrderItemReportId + 1, OrderItemId);
            if (reportDialog.ShowDialog() == true)
            {
                OrderItemReport_ CreatedReport = reportDialog.Answer;
                OrderItemReports.Add(CreatedReport);
                Dp.AddOrderItemReport(CreatedReport);

                RaisePropertyChanged(nameof(SelectedDetailed.OrderItemReports));
            }
        }

        private void ApplyChanges()
        {
            if (Status.Equals(AllStatuses[3]))
            {
                if (MessageBox.Show("Wollen Sie den Auftrag wirklich ablehnen?", "Auftrag ablehnen?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
            }

            SelectedDetailed.AddittionalCost = AddittionalCost;
            SelectedDetailed.Finalprice = Finalprice;
            SelectedDetailed.IsAllInclusive = IsAllInclusive;

            SelectedDetailed.IsConfirmed = GetConfirmStatus(Status);
            SelectedDetailed.IsFinished = GetFinishedStatus(Status);

            //SelectedDetailed.PreferedDate = PreferedDate;
            SelectedDetailed.PreferedDate = SelectedDetailed.PreferedDate + TimeSpan.ParseExact(preferedDateTime, "g", CultureInfo.CurrentCulture);
            SelectedDetailed.ServiceProviderComment = ServiceProviderComment;


            if (IsConfirmed != null && IsConfirmed != "x" && SelectedDetailed.PreferedDate < DateTime.Now)
                msg.Send(new GenericMessage<string>("past"));
            else if (IsConfirmed != null && IsConfirmed != "x" && SelectedDetailed.PreferedDate > DateTime.Now)
                msg.Send(new GenericMessage<string>("future"));
            else
                msg.Send(new GenericMessage<string>("denied"));


            if (Dp.UpdateOrderItemData(SelectedDetailed))
            {
                MessageBox.Show("Update erfolgreich!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                RaisePropertyChanged(nameof(PreferedDate)); //so that the PreferedDate in the DetailView gets actually updated once it's sent to the DB
                msg.Send(new GenericMessage<string>("update"));     //Damit die Auswertung der Firmendaten mitbekommt, dass es eine Änderung gibt.
            }
            else
                MessageBox.Show("Update fehlgeschlagen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public string GetStatus(string isFinished, string isConfirmed)
        {
            if (isConfirmed != null)
            {
                if (isConfirmed.Equals("x") || isConfirmed.Equals("X"))
                {
                    return AllStatuses[3];
                }

                if (isFinished != null) //OJE Hier durchs unittest möglicherweise einen type erkannt--> geändert von IsFinished zu isFinished
                {
                    if (isFinished.Equals("Y") || isFinished.Equals("Ja"))
                    {
                        return AllStatuses[0];//Abgeschlossen
                    }
                    else if (isConfirmed.Equals("Y") || isConfirmed.Equals("Ja"))
                    {
                        return AllStatuses[1]; // Angenommen
                    }
                }
            }
            return AllStatuses[2]; //Nicht bestätigt"
        }
        public string GetFinishedStatus(string status)
        {
            //wenn status in checkbox == Abgeschlossen [0]
            if (status != null)
            {
                if (status.Equals(AllStatuses[0]))
                    return "Y";
            }
            return "N";
        }

        public string GetConfirmStatus(string status)
        {
            //wenn status in checkbox == Abgeschlossen [0] oder Angenommen [1]  dann 'Y'
            if (status != null)
            {
                if (status.Equals(AllStatuses[3]))
                    return "x";
                if (status.Equals(AllStatuses[0]) || status.Equals(AllStatuses[1]))
                    return "Y";
            }
            return "N";
        }

        private void ChangeSelected(GenericMessage<OrderSummary> obj)
        {
            SelectedJob = obj.Content;


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
                PreferedDateTime = PreferedDate.ToString("H:mm");
                CreateDateTime(PreferedDateTime);                          
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
                ServiceUnit = SelectedDetailed.ServiceUnit;
                OrderItemReports = new ObservableCollection<OrderItemReport_>(SelectedDetailed.OrderItemReports as List<OrderItemReport_>);
                Status = GetStatus(IsFinished, IsConfirmed);
            }

        }


        //private void CreateDemoData()
        //{
        //    Hour = 14;
        //    Minute = 12;
        //    CustomerId = 123;
        //    Firstname = "TestVorname";
        //    Lastname = "TestNachname";
        //    Address = "Testadresse 4";
        //    Zip = "Zip1234";
        //    City = "TestStadt";
        //    Phone = "12345";
        //    Email = "test@test.test";
        //    OrderItemId = 55;
        //    OrderId = 100000000;
        //    PreferedDate = new DateTime(2018, 12, 31, Hour, Minute, 0);
        //    Servicedescription = "Beschreibung des Services, sehr guter service. Sehr toll!!!!!";
        //    BookedItems = 444;
        //    IsAllInclusive = "Y";
        //    Finalprice = 76.43;
        //    OrderedDateTime = new DateTime(2018, 12, 31, Hour, Minute, 0);
        //    CustomerNotice = "Sehr gute lange notitz\r\n funktioniert multiline?\n\nnoch mehr text";
        //    IsFinished = "N";
        //    IsConfirmed = "Y";
        //    AddittionalCost = 84.44;
        //    ServiceProviderComment = "ein kommentar des service providers\ngeht hier multiline? \n interessante frage";
        //    ServiceUnit = "Arbeitsstunde";
        //    OrderItemReports = new ObservableCollection<OrderItemReport_>()
        //    {
        //        new OrderItemReport_()
        //        {
        //            Comment = "Kommentar kksksksksk",
        //            Id = 15,
        //            OrderItemId = 94,
        //            ReportDate = new DateTime(2018, 1, 1,Hour,Minute,0),
        //            Appendix= new List<OrderItemReportAppendix>()
        //            {
        //                new OrderItemReportAppendix()
        //                {
        //                    Id =939,
        //                    OrderItemReportId = 99,
        //                    Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture.jpg",UriKind.Relative)))
        //                },
        //                new OrderItemReportAppendix()
        //                {
        //                    Id =112,
        //                    OrderItemReportId = 12,
        //                    Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture2.jpg",UriKind.Relative)))
        //                },
        //                new OrderItemReportAppendix()
        //                {
        //                    Id =934,
        //                    OrderItemReportId = 59,
        //                    Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture.jpg",UriKind.Relative)))
        //                }
        //            }

        //        },
        //        new OrderItemReport_()
        //        {
        //            Comment = "2. Kommentar kkasksk",
        //            Id = 16,
        //            OrderItemId = 345,
        //            ReportDate = new DateTime(2018, 2, 1,Hour,Minute,0),
        //            Appendix= new List<OrderItemReportAppendix>()
        //            {
        //                new OrderItemReportAppendix()
        //                {
        //                    Id =111,
        //                    OrderItemReportId = 22,
        //                    Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(@"..\..\Images\TestPicture2.jpg",UriKind.Relative)))
        //                }
        //            }

        //        }
        //    };

        //}
    }
}