using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SignalR.Hubs;

namespace MathRace
{
    public class Race : Hub, IConnected
    {
        public Task Connect()
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(() => { });
        }
    }
}