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
        public int Id { get; set; }

        [DataMember]
        public int OrderItemReportId { get; set; }
        [DataMember]
        public byte[] Picture { get; set; }

        public OrderItemReportAppendix()
        {

        }
    }
}
