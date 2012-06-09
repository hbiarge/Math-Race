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
        private readonly RaceManager raceManager;

        public Race(RaceManager raceManager)
        {
            if (raceManager == null)
            {
                throw new ArgumentNullException("raceManager");
            }

            this.raceManager = raceManager;
        }

        public Task SolveOperation(int operation, string name)
        {
            if (operation == raceManager.Operation.Solution)
            {
                return Task.Factory.StartNew(() =>
                                          {
                                              //result_operation: 1:you win, 2:other player won, 0: bad operation
                                              this.Clients.resultOperation(2);
                                              //msg to rest of players. someone else won!
                                              this.Caller.resultOperation(1); //msg to winner

                                              var safeName = name.Length > 20 ? name.Substring(0, 20) : name;
                                              //avoid long names
                                              raceManager.AddWinnerToScores(safeName);
                                              this.Clients.scores(raceManager.Scores); //broacast scores

                                              //new challenge
                                              raceManager.CreteNewOperation();
                                              this.Clients.newOperation(raceManager.Operation.Quest);
                                              //new challenge for all players
                                          });
            }
            else
            {
                return this.Clients.resultOperation(0);
            }
        }

        public Task Connect()
        {
            return Task.WhenAll(
                this.Clients.history(raceManager.History),
                this.Clients.hallOfFame(raceManager.HallOfFame),
                this.Clients.newOperation(raceManager.Operation.Quest)
            );
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(() => { });
        }
    }
}