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
            throw new NotImplementedException();
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            throw new NotImplementedException();
        }
    }
}