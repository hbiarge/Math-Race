using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using SignalR;
using SignalR.Hubs;

namespace MathRace
{
    public static class RaceManager
    {
        private static Timer timer;
        private static IHubContext hub;
        private static DateTime gameStarted;


        public static void Start()
        {
            gameStarted = DateTime.Now;
            hub = GlobalHost.ConnectionManager.GetHubContext<Race>();

            timer = new Timer(o =>
                                  {
                                      var elapsed = DateTime.Now.Subtract(gameStarted);
                                      var remaining = 30 - (int)elapsed.TotalSeconds;

                                      if (remaining < 0)
                                      {
                                          gameStarted = DateTime.Now;
                                          hub.Clients.newGame();
                                      }
                                      else
                                      {
                                          hub.Clients.time(remaining);
                                      }

                                  }, null, 0, 1000);
        }
    }
}