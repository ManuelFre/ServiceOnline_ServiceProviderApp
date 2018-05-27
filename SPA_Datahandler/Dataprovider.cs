using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SPA_Datahandler
{
    public class Dataprovider
    {
        protected DbServiceProviderAppEntities dbContext;

        public Dataprovider()
        {
            //Herstellung einer Datenbankverbindung:
            dbContext = new DbServiceProviderAppEntities();
            dbContext.Configuration.AutoDetectChangesEnabled = true;
        }

        public List<country> QueryAllCountries()
        {
            
            SimpleDatabaseFunctions<country> SDF = new SimpleDatabaseFunctions<country>();
            return SDF.QueryAll();
        }

        public List<OrderSummary> QueryOrderSummaries()
        {
            dbContext = new DbServiceProviderAppEntities();

            var query = (from od in dbContext.order_detail
                         join oi in dbContext.order_item on od.order_id equals oi.order_id
                         join sv in dbContext.service on oi.service_id equals sv.Id
                         orderby oi.preferred_date_time
                         select new { od, oi, sv })
                .AsEnumerable()
                .Select(x => new OrderSummary()
                {
                    OrderItemId = x.oi.Id,
                    //Address = od.address_1,
                    Address = String.Format("{0} {1}", x.od.address_1, x.od.address_2),
                    BookedItems = x.oi.quantity,
                    City = x.od.city,
                    //Customername = x.od.last_name,
                    Customername = String.Format("{0} {1}", x.od.first_name, x.od.last_name),
                    IsAllInclusive = x.oi.is_all_inclusive,
                    IsFinished = x.oi.is_finished,
                    Phone = x.od.phone_2,
                    PreferedDate = x.oi.preferred_date_time,
                    Servicedescription = x.sv.name,
                    Zip = x.od.zip,
                    IsConfirmed = x.oi.is_confirmed
                });

            return query.ToList();
        }

        public List<OrderSummary> QueryOrders()
        {
            dbContext = new DbServiceProviderAppEntities();

            var query = (from od in dbContext.order_detail
                         join oi in dbContext.order_item on od.order_id equals oi.order_id
                         join sv in dbContext.service on oi.service_id equals sv.Id
                         where oi.preferred_date_time < System.DateTime.Now
                         orderby oi.preferred_date_time descending
                         select new { od, oi, sv })
                .AsEnumerable()
                .Select(x => new OrderSummary()
                {
                    OrderItemId = x.oi.Id,
                    //Address = od.address_1,
                    Address = String.Format("{0} {1}", x.od.address_1, x.od.address_2),
                    BookedItems = x.oi.quantity,
                    ServiceUnit = x.sv.service_unit,
                    City = x.od.city,
                    //Customername = x.od.last_name,
                    Customername = String.Format("{0} {1}", x.od.first_name, x.od.last_name),
                    IsAllInclusive = x.oi.is_all_inclusive,
                    IsFinished = x.oi.is_finished,
                    Phone = x.od.phone_2,
                    PreferedDate = x.oi.preferred_date_time,
                    Servicedescription = x.sv.name,
                    Zip = x.od.zip,
                    IsConfirmed = x.oi.is_confirmed
                });

            return query.ToList();
        }

        public List<OrderSummary> QueryUpcomingOrders()
        {
            dbContext = new DbServiceProviderAppEntities();

            var query = (from od in dbContext.order_detail
                         join oi in dbContext.order_item on od.order_id equals oi.order_id
                         join sv in dbContext.service on oi.service_id equals sv.Id
                         where oi.preferred_date_time > System.DateTime.Now
                         orderby oi.preferred_date_time
                         select new { od, oi, sv })
                .AsEnumerable()
                .Select(x => new OrderSummary()
                {
                    OrderItemId = x.oi.Id,
                    //Address = od.address_1,
                    Address = String.Format("{0} {1}", x.od.address_1, x.od.address_2),
                    BookedItems = x.oi.quantity,
                    ServiceUnit = x.sv.service_unit,
                    City = x.od.city,
                    //Customername = x.od.last_name,
                    Customername = String.Format("{0} {1}", x.od.first_name, x.od.last_name),
                    IsAllInclusive = x.oi.is_all_inclusive,
                    IsFinished = x.oi.is_finished,
                    Phone = x.od.phone_2,
                    PreferedDate = x.oi.preferred_date_time,
                    Servicedescription = x.sv.name,
                    Zip = x.od.zip,
                    IsConfirmed = x.oi.is_confirmed
                });

            return query.ToList();
        }
        public DetailedClass QueryDetailView(long OrderItemId)
        {
            dbContext = new DbServiceProviderAppEntities();

            var query = (from od in dbContext.order_detail
                         join oi in dbContext.order_item on od.order_id equals oi.order_id
                         join sv in dbContext.service on oi.service_id equals sv.Id
                         join oh in dbContext.order_header on od.order_id equals oh.Id
                         join cr in dbContext.sow_user on oh.sow_user_id equals cr.Id
                         where oi.Id == OrderItemId
                         select new { od, oi, sv, oh, cr })
                .AsEnumerable()
                .Select(x => new DetailedClass()
                {
                    CustomerId = x.cr.Id,
                    Firstname = x.od.first_name,
                    Lastname = x.od.last_name,
                    Address = String.Format("{0} {1}", x.od.address_1, x.od.address_2),
                    Zip = x.od.zip,
                    City = x.od.city,
                    Phone = x.od.phone_2,
                    Email = x.cr.email,
                    OrderItemId = x.oi.Id,
                    PreferedDate = x.oi.preferred_date_time,
                    Servicedescription = x.sv.name,
                    BookedItems = x.oi.quantity,
                    IsAllInclusive = x.oi.is_all_inclusive,
                    Finalprice = x.oi.final_price,
                    OrderedDateTime = x.oi.createdAt,
                    CustomerNotice = x.oh.customer_note,
                    IsFinished = x.oi.is_finished,
                    OrderId = x.oi.order_id,
                    IsConfirmed = x.oi.is_confirmed,
                    AddittionalCost = x.oi.addittional_cost,
                    ServiceProviderComment = x.oi.service_provider_comment,
                    ServiceUnit = x.sv.service_unit
                });

            DetailedClass RetVal = query.Single();

            // Holen der Order_Item_Reports zum Order_Item
            var queryReport = from oim in dbContext.order_item_report
                              where oim.order_item_id == OrderItemId
                              select new OrderItemReport
                              {
                                  Id = oim.Id,
                                  Comment = oim.comment,
                                  ReportDate = oim.report_date
                              };
            List<OrderItemReport> OIM = queryReport.ToList();

            // Holen der Order_Item_Report_Appendix zu allen Order_Item_Reports
            for (int i = 0; i < OIM.Count; i++)
            {
                int OrderITemReportId = OIM[i].Id;

                var queryAppendix = from oima in dbContext.order_item_report_appendix
                                    where oima.order_item_report_id == OrderITemReportId
                                    select new OrderItemReportAppendix
                                    {
                                        Id = oima.Id,
                                        OrderItemReportId = oima.order_item_report_id,
                                        Picture = oima.appendix
                                    };

                List<OrderItemReportAppendix> OIMA = queryAppendix.ToList();
                OIM[i].Appendix = OIMA;
            }
            RetVal.OrderItemReports = OIM;

            return RetVal;
        }

        //Updated Änderungen am Detailitem
        public bool UpdateOrderItemData(DetailedClass DetailToUpdate)
        {
            dbContext = new DbServiceProviderAppEntities();
            //Auswerten des Order_item aus der DB
            order_item OriginalOrderItem = (from oi in dbContext.order_item
                                            where oi.Id == DetailToUpdate.OrderItemId
                                            select oi).FirstOrDefault(); //changed from first() to firstordefault()

            //order_item OriginalOrderItem = new order_item();
            if (OriginalOrderItem != null)
            {
                //Änderungsfähige Daten übernehmen
                OriginalOrderItem.addittional_cost = DetailToUpdate.AddittionalCost;
                OriginalOrderItem.final_price = DetailToUpdate.Finalprice;
                OriginalOrderItem.is_all_inclusive = DetailToUpdate.IsAllInclusive;
                OriginalOrderItem.is_confirmed = DetailToUpdate.IsConfirmed;
                OriginalOrderItem.is_finished = DetailToUpdate.IsFinished;
                OriginalOrderItem.preferred_date_time = DetailToUpdate.PreferedDate;
                OriginalOrderItem.service_provider_comment = DetailToUpdate.ServiceProviderComment;
                OriginalOrderItem.final_price_without_tax = OriginalOrderItem.final_price / 1.2;   //Falls der Final_Price geändert wurde
                OriginalOrderItem.final_price_with_tax = OriginalOrderItem.final_price;           //Falls der Final_Price geändert wurde
                OriginalOrderItem.tax = OriginalOrderItem.final_price_with_tax - OriginalOrderItem.final_price_without_tax;
                OriginalOrderItem.per_item_tax = OriginalOrderItem.tax / OriginalOrderItem.quantity;
                OriginalOrderItem.createdAt = DateTime.Now;
               

                //schreiben der Änderung in die spa_changes Tabelle
                spa_changes chng = new spa_changes();
                chng.order_id = OriginalOrderItem.Id;
                chng.change_date = DateTime.Now;

                dbContext.Set<spa_changes>().Add(chng);
                
                //Änderungen speichern

                dbContext.SaveChanges();
                
                return true;
            }
            else
            {
                return false;
            }
            



        }

        public void AddOrderItemReport(OrderItemReport NewReport)
        {
            dbContext = new DbServiceProviderAppEntities();

            //Holen der maximalen Order_item_report_id:
            int NextId = (from oim in dbContext.order_item_report
                          select oim.Id).Max() + 1;

            //Umwandeln des OrderItemReport in das DB-Objekt
            order_item_report DbNewReport = new order_item_report
            {
                report_date = NewReport.ReportDate,
                order_item_id = NewReport.OrderItemId,
                comment = NewReport.Comment,
                Id = NextId
            };

            dbContext.Set<order_item_report>().Add(DbNewReport);


            //Umwandeln der OrderItemReportAppendix in die DB-Objekte
            foreach (OrderItemReportAppendix oima in NewReport.Appendix)
            {
                order_item_report_appendix DbOima = new order_item_report_appendix
                {
                    order_item_report_id = DbNewReport.Id,
                    appendix = oima.Picture
                };

                dbContext.Set<order_item_report_appendix>().Add(DbOima);
            }

            //schreiben der Änderung in die spa_changes Tabelle
            dbContext.Set<spa_changes>().Add(new spa_changes { order_id = NewReport.OrderItemId, change_date = DateTime.Now });
            UpdateDataBase();
        }

        public DateTime QueryLastSync()
        {
            dbContext = new DbServiceProviderAppEntities();

            return (from ls in dbContext.spa_synctimes
                    select ls.synctime).Max<DateTime>();
        }
        public bool LogInAndCheckUserData(string username, string password)
        {
            dbContext = new DbServiceProviderAppEntities();

            var query = from ud in dbContext.service_provider_login
                        where ud.username == username && ud.password == password
                        select ud;

            List<service_provider_login> Userdata = query.ToList();

            //gibt True zurück, wenn es genau einen DB-Eintrag mit dem Usernamen und Passwort gibt.
            if (Userdata.Count() == 1)
            {
                dbContext.Set<spa_log_in>().Add(new spa_log_in { user_id = Userdata[0].Id, last_login = DateTime.Now, is_logged_in ="Y" });
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool LogOut()
        {
            var query = from li in dbContext.spa_log_in
            where li.is_logged_in == "Y"
            select li;

            foreach(spa_log_in sli in query.ToList())
            {
                sli.is_logged_in = "N";
            }

            return true;
        }

        public string GetLoggedInUsername()
        {
            dbContext = new DbServiceProviderAppEntities();
            // Mittleres Query wertet die Service_Provider_Id des zuletzt eingeloggten User aus,
            // äußeres Query holt mittels der ID den Username aus der service_provider_login Tabelle.

            DateTime LastGuiltyLogIn = DateTime.Now.AddHours(-4800);        //ToDo: LastLogIn noch ändern!!!

            var query = (from spl in dbContext.service_provider_login
                         where spl.service_provider_id ==
                            (
                                from li in dbContext.spa_log_in
                                where li.last_login > LastGuiltyLogIn
                                && li.is_logged_in =="Y"
                                orderby li.last_login descending
                                select li.user_id
                            ).First()
                         select spl.username);
            return query.First().ToString();
        }

        public order_detail QueryOrderDetail(long OrderItemId)
        {
            dbContext = new DbServiceProviderAppEntities();

            var query = from od in dbContext.order_detail
                        where od.Id == OrderItemId
                        select od;

            return query.First();
        }

        public order_item QueryOrderItem(long OrderItemId)
        {
            dbContext = new DbServiceProviderAppEntities();

            var query = from oi in dbContext.order_item
                        where oi.Id == OrderItemId
                        select oi;

            return query.First();
        }

        public void UpdateDataBase()
        {
            dbContext.SaveChanges();
        }

        public ServiceProviderData QueryServiceProviderData()
        {
            dbContext = new DbServiceProviderAppEntities();

            //prüfen ob jemand und wenn ja, wer angemeldet ist

            DateTime LastGuiltyLogIn = DateTime.Now.AddHours(-4800);        //ToDo: LastLogIn noch ändern!!!

            var CheckQuery = from li in dbContext.spa_log_in
                        where li.last_login > LastGuiltyLogIn          
                        && li.is_logged_in == "Y"
                        orderby li.last_login descending
                        select li;

            if(CheckQuery.ToList().Count() == 1)
            {
                int UserId = CheckQuery.ToList()[0].user_id;

                //STAMMDATEN:
                var MasterDataQuery = from sp in dbContext.service_provider
                                      where sp.Id == UserId
                                      select new ServiceProviderData
                                      {
                                          Id = sp.Id,
                                          CompanyName = sp.company_name,
                                          Address = sp.address_1,
                                          City = sp.city,
                                          Zip = sp.zip,
                                          Phone = sp.phone_1
                                      };
                ServiceProviderData RetVal = MasterDataQuery.FirstOrDefault();


                //ABGESCHLOSSENE AUFTRÄGE:
                var CntCompletedOrdersQuery = from oi in dbContext.order_item
                                              join s in dbContext.service on oi.service_id equals s.Id
                                              join sp in dbContext.service_provider on s.service_provider_id equals sp.Id
                                              where sp.Id == UserId && oi.is_finished =="Y" && oi.is_confirmed == "Y"
                                              select oi;

                RetVal.CntCompletedOrders = CntCompletedOrdersQuery.ToList().Count();

                //OFFENE AUFTRÄGE:
                var CntOpenOrdersQuery = from oi in dbContext.order_item
                                              join s in dbContext.service on oi.service_id equals s.Id
                                              join sp in dbContext.service_provider on s.service_provider_id equals sp.Id
                                              where sp.Id == UserId && oi.is_finished == "N" && oi.is_confirmed == "Y"
                                              select oi;
                RetVal.CntOpenOrders = CntOpenOrdersQuery.ToList().Count();

                //LUKRIERTER UMSATZ
                double SumTargetedSales = 0;

                foreach (var orderitem in CntCompletedOrdersQuery.ToList())
                {
                    SumTargetedSales += orderitem.final_price_with_tax;
                }
                RetVal.SumTargetedSales = SumTargetedSales;


                //Unbestätigte Aufträge
                var CntUnconfirmedOrdersQuery = from oi in dbContext.order_item
                                         join s in dbContext.service on oi.service_id equals s.Id
                                         join sp in dbContext.service_provider on s.service_provider_id equals sp.Id
                                         where sp.Id == UserId && (oi.is_confirmed == "N" || oi.is_confirmed == null)
                                                select oi;
                RetVal.CntUnconfirmedOrders = CntUnconfirmedOrdersQuery.ToList().Count();

                //angenommene Aufträge diese Woche

                int ThisMonth = DateTime.Now.Month;

                var CntOpenOrdersThisMonthQuery = from oi in dbContext.order_item
                                                join s in dbContext.service on oi.service_id equals s.Id
                                                join sp in dbContext.service_provider on s.service_provider_id equals sp.Id
                                                where sp.Id == UserId && oi.is_finished == "N" && oi.is_confirmed == "Y" && oi.preferred_date_time.Month == ThisMonth
                                                select oi;
                RetVal.CntOpenOrdersThisMonth = CntOpenOrdersThisMonthQuery.ToList().Count();

                return RetVal;
            }
            return null;
        }


        public bool UpdateServiceProviderData(ServiceProviderData ServiceProvider)
        {
            service_provider OrignialServiceProvider = (from sp in dbContext.service_provider
                                where sp.Id == ServiceProvider.Id
                                select sp).FirstOrDefault();

            if(OrignialServiceProvider != null)
            {
                OrignialServiceProvider.address_1 = ServiceProvider.Address;
                OrignialServiceProvider.city = ServiceProvider.City;
                OrignialServiceProvider.createdAt = DateTime.Now;
                OrignialServiceProvider.phone_1 = ServiceProvider.Phone;
                OrignialServiceProvider.zip = ServiceProvider.Zip;

                dbContext.SaveChanges();

                return true;
            }

            return false;
            
        }


    }
}
