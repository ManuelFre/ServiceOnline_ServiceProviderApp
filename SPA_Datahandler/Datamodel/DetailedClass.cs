using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler.Datamodel
{
    [DataContract]
    public class DetailedClass
    {
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string Firstname { get; set; }
        [DataMember]
        public string Lastname { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int OrderItemId { get; set; }
        [DataMember]
        public long OrderId { get; set; }
        [DataMember]
        public DateTime PreferedDate { get; set; }
        [DataMember]
        public string Servicedescription { get; set; }
        [DataMember]
        public int BookedItems { get; set; }
        [DataMember]
        public string IsAllInclusive { get; set; }
        [DataMember]
        public double Finalprice { get; set; }
        [DataMember]
        public DateTime OrderedDateTime { get; set; }
        [DataMember]
        public string CustomerNotice { get; set; }
        [DataMember]
        public string IsFinished { get; set; }
        [DataMember]
        public string IsConfirmed { get; set; }
        [DataMember]
        public double? AddittionalCost { get; set; }
        [DataMember]
        public string ServiceProviderComment { get; set; }
        [DataMember]
        public string ServiceUnit { get; set; }
        [DataMember]
        public List<OrderItemReport> OrderItemReports { get; set; }


        private Dataprovider dp;
        public DetailedClass()
        {
            dp = new Dataprovider();
        }


        public DetailedClass GetDetailView(OrderSummary selectedOrder)
        {
            return dp.QueryDetailView(selectedOrder.OrderItemId);
        }
        public void UpdateOrderDetailItem()
        {
            dp.UpdateOrderItemData(this);
        }

        public void AddOrderItemReport(OrderItemReport NewReport)
        {
            dp.AddOrderItemReport(NewReport);
        }
    }
}
