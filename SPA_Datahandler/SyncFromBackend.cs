using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA_Datahandler.BackendDatabaseHandler;

namespace SPA_Datahandler
{
    public class SyncFromBackend
    {
        private SimpleDatabaseFunctions<service_provider> ServiceProviderDbF;
        private BackendDatabaseHandling<service_provider> ServiceProviderBkd;
        private SimpleDatabaseFunctions<country> CountryDbF;
        private BackendDatabaseHandling<country> CountryBkd;
        private SimpleDatabaseFunctions<zone> ZoneDbF;
        private BackendDatabaseHandling<zone> ZoneBkd;
        private SimpleDatabaseFunctions<service_category> ServiceCategoryDbF;
        private BackendDatabaseHandling<service_category> ServiceCategoryBkd;
        private SimpleDatabaseFunctions<service> ServiceDbF;
        private BackendDatabaseHandling<service> ServiceBkd;
        private SimpleDatabaseFunctions<sow_user> SowUserDbF;
        private BackendDatabaseHandling<sow_user> SowUserBkd;
        private SimpleDatabaseFunctions<sow_user_delivery_address> SowUserDeliveryAddressDbF;
        private BackendDatabaseHandling<sow_user_delivery_address> SowUserDeliveryAddressBkd;
        private SimpleDatabaseFunctions<order_state> OrderStateDbF;
        private BackendDatabaseHandling<order_state> OrderStateBkd;
        private SimpleDatabaseFunctions<order_header> OrderHeaderDbF;
        private BackendDatabaseHandling<order_header> OrderHeaderBkd;
        private SimpleDatabaseFunctions<order_item> OrderItemDbF;
        private BackendDatabaseHandling<order_item> OrderItemBkd;
        private SimpleDatabaseFunctions<order_detail> OrderDetailDbF;
        private BackendDatabaseHandling<order_detail> OrderDetailBkd;
        public SyncFromBackend()
        {
            ServiceProviderDbF = new SimpleDatabaseFunctions<service_provider>();
            ServiceProviderBkd = new BackendDatabaseHandling<service_provider>();
            CountryDbF = new SimpleDatabaseFunctions<country>();
            CountryBkd = new BackendDatabaseHandling<country>();
            ZoneDbF = new SimpleDatabaseFunctions<zone>();
            ZoneBkd = new BackendDatabaseHandling<zone>();
            ServiceCategoryDbF = new SimpleDatabaseFunctions<service_category>();
            ServiceCategoryBkd = new BackendDatabaseHandling<service_category>();
            ServiceDbF = new SimpleDatabaseFunctions<service>();
            ServiceBkd = new BackendDatabaseHandling<service>();
            SowUserDbF = new SimpleDatabaseFunctions<sow_user>();
            SowUserBkd = new BackendDatabaseHandling<sow_user>();
            SowUserDeliveryAddressDbF = new SimpleDatabaseFunctions<sow_user_delivery_address>();
            SowUserDeliveryAddressBkd = new BackendDatabaseHandling<sow_user_delivery_address>();
            OrderStateDbF = new SimpleDatabaseFunctions<order_state>();
            OrderStateBkd = new BackendDatabaseHandling<order_state>();
            OrderHeaderDbF = new SimpleDatabaseFunctions<order_header>();
            OrderHeaderBkd = new BackendDatabaseHandling<order_header>();
            OrderItemDbF = new SimpleDatabaseFunctions<order_item>();
            OrderItemBkd = new BackendDatabaseHandling<order_item>();
            OrderDetailDbF = new SimpleDatabaseFunctions<order_detail>();
            OrderDetailBkd = new BackendDatabaseHandling<order_detail>();
        }

