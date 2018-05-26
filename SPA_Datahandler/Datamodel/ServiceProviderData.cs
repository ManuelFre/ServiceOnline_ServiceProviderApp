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


        public ServiceProviderData()
        {

        }

        public ServiceProviderData(int id, string companyName, string address, string city, string zip, string phone1, DateTime createdAt, int cntCompletedOrders, int cntOpenOrders, double sumTargetedSales)
        {
            Id = id;
            CompanyName = companyName;
            Address = address;
            City = city;
            Zip = zip;
            Phone = phone1;
            CreatedAt = createdAt;
            CntCompletedOrders = cntCompletedOrders;
            CntOpenOrders = cntOpenOrders;
            SumTargetedSales = sumTargetedSales;
        }
    }
}
