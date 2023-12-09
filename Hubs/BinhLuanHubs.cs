using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace QUIZ_IT.Hubs
{
    [HubName("chat")]
    public class BinhLuanHubs : Hub
    {
        public void Hello ()
        {
            Clients.All.hello();
        }

        public void Message (int CauHoiId, int BinhLuanId)
        {
            Clients.All.message(CauHoiId, BinhLuanId);
        }
    }
}