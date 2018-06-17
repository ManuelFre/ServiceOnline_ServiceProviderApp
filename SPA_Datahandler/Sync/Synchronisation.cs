using SPA_Datahandler.SyncServiceReference;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler.Sync
{
    public class Synchronisation : Dataprovider
    {
        public SpaServClient SyncClient { get; set; }

        public string GetFromDate { get; set; }
        public string DateTimeNow { get; set; }



        public Synchronisation()
        {
            SyncClient = new SpaServClient("BasicHttpBinding_ISpaServ");
        }


        public SpaUser LogIn(string username, string password)
        {
            //SyncClient.Logon("franz", "Start123");   
            SpaUser NewUser =  SyncClient.Logon(username, password);            
            return NewUser;
        }

        public Boolean FullSync(int ServiceProviderId)           //Hier wird die LEERE Datenbank mit Backenddaten zum angemeldeten User versorgt.
        {
            GetFromDate = "01.01.1900 00:00:00";
            DateTimeNow = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            if (ClearAllLocalTables())
            {
                GetSowUser(ServiceProviderId);
                GetOrderHeader(ServiceProviderId);
                GetOrderDetail(ServiceProviderId);
                GetServiceProvider(ServiceProviderId);
                GetService(ServiceProviderId);
                GetOrderItem(ServiceProviderId);
                GetOrderItemReport(ServiceProviderId);
                GetOrderItemReportAppendix(ServiceProviderId);
                WriteLastSync(DateTime.Now);
            }
            return true;
        }

        public Boolean PartSync(int ServiceProviderId)
        {
            DateTime LastSync = QueryLastSync();

            GetFromDate = LastSync.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTimeNow = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            int WrittenData = SendOrderItem() + SendOrderItemReport() + SendOrderItemReportAppendix() + SendServiceProvider();


            GetSowUser(ServiceProviderId);
            GetOrderHeader(ServiceProviderId);
            GetOrderDetail(ServiceProviderId);
            GetServiceProvider(ServiceProviderId);
            GetService(ServiceProviderId);
            GetOrderItem(ServiceProviderId);
            GetOrderItemReport(ServiceProviderId);
            GetOrderItemReportAppendix(ServiceProviderId);
            WriteLastSync(DateTime.Now);

            return true;
        }

        

        public Boolean ClearAllLocalTables()
        {
            dbContext = new DbServiceProviderAppEntities();
            SimpleDatabaseFunctions<order_item_report_appendix> SDOira = new SimpleDatabaseFunctions<order_item_report_appendix>();
            SimpleDatabaseFunctions<order_item_report> SDOir = new SimpleDatabaseFunctions<order_item_report>();
            SimpleDatabaseFunctions<order_item> SDOi = new SimpleDatabaseFunctions<order_item>();
            SimpleDatabaseFunctions<service> SDS = new SimpleDatabaseFunctions<service>();
            SimpleDatabaseFunctions<service_provider> SDSp = new SimpleDatabaseFunctions<service_provider>();
            SimpleDatabaseFunctions<order_detail> SDOd = new SimpleDatabaseFunctions<order_detail>();
            SimpleDatabaseFunctions<order_header> SDOh = new SimpleDatabaseFunctions<order_header>();
            SimpleDatabaseFunctions<sow_user_delivery_address> SDSuda = new SimpleDatabaseFunctions<sow_user_delivery_address>();
            SimpleDatabaseFunctions<sow_user> SDSu = new SimpleDatabaseFunctions<sow_user>();
            SimpleDatabaseFunctions<spa_synctimes> SDSync = new SimpleDatabaseFunctions<spa_synctimes>();

            return SDOira.ClearTable() &&
            SDOir.ClearTable() &&
            SDOi.ClearTable() &&
            SDS.ClearTable() &&
            SDSp.ClearTable() &&
            SDOd.ClearTable() &&
            SDOh.ClearTable() &&
            SDSuda.ClearTable() &&
            SDSu.ClearTable() &&
            SDSync.ClearTable();
        }


        #region SendToSync
        private int SendOrderItem()
        {
            DateTime FromDate = DateTime.Parse(GetFromDate);
            List<order_item> LocalOrderItems = QueryOrderItems(FromDate);
            OrderItem[] SendOrderItems = new OrderItem[LocalOrderItems.Count()];
            for (int i = 0; i< LocalOrderItems.Count(); i++)
            {
                OrderItem tmp = new OrderItem();
                tmp.AddCost = (LocalOrderItems[i].addittional_cost == null) ? 0 : LocalOrderItems[i].addittional_cost.Value;
                tmp.Comment = LocalOrderItems[i].service_provider_comment;
                tmp.CreateDat = LocalOrderItems[i].createdAt.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                tmp.FinalPrice = LocalOrderItems[i].final_price;
                tmp.FinalPriceWithoutTax = LocalOrderItems[i].final_price_without_tax;
                tmp.FinalPriceWithTax = LocalOrderItems[i].final_price_with_tax;
                tmp.Id = LocalOrderItems[i].Id;
                tmp.IsAllIncl = LocalOrderItems[i].is_all_inclusive;
                tmp.IsConfirmed= LocalOrderItems[i].is_confirmed;
                tmp.IsFinished = LocalOrderItems[i].is_finished;
                tmp.OptionPrice = LocalOrderItems[i].option_price;
                tmp.OrderId = LocalOrderItems[i].order_id;
                tmp.PerItemTax = LocalOrderItems[i].per_item_tax;
                tmp.PreferredDatetime = LocalOrderItems[i].preferred_date_time.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                tmp.Price = LocalOrderItems[i].price;
                tmp.Quantity = LocalOrderItems[i].quantity;
                tmp.ServiceId = LocalOrderItems[i].service_id;
                tmp.Tax = LocalOrderItems[i].tax;

                SendOrderItems[i] = tmp;
            }
            int cntChangedItems = SyncClient.PutOrderItem(SendOrderItems, DateTimeNow, false);
            return cntChangedItems;
        }

        private int SendOrderItemReport()
        {
            DateTime FromDate = DateTime.Parse(GetFromDate);
            List<order_item_report> LocalOrderItemReports = QueryOrderItemReports(FromDate);
            OrderItemReport[] SendOrderItemReports = new OrderItemReport[LocalOrderItemReports.Count() ];

            for (int i = 0; i < SendOrderItemReports.Count(); i++)
            {
                OrderItemReport tmp = new OrderItemReport();
                tmp.CreateDat =  LocalOrderItemReports[i].createdat.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                tmp.Id = LocalOrderItemReports[i].Id.ToString();
                tmp.OrderItemId = LocalOrderItemReports[i].order_item_id;
                tmp.ReportComment = LocalOrderItemReports[i].comment;
                
                SendOrderItemReports[i] = tmp;
            }
            return SyncClient.PutOrderItemReport(SendOrderItemReports, DateTimeNow, false);
        }

        private int SendOrderItemReportAppendix()
        {
            DateTime FromDate = DateTime.Parse(GetFromDate);
            List<order_item_report_appendix> LocalOrderItemReportAppendix = QueryOrderItemReportAppendix(FromDate);
            OrderItemReportAp[] SendOrderItemReportAppendix = new OrderItemReportAp[LocalOrderItemReportAppendix.Count() ];

            for (int i = 0; i < LocalOrderItemReportAppendix.Count(); i++)
            {
                OrderItemReportAp tmp = new OrderItemReportAp();
                tmp.CreateDat = LocalOrderItemReportAppendix[i].createdat.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                tmp.Id = LocalOrderItemReportAppendix[i].Id.ToString();
                tmp.OrderItemReportId = LocalOrderItemReportAppendix[i].order_item_report_id.ToString();
                tmp.Appendix = LocalOrderItemReportAppendix[i].appendix;

                String tst = System.Text.Encoding.UTF8.GetString(tmp.Appendix, 0, tmp.Appendix.Length);

                SendOrderItemReportAppendix[i] = tmp;
            }
            int cntSentAppendix = SyncClient.PutOrderItemReportAp(SendOrderItemReportAppendix, DateTimeNow, false);
            return cntSentAppendix;
        }

        private int SendServiceProvider()
        {
            DateTime FromDate = DateTime.Parse(GetFromDate);
            service_provider LocalServiceProvider = QueryServiceProvider(FromDate);


            if (LocalServiceProvider != null)
            {
                ServiceProvider[] SendServiceProvider = new ServiceProvider[1];
                
                ServiceProvider tmp = new ServiceProvider();
                tmp.Timestamp = LocalServiceProvider.createdAt.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                tmp.Id = LocalServiceProvider.Id;
                tmp.Address1 = LocalServiceProvider.address_1;
                tmp.Address2 = LocalServiceProvider.address_2;
                tmp.City = LocalServiceProvider.city;
                tmp.CompanyName = LocalServiceProvider.company_name;
                tmp.CountryId = LocalServiceProvider.country_id;
                tmp.Phone1 = LocalServiceProvider.phone_1;
                tmp.Phone2 = LocalServiceProvider.phone_2;
                tmp.Taxnumber = LocalServiceProvider.tax_number;
                tmp.Zip = LocalServiceProvider.zip;
                if(LocalServiceProvider.zone_id < 1)
                {
                    tmp.ZoneId = 207;                       //Wenn die ZoneId Null ist, wird sie auf 207 (Wien) geändert. 
                }
                else
                {
                    tmp.ZoneId = LocalServiceProvider.zone_id;
                }
                
                //tmp.Appendix = LocalOrderItemReportAppendix[i].appendix;      funktioniert im Sync noch nicht

                SendServiceProvider[0] = tmp;

                return SyncClient.PutServiceProvider(SendServiceProvider, DateTimeNow, false);
            }
            return 0;
            
        }

        #endregion

        #region GetFromSync
        private List<sow_user> GetSowUser(int ServiceProviderId)
        {
            List<sow_user> ReturnList = new List<sow_user>();
            foreach (SowUser su in SyncClient.GetSowUser(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                sow_user tmp = QuerySowUser(su.Id);
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new sow_user();
                    NewValue = true;
                }
                tmp.email = su.Email;
                tmp.Id = su.Id;
                tmp.name = su.Name;
                tmp.registeredAt = System.DateTime.Parse(su.RegisteredDat);
                tmp.username = su.Username;
                if (NewValue)
                {
                    dbContext.Set<sow_user>().Add(tmp);
                }
                dbContext.SaveChanges();

                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        private List<order_header> GetOrderHeader(int ServiceProviderId)
        {
            List<order_header> ReturnList = new List<order_header>();
            foreach (OrderHeader oh in SyncClient.GetOrderHeader(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                order_header tmp = QueryOrderHeader(oh.Id);
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new order_header();
                    NewValue = true;
                }
                tmp.createdAt = System.DateTime.Parse(oh.CreateDat);
                tmp.currency_code = oh.CurrencyCode;
                tmp.customer_note = oh.CustomerNote;
                tmp.Id = oh.Id;
                tmp.ip_address = oh.IpAddress;
                tmp.orderpayment_type = oh.OrderpaymentType;
                tmp.order_state_id = oh.OrderStateId;
                tmp.sow_user_id = oh.SowUserId;
                tmp.subtotal = oh.Subtotal;
                tmp.subtotal_ex_tax = oh.SubtotalExTax;
                tmp.tax = oh.Tax;
                tmp.total = oh.Total;

                if (NewValue)
                {
                    dbContext.Set<order_header>().Add(tmp);
                }
                dbContext.SaveChanges();

                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        private List<order_detail> GetOrderDetail(int ServiceProviderId)
        {
            List<order_detail> ReturnList = new List<order_detail>();
            foreach (OrderDetail od in SyncClient.GetOrderDetail(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                order_detail tmp = QueryOrderDetail(od.Id);
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new order_detail();
                    NewValue = true;
                }                                
                tmp.address_1 = od.Address1;
                tmp.address_2 = od.Address2;
                tmp.city = od.City;
                tmp.company = od.Company;
                tmp.country_id = od.CountryId;
                tmp.createdAt = System.DateTime.Parse(od.CreateDat);
                tmp.Id = od.Id;
                tmp.phone_1 = od.Phone1;
                tmp.phone_2 = od.Phone2;
                tmp.tax_number = od.Taxnumber;
                tmp.zip = od.Zip;
                tmp.first_name = od.Firstname;
                tmp.last_name = od.Lastname;
                tmp.order_id = od.OrderId;
                tmp.zone_id = od.ZoneId;

                if (NewValue)
                {
                    dbContext.Set<order_detail>().Add(tmp);
                }
                dbContext.SaveChanges();
                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        private List<service_provider> GetServiceProvider(int ServiceProviderId)
        {
            List<service_provider> ReturnList = new List<service_provider>();
            foreach (ServiceProvider sp in SyncClient.GetServiceProvider(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                service_provider tmp = QueryServiceProvider(sp.Id);
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new service_provider();
                    NewValue = true;
                }
                tmp.address_1 = sp.Address1;
                tmp.address_2 = sp.Address2;
                tmp.city = sp.City;
                tmp.company_name = sp.CompanyName;
                tmp.country_id = sp.CountryId;
                tmp.createdAt = System.DateTime.Parse(sp.Timestamp);
                tmp.Id = sp.Id;
                tmp.phone_1 = sp.Phone1;
                tmp.phone_2 = sp.Phone2;
                tmp.tax_number = sp.Taxnumber;
                tmp.zip = sp.Zip;
                tmp.zone_id = sp.ZoneId;

                if (NewValue)
                {
                    dbContext.Set<service_provider>().Add(tmp);
                }
                dbContext.SaveChanges();
                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        private List<service> GetService(int ServiceProviderId)
        {
            List<service> ReturnList = new List<service>();
            foreach (Service s in SyncClient.GetService(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                service tmp = QueryService(s.Id);
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new service();
                    NewValue = true;
                }

                tmp.Id = s.Id;
                tmp.all_inclusive_extra_charge = s.AllInclExtra;
                tmp.amount_of_bookable_units = 10000;      //nicht im Sync inbegriffen, darf in der DB aber nicht Null sein
                tmp.category_id = s.CategoryId;
                tmp.createdAt = System.DateTime.Parse(s.CreateDat);
                tmp.intro_text = s.IntroText;
                tmp.name = s.Name;
                tmp.price_incl_tax = s.PriceInclTax;
                tmp.product_source_id = s.ProductSourceId;
                tmp.service_provider_id = s.ServiceProviderId;
                tmp.service_unit = s.Unit;
                tmp.short_name = s.Shortname;
                tmp.tax_type = s.TaxType;

                if (NewValue)
                {
                    dbContext.Set<service>().Add(tmp);
                }
                dbContext.SaveChanges();

                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        private List<order_item> GetOrderItem(int ServiceProviderId)
        {
            List<order_item> ReturnList = new List<order_item>();
            foreach (OrderItem OI in SyncClient.GetOrderItem(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                order_item tmp = QueryOrderItem(OI.Id);
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new order_item();
                    NewValue = true;
                }

                tmp.Id = OI.Id;
                tmp.addittional_cost = OI.AddCost;
                tmp.createdAt = System.DateTime.Parse(OI.CreateDat);
                tmp.final_price = OI.FinalPrice;
                tmp.final_price_without_tax = OI.FinalPriceWithoutTax;
                tmp.final_price_with_tax = OI.FinalPriceWithTax;
                tmp.is_all_inclusive = OI.IsAllIncl;
                tmp.is_confirmed = OI.IsConfirmed;
                tmp.is_finished = OI.IsFinished;
                tmp.option_price = OI.OptionPrice;
                tmp.order_id = OI.OrderId;
                tmp.per_item_tax = OI.PerItemTax;
                tmp.preferred_date_time = System.DateTime.Parse(OI.PreferredDatetime);
                tmp.price = OI.Price;
                tmp.quantity = OI.Quantity;
                tmp.service_id = OI.ServiceId;
                tmp.service_provider_comment = OI.Comment;           
                tmp.tax = OI.Tax;

                if (NewValue)
                {
                    dbContext.Set<order_item>().Add(tmp);
                }
                dbContext.SaveChanges();

                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        private List<order_item_report> GetOrderItemReport(int ServiceProviderId)
        {
            List<order_item_report> ReturnList = new List<order_item_report>();
            foreach (OrderItemReport OIR in SyncClient.GetOrderItemReport(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                order_item_report tmp = QueryOrderItemReport(new Guid(OIR.Id));
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new order_item_report();
                    NewValue = true;
                }
                tmp.Id = new Guid(OIR.Id);
                tmp.comment = OIR.ReportComment;
                tmp.createdat = System.DateTime.Parse(OIR.CreateDat);
                tmp.order_item_id = OIR.OrderItemId;
                if (NewValue)
                {
                    dbContext.Set<order_item_report>().Add(tmp);
                }
                dbContext.SaveChanges();
                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        private List<order_item_report_appendix> GetOrderItemReportAppendix(int ServiceProviderId)
        {
            List<order_item_report_appendix> ReturnList = new List<order_item_report_appendix>(); 
            foreach (OrderItemReportAp OIRA in SyncClient.GetOrderItemReportAp(GetFromDate, DateTimeNow, ServiceProviderId))
            {
                order_item_report_appendix tmp = QueryOrderItemReportAppendix(new Guid(OIRA.Id));
                Boolean NewValue = false;
                if (tmp == null)
                {
                    tmp = new order_item_report_appendix();
                    NewValue = true;
                }
                tmp.Id = new Guid(OIRA.Id);
                tmp.appendix = OIRA.Appendix;
                tmp.order_item_report_id = new Guid(OIRA.OrderItemReportId);
                tmp.createdat = System.DateTime.Parse(OIRA.CreateDat);
                //tmp.deletedat =??

                if (NewValue)
                {
                    dbContext.Set<order_item_report_appendix>().Add(tmp);
                }
                dbContext.SaveChanges();
                ReturnList.Add(tmp);
            }
            return ReturnList;
        }
        #endregion
    }
}
