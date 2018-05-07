using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace SPA_Datahandler.BackendDatabaseHandler
{
    
    class BackendDatabaseConnection
    {
        private OracleConnectionStringBuilder sb;
        private OracleConnection conn;

        public BackendDatabaseConnection()
        {
            sb = new OracleConnectionStringBuilder();

            sb.DataSource = "wi-gate.technikum-wien.at:60730";
            //sb.UserID = "pu_freischlager";              
            //sb.Password = "start123";
            //sb.ProxyUserId = "so_datenbasis";
            //sb.ProxyPassword = "fst4";

            sb.UserID = "so_datenbasis";
            sb.Password = "fst4";
           
        }

        public void Open()
        {
            conn = new OracleConnection(sb.ToString());
            conn.Open();
        }

        public void Close()
        {
            conn.Close();
        }

        public OracleDataReader Read(string Tablename)
        {
            OracleCommand command = new OracleCommand("select * from " + Tablename);
            command.Connection = conn;
            return command.ExecuteReader();
        }
        

    }
}
