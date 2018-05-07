using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler
{
    public class SimpleDatabaseFunctions<T> : Dataprovider where T : class
    {
        public bool ClearTable()
        {
            try
            {
                foreach (T item in QueryAll())
                {
                    dbContext.Set<T>().Remove(item);
                    //dbContext.T.Remove(item);
                    dbContext.SaveChanges();
                }
            }
            catch(Exception e)
            {
                return false;
            }
            
            return true;

        }

        public bool InsertItem(T item)
        {
            try
            {
                dbContext.Set<T>().Add(item);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                dbContext.Set<T>().Remove(item);
                return false;
            }
            return true;

        }

        public List<T> QueryAll()
        {
            var query = (from p in dbContext.Set<T>().Cast<T>() select p);

            List<T> ReturnList = new List<T>();
            foreach (T item in query.ToList())
            {
                ReturnList.Add(item);
            }
            return ReturnList;
        }

    }
}
