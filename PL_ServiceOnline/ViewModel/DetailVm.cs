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
    public class DetailVm : ViewModelBase
    {
        private IMessenger msg = Messenger.Default;

        //Im Moment wird die Order Summary, die ausgewählt wurde, übergeben, danach wird diese nochmal in der DB abgefragt, damit auch Bilder etc. abgefragt werden können. Das fehlt! Es gibt im Moment nur den selben Inhalt mit selber id zurück.
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
            set { preferedDate = value; }//Inn der Uhrzeit ist ein Falsches Datum und in dem Datum eine falsche Uhrzeit abgespeichert LOL NEIN DB FEHLER FML
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

        //private string time;

        //public string Time
        //{
        //    get { return time; }
        //    set
        //    {
        //        int.TryParse(value.Split(':')[0], out int tmp);
        //        Hour = tmp;
        //        int.TryParse(value.Split(':')[1], out tmp);
        //        Minute = tmp;
        //        time = value;
        //    }
        //}


        public int Hour { get; set; }
        public int Minute { get; set; }


        //public string StringPreferedDate { get; set; }
        //public string StringOrderedDateTime { get; set; }


        private string serviceProviderComment;

        public string ServiceProviderComment
        {
            get { return serviceProviderComment; }
            set { serviceProviderComment = value; RaisePropertyChanged(); }
        }

        public string ServiceUnit { get; set; }

        //TODO: IsFinished und IsConfirmed zusammenfassen und als Combobox ("Status:") anzeigen "nicht angenommen"(default wenn null, N, Nein), "abgelehnt" (msgBox=>confirm das der auftrag wirklich abgelehnt werden will), "angenommen" und "erledigt" 
        //Auftrag löschen
        //Gibt der Dienstleister im Schritt „Auftragsdetails bearbeiten“ bekannt, dass er den Auftrag ablehnen möchte, 
        //so wird um erneute Bestätigung gebeten und der Auftrag als "abgelehnt" in der Datenbank persistiert.

        /// <summary>
        /// "nicht angenommen"(default wenn null, N, Nein), "abgelehnt" (msgBox=>confirm das der auftrag wirklich abgelehnt werden will), "angenommen" und "erledigt" 
        /// </summary>

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged(); }
        }


        //public string Status { get; set; }

        public string[] AllStatuses { get; set; }


        public ObservableCollection<OrderItemReport_> OrderItemReports { get; set; }



        private DetailedClass OS { get; set; }



        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public DetailVm()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");

            msg.Register<GenericMessage<OrderSummary>>(this, ChangeSelected);

            AllStatuses = new string[] { "Abgeschlossen", "Angenommen", "Nicht bestätigt", "Auftrag ablehnen" };

            SelectedJob = new OrderSummary();
            OS = new DetailedClass();

            Dp = new Dataprovider();


            BtnSyncWithBackend = new RelayCommand(() => StartSync());



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
                                //MessageBox.Show($"Id= {p.Id},  OrderItemId= { p.OrderItemId}"); //just to check if the correct parameters actually arrive
                                //SelectedDetailed.OrderItemReports.Add(new OrderItemReportAppendix()
                                //item.Appendix.Add(new OrderItemReportAppendix()
                                SelectedDetailed.OrderItemReports[i].Appendix.Add(new OrderItemReportAppendix()                                
                                {
                                    Id = item.OrderItemId,
                                    Picture = ImageConverter.ImageToByteArray(new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute)))

                                });
                                //MessageBox.Show(SelectedDetailed.OrderItemReports[i].Appendix.ToString() + "     " + i);
                                
                                item.Visibility = "Visible"; //TODO: Why does this not work?
                                RaisePropertyChanged(nameof(SelectedDetailed));

                                //Dp.UpdateOrderItemReportAppendix(SelectedDetailed); //TODO: In that methode update all OrderItemReports or just this one...
                            }
                            RaisePropertyChanged(nameof(item.Visibility));
                            i++;
                        }
                    }

                });

            //CreateDemoData();


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
                    //if(Dp.AddOrderItemReport(oir))
                    //    MessageBox.Show("Update erfolgreich!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    //else
                    //    MessageBox.Show("Update fehlgeschlagen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);


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
            CreateReportDialog reportDialog = new CreateReportDialog(CurrentMaxOrderItemReportId+1);
            if (reportDialog.ShowDialog() == true)
            {
                OrderItemReports.Add(reportDialog.Answer);
                RaisePropertyChanged(nameof(SelectedDetailed.OrderItemReports));
                //TODO:Since hard reset after merge fail,... new reports are not shown anymore,.. why?
          

                //MessageBox.Show(reportDialog.Answer.ToString());
            }
            //TODO: Actually remove the whole stuff b4 release if not needed [Create PDF here (PDFsharp NuGet package schon eingebaut)]


            // Create a temporary file
            //string filename = String.Format("{0}_tempfile.pdf", Guid.NewGuid().ToString("D").ToUpper());
            //var s_document = new PdfDocument();
            //s_document.Info.Title = "PDFsharp XGraphic Sample";
            //s_document.Info.Author = "Stefan Lange";
            //s_document.Info.Subject = "Created with code snippets that show the use of graphical functions";
            //s_document.Info.Keywords = "PDFsharp, XGraphics";

            //// Create demonstration pages
            //new LinesAndCurves().DrawPage(s_document.AddPage());
            //new Shapes().DrawPage(s_document.AddPage());
            //new Paths().DrawPage(s_document.AddPage());
            //new Text().DrawPage(s_document.AddPage());
            //new Images().DrawPage(s_document.AddPage());

            // Save the s_document...
            //s_document.Save(filename);



            // ...and start a viewer
            //Process.Start(filename);


        }

        private void CreateDemoData()
        {
            Hour = 14;
            Minute = 12;
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
            PreferedDate = new DateTime(2018, 12, 31, Hour, Minute, 0);
            Servicedescription = "Beschreibung des Services, sehr guter service. Sehr toll!!!!!";
            BookedItems = 444;
            IsAllInclusive = "Y";
            Finalprice = 76.43;
            OrderedDateTime = new DateTime(2018, 12, 31, Hour, Minute, 0);
            CustomerNotice = "Sehr gute lange notitz\r\n funktioniert multiline?\n\nnoch mehr text";
            IsFinished = "N";
            IsConfirmed = "Y";
            AddittionalCost = 84.44;
            ServiceProviderComment = "ein kommentar des service providers\ngeht hier multiline? \n interessante frage";
            ServiceUnit = "Arbeitsstunde";
            OrderItemReports = new ObservableCollection<OrderItemReport_>()
            {
                new OrderItemReport_()
                {
                    Comment = "Kommentar kksksksksk",
                    Id = 15,
                    OrderItemId = 94,
                    ReportDate = new DateTime(2018, 1, 1,Hour,Minute,0),
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
                new OrderItemReport_()
                {
                    Comment = "2. Kommentar kkasksk",
                    Id = 16,
                    OrderItemId = 345,
                    ReportDate = new DateTime(2018, 2, 1,Hour,Minute,0),
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
            if (Status.Equals(AllStatuses[3]))
            {
                if (MessageBox.Show("Wollen Sie den Auftrag wirklich ablehnen?", "Auftrag ablehnen?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
            }
            //OS.AddittionalCost = AddittionalCost;



            SelectedDetailed.AddittionalCost = AddittionalCost;
            SelectedDetailed.Finalprice = Finalprice;
            SelectedDetailed.IsAllInclusive = IsAllInclusive;
            //SelectedDetailed.IsConfirmed = IsConfirmed;
            //SelectedDetailed.IsFinished = IsFinished;
            SelectedDetailed.IsConfirmed = GetConfirmStatus(Status);
            SelectedDetailed.IsFinished = GetFinishedStatus(Status);

            SelectedDetailed.PreferedDate = PreferedDate;
            SelectedDetailed.ServiceProviderComment = ServiceProviderComment;


            if(IsConfirmed != null && IsConfirmed != "x" && PreferedDate < DateTime.Now)
                msg.Send<GenericMessage<string>>(new GenericMessage<string>("past"));
            else if (IsConfirmed != null && IsConfirmed != "x" && PreferedDate > DateTime.Now)
                msg.Send<GenericMessage<string>>(new GenericMessage<string>("future"));
            else
                msg.Send<GenericMessage<string>>(new GenericMessage<string>("denied"));


            if (Dp.UpdateOrderItemData(SelectedDetailed))
            {
                MessageBox.Show("Update erfolgreich!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                RaisePropertyChanged(nameof(PreferedDate)); //so that the PreferedDate in the DetailView gets actually updated once it's sent to the DB
            }
            else
                MessageBox.Show("Update fehlgeschlagen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private string GetStatus(string isFinished, string isConfirmed)
        {
            if (isConfirmed != null)
            {
                if (isConfirmed.Equals("x") || isConfirmed.Equals("X"))
                {
                    return AllStatuses[3];
                }

                if (IsFinished != null)
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
        private string GetFinishedStatus(string status)
        {
            //wenn status in checkbox == Abgeschlossen [0]
            if (status != null)
            {
                if (status.Equals(AllStatuses[0]))
                    return "Y";
            }
            return "N";
        }

        private string GetConfirmStatus(string status)
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
            //TODO: proper view for OrderItemReport


            SelectedJob = obj.Content;


            //AddittionalCost;
            //Finalprice;
            //IsAllInclusive;
            //sConfirmed;
            //IsFinished;
            //PreferedDate;
            //ServiceProviderComment;

            //.Contains(",") ? AddittionalCost.Replace(',', '.') : AddittionalCost  .Replace('.', ',')


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
                ServiceUnit = SelectedDetailed.ServiceUnit;
                OrderItemReports = new ObservableCollection<OrderItemReport_>(SelectedDetailed.OrderItemReports as List<OrderItemReport_>);
                Status = GetStatus(IsFinished, IsConfirmed);
                //StringOrderedDateTime = OrderedDateTime.ToString("dd.MM.yyyy - hh.mm");
                //StringPreferedDate = PreferedDate.ToString("dd.MM.yyyy - hh.mm");
            }

        }



        public void StartSync()
        {
            SyncFromBackend SFB = new SyncFromBackend();
            MessageBox.Show(SFB.StartSync().ToString());
        }

    }
}