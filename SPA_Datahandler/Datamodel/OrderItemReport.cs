using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler.Datamodel
{
    [DataContract]
    public class OrderItemReport
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public DateTime ReportDate { get; set; }
        [DataMember]
        public List<OrderItemReportAppendix> Appendix { get; set; }

        public OrderItemReport()
        {

        }
    }
}
