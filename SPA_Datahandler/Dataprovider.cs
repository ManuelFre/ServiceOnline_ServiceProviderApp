using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using SPA_Datahandler.Datamodel;

namespace SPA_Datahandler
{
    public class Dataprovider
    {
        public Dataprovider()
        {
            //Herstellung einer Datenbankverbindung:
            dbContext = new DbServiceProviderAppEntities();
        }

        public List<country> QueryAllCountries()
        {
            SimpleDatabaseFunctions<country> SDF = new SimpleDatabaseFunctions<country>();
            return SDF.QueryAll();
        }

        public List<OrderSummary> QueryOrderSummaries()
        {
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
                    Zip = x.od.zip
                });

            return query.ToList();
        }

        public List<OrderSummary> QueryPastOrders()
        {
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
                    City = x.od.city,
                    //Customername = x.od.last_name,
                    Customername = String.Format("{0} {1}", x.od.first_name, x.od.last_name),
                    IsAllInclusive = x.oi.is_all_inclusive,
                    IsFinished = x.oi.is_finished,
                    Phone = x.od.phone_2,
                    PreferedDate = x.oi.preferred_date_time,
                    Servicedescription = x.sv.name,
                    Zip = x.od.zip
                });

            return query.ToList();
        }

        public List<OrderSummary> QueryUpcomingOrders()
        {
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
                    City = x.od.city,
                    //Customername = x.od.last_name,
                    Customername = String.Format("{0} {1}", x.od.first_name, x.od.last_name),
                    IsAllInclusive = x.oi.is_all_inclusive,
                    IsFinished = x.oi.is_finished,
                    Phone = x.od.phone_2,
                    PreferedDate = x.oi.preferred_date_time,
                    Servicedescription = x.sv.name,
                    Zip = x.od.zip
                });

            return query.ToList();
        }
        public DetailedClass QueryDetailView(long OrderItemId)
        {
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
                    Finalprice = x.oi.final_price_with_tax,
                    OrderedDateTime = x.oi.createdAt,
                    CustomerNotice = x.oh.customer_note,
                    IsFinished = x.oi.is_finished,
                    OrderId = x.oi.order_id
                });

            return query.Single();
        }

        public order_detail QueryOrderDetail(long OrderItemId)
        {
            var query = from od in dbContext.order_detail
                        where od.Id == OrderItemId
                        select od;

            return query.First();
        }

        public order_item QueryOrderItem(long OrderItemId)
        {
            var query = from oi in dbContext.order_item
                        where oi.Id == OrderItemId
                        select oi;

            return query.First();
        }

        public void UpdateDataBase()
        {
            dbContext.SaveChanges();

        }

        protected DbServiceProviderAppEntities dbContext;
        //public List<country> InsertAndShowCountry(string Name, String iso_2, String iso_3)
        //{
        //    //Quelle: https://docs.telerik.com/devtools/wpf/consuming-data/linq-to-ado-net-entity-data-model
        //    //Query auf Datenbank absetzen und auswerten
        //    var query = (from p in dbContext.country.Cast<country>() where p.Id > -1 select p);

        //    //neues Datenbankobjekt anlegen
        //    country Country = new country();
        //    Country.Id = query.ToList().Count()+2;
        //    Country.name = Name;
        //    Country.iso_2 = iso_2;
        //    Country.iso_3 = iso_3;
        //    Country.createdAt = DateTime.Now;
        //    Country.deletedAt = null;

        //    //Datenbankobjekt in DB importieren
        //    dbContext.country.Add(Country);
        //    dbContext.SaveChanges();

        //    query = (from p in dbContext.country.Cast<country>() where p.Id > -1 select p);

        //    List<country> ReturnCountries = new List<country>();
        //    foreach (country item in query.ToList())
        //    {
        //        ReturnCountries.Add(item);
        //    }
        //    return ReturnCountries;
        //}







        //public bool InsertOrder_Detail(country Country,int Id, string address1, string address2, string city, string company, int countryId, DateTime createdAt, DateTime deletedAt,string firstname, string lastname,order_header orderHeader,string phone1, string phone2,int orderid, string taxNumber, string zip, zone zone, int zoneId)
        //{
        //    try
        //    {
        //        order_detail orderDetail = new order_detail
        //        {
        //            country = Country,
        //            Id = Id,
        //            address_1 = address1,
        //            address_2 = address2,
        //            city = city,
        //            company = company,
        //            country_id = countryId,
        //            createdAt = createdAt,
        //            deletedAt = deletedAt,
        //            first_name = firstname,
        //            last_name = lastname,
        //            order_header = orderHeader,
        //            phone_1 = phone1,
        //            phone_2 = phone2,
        //            order_id = orderid,
        //            tax_number = taxNumber,
        //            zip = zip,
        //            zone = zone,
        //            zone_id = zoneId
        //        };

        //        dbContext.order_detail.Add(orderDetail);
        //        dbContext.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        ////public bool ClearTableCountry()
        ////{
        ////    try
        ////    {
        ////        foreach (country cntry in QueryAllCountries())
        ////        {
        ////            dbContext.country.Remove(cntry);
        ////        }
        ////    }catch
        ////    {
        ////        return false;
        ////    }
        ////    return true;

        ////}


        //public bool InsertCountry(int Id, string Name, String iso_2, String iso_3, DateTime createdAt, DateTime? deletedAt)
        //{
        //    country cntry = new country();
        //    try
        //    {
        //        cntry = new country
        //        {
        //            Id = Id,
        //            name = Name,
        //            iso_2 = iso_2,
        //            iso_3 = iso_3,
        //            createdAt = createdAt,
        //            deletedAt = deletedAt
        //        };
        //        dbContext.country.Add(cntry);
        //        dbContext.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        dbContext.country.Remove(cntry);
        //        return false;
        //    }
        //    return true;
        //}




        //public bool InsertOrderHeader(int id, int sowUserId, double total, double subtotal, double subtotalExTax, double tax, string orderpaymentType, string currencyCode, string ipAddress, string customerNote, int orderStateId, DateTime createdAt, DateTime? deletedAt, ICollection<order_detail> orderDetail, sow_user sowUser, order_state orderState, ICollection<order_item> orderItem)
        //{
        //    try
        //    {

        //        order_header orderHeader = new order_header
        //        {
        //            Id = id,
        //            sow_user_id = sowUserId,
        //            total = total,
        //            subtotal = subtotal,
        //            subtotal_ex_tax = subtotalExTax,
        //            tax = tax,
        //            orderpayment_type = orderpaymentType,
        //            currency_code = currencyCode,
        //            ip_address = ipAddress,
        //            customer_note = customerNote,
        //            order_state_id = orderStateId,
        //            createdAt = createdAt,
        //            deletedAt = deletedAt,
        //            order_detail = orderDetail,
        //            sow_user = sowUser,
        //            order_state = orderState,
        //            order_item = orderItem
        //        };
        //        dbContext.order_header.Add(orderHeader);
        //        dbContext.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}
