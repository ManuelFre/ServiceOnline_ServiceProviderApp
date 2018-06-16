using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler.Datamodel
{
    [DataContract]
    public class OrderItemReportAppendix
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid OrderItemReportId { get; set; }
        [DataMember]
        public byte[] Picture { get; set; }

        public OrderItemReportAppendix()
        {

        }
    }
}
