using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler.Datamodel
{
    [DataContract]
    public class OrderSummary
    {
        [DataMember]
        public long OrderItemId { get; set; }
        [DataMember]
        public string Customername { get; set; }
        [DataMember]
        public string Servicedescription { get; set; }
        [DataMember]
        public DateTime PreferedDate { get; set; }
        [DataMember]
        public int BookedItems { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string IsAllInclusive { get; set; }
        [DataMember]
        public string IsFinished { get; set; }
        [DataMember]
        public string IsAccepted { get; set; }


        private Dataprovider dp;
        public OrderSummary()
        {
            dp = new Dataprovider();
        }

        public List<OrderSummary> GetAllOrderSummaries()
        {
            return dp.QueryOrderSummaries();
        }
        public List<OrderSummary> GetPastOrderSummaries()
        {
            return dp.QueryOrders();
        }
        public List<OrderSummary> GetUpcomingOrderSummaries()
        {
            return dp.QueryUpcomingOrders();
        }


    }
}
