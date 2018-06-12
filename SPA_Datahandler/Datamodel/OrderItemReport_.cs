using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPA_Datahandler.Datamodel
{
    [DataContract]
    public class OrderItemReport_
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public DateTime ReportDate { get; set; }
        [DataMember]
        public int OrderItemId { get; set; }
        [DataMember]
        public List<OrderItemReportAppendix> Appendix { get; set; }

        public string Visibility { get; set; }
        public OrderItemReport_()
        {
            Visibility = "Collapsed";
        }
    }
}
