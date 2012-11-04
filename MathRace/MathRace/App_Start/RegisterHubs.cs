using System.Web;

using MathRace.App_Start;

[assembly: PreApplicationStartMethod(typeof(RegisterHubs), "Start")]

namespace MathRace.App_Start
{
    using System.Web.Routing;

    using Microsoft.AspNet.SignalR;

    public static class RegisterHubs
    {
        public static void Start()
        {
            // Register the default hubs route: ~/signalr/hubs
            RouteTable.Routes.MapHubs();            
        }
    }
}
