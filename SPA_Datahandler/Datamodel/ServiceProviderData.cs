using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler.Datamodel
{
    public class ServiceProviderData
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }       
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CntCompletedOrders { get; set; }
        public int CntOpenOrders { get; set; }
        public double SumTargetedSales { get; set; }
        public int CntUnconfirmedOrders { get; set; }
        public int CntOpenOrdersThisMonth { get; set; }

        public ServiceProviderData()
        {

        }

    }
}
