using SPA_Datahandler.SyncServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA_Datahandler.Sync
{
    class SyncSettings
    {
        public SpaServClient SyncClient { get; set; }
        public SyncSettings()
        {
            SyncClient = new SpaServClient("BasicHttpBinding_ISpaServ");
            InitSync();
        }


        public void InitSync()
        {
            string tst = SyncClient.TestString();

            SpaUser user = SyncClient.Logon("franz", "Start123");
            SyncClient.InitSync(false, "26.12.2017 13:08:00");

            var temp = SyncClient.GetOrderDetail();

            string tmp = "asdf";
        }

    }
}
