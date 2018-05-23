using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;


namespace SPA_Datahandler.BackendDatabaseHandler
{
    public class BackendDatabaseHandling<T> where T : class, new()
    {
        private BackendDatabaseConnection BeDb;
        public BackendDatabaseHandling()
        {
            BeDb = new BackendDatabaseConnection();
            BeDb.Open();
        }


        public List<T> GetBackendItems(string tablename)
        {
            List<T> ReturnItems = new List<T>();
            foreach (DbDataRecord DataRow in BeDb.Read(tablename))
            {
                T Item = new T();
                int cnt = 0;
                foreach (PropertyInfo propertyInfo in Item.GetType().GetProperties())
                {
                    if (cnt< DataRow.FieldCount)
                    {                 
                        if (!(DataRow[propertyInfo.Name] is DBNull))
                        {
                            Type t;
                            if (propertyInfo.Equals(typeof(Nullable<System.DateTime>)))         //If Column is Nullable, the underlying Type needs to be determined
                            {
                                t = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                            }
                            else if (propertyInfo.PropertyType.Equals(typeof(double?)))
                            {
                                t = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                            }
                            else
                            {
                                t = propertyInfo.PropertyType;
                            }
                            propertyInfo.SetValue(Item, Convert.ChangeType(DataRow[propertyInfo.Name], t) );
                        }
                    }
                    //else
                    //{
                    //    Type t;
                    //    t = propertyInfo.PropertyType;
                    //    propertyInfo.SetValue(Item, t.GetTypeInfo().AsType());
                    //}
                    cnt++;
                }
                ReturnItems.Add(Item);
            }
            return ReturnItems;
        }

        //public List<zone> GetBackendZones()
        //{
        //    List<zone> ReturnZones = new List<zone>();
        //    foreach (DbDataRecord Item in BeDb.Read("zone"))
        //    {
        //        zone zne = new zone();
        //        int cnt = 0;
        //        foreach (PropertyInfo propertyInfo in zne.GetType().GetProperties())
        //        {
        //            if (cnt < Item.FieldCount)
        //            {
        //                if (!(Item[propertyInfo.Name] is DBNull))
        //                {
        //                    Type t;
        //                    if (propertyInfo.Equals(typeof(Nullable<System.DateTime>)))         //If Column is Nullable, the underlying Type needs to be determined
        //                    {
        //                        t = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
        //                    }
        //                    else
        //                    {
        //                        t = propertyInfo.PropertyType;
        //                    }
        //                    propertyInfo.SetValue(zne, Convert.ChangeType(Item[propertyInfo.Name], t));
        //                }
        //            }
        //            cnt++;
        //        }
        //        ReturnZones.Add(zne);
        //    }
        //    return ReturnZones;
        //}

    }
}