        public bool StartSync()
        {
            DeleteTables();
            //if (DeleteTables())
            //{
            //return  InsertCountriesFromSync() && 
            //InsertServiceCategoryFromSync() &&
            //InsertServiceProviderFromSync() && 
            //InsertServiceCategoryFromSync() && 
            //InsertServiceFromSync() && 
            //InsertSowUserFromSync() && 
            //InsertSowUserDeliveryAddressFromSync() && 
            //InsertOrderStateFromSync() &&
            //InsertOrderHeaderFromSync() && 
            //InsertOrderItemFromSync() && 
            //InsertOrderDetailFromSync();       //&& InsertZonesFromSync()
            InsertCountriesFromSync();
            InsertServiceCategoryFromSync();
            InsertServiceProviderFromSync();
            InsertServiceCategoryFromSync();
            InsertServiceFromSync();
            InsertSowUserFromSync();
            InsertSowUserDeliveryAddressFromSync();
            InsertOrderStateFromSync();
            InsertOrderHeaderFromSync();
            InsertOrderItemFromSync();
            InsertOrderDetailFromSync();
            //}
            return false;
        }

        private bool DeleteTables()
        {
            return OrderDetailDbF.ClearTable() && /*OrderItemDbF.ClearTable() &&*/ OrderHeaderDbF.ClearTable() && OrderStateDbF.ClearTable() && SowUserDbF.ClearTable() 
                && SowUserDeliveryAddressDbF.ClearTable() && ServiceDbF.ClearTable() && ServiceCategoryDbF.ClearTable() 
                && ServiceProviderDbF.ClearTable()  && CountryDbF.ClearTable();      //&& ZoneDbF.ClearTable()

        }


        private bool InsertCountriesFromSync()
        {
            List<country> itemsList = CountryBkd.GetBackendItems("country");
            try
            {               
                foreach (var item in itemsList)
                {
                    CountryDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        private bool InsertZonesFromSync()
        {
            List<zone> itemsList = ZoneBkd.GetBackendItems("zone");
            try
            {
                
                foreach (var item in itemsList)
                {
                    ZoneDbF.InsertItem(item);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool InsertServiceProviderFromSync()
        {

            List<service_provider> itemsList = ServiceProviderBkd.GetBackendItems("service_provider");
            try
            {
                
                foreach (var item in itemsList)
                {
                    ServiceProviderDbF.InsertItem(item);
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        private bool InsertServiceCategoryFromSync()
        {
            List<service_category> itemsList = ServiceCategoryBkd.GetBackendItems("service_category");
            try
            {
                foreach (var item in itemsList)
                {
                    ServiceCategoryDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool InsertServiceFromSync()
        {
            List<service> itemsList = ServiceBkd.GetBackendItems("service");
            try
            {
                foreach (var item in itemsList)
                {
                    ServiceDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private bool InsertSowUserFromSync()
        {
            List<sow_user> itemsList = SowUserBkd.GetBackendItems("sow_user");
            try
            {
                foreach (var item in itemsList)
                {
                    SowUserDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool InsertSowUserDeliveryAddressFromSync()
        {
            List<sow_user_delivery_address> itemsList = SowUserDeliveryAddressBkd.GetBackendItems("sow_user_delivery_address");
            try
            {
                foreach (var item in itemsList)
                {
                    SowUserDeliveryAddressDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool InsertOrderStateFromSync()
        {
            List<order_state> itemsList = OrderStateBkd.GetBackendItems("order_state");
            try
            {
                foreach (var item in itemsList)
                {
                    OrderStateDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool InsertOrderHeaderFromSync()
        {
            List<order_header> itemsList = OrderHeaderBkd.GetBackendItems("order_header");
            try
            {
                foreach (var item in itemsList)
                {
                    OrderHeaderDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool InsertOrderItemFromSync()
        {
            List<order_item> itemsList = OrderItemBkd.GetBackendItems("order_item");
            try
            {
                foreach (var item in itemsList)
                {
                    OrderItemDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool InsertOrderDetailFromSync()
        {
            List<order_detail> itemsList = OrderDetailBkd.GetBackendItems("order_detail");
            try
            {
                foreach (var item in itemsList)
                {
                    OrderDetailDbF.InsertItem(item);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
