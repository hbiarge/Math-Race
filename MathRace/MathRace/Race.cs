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
        public void SolveOperation(int operation, string name)
        {
            if (operation == RaceManager.Operation.Solution)
            {
                //result_operation: 1:you win, 2:other player won, 0: bad operation
                this.Clients.resultOperation(2); //msg to rest of players. someone else won!
                this.Caller.resultOperation(1); //msg to winner

                var safeName = name.Length > 20 ? name.Substring(0, 20) : name; //avoid long names
                RaceManager.AddWinnerToScores(safeName);
                this.Clients.scores(RaceManager.Scores); //broacast scores

                //new challenge
                RaceManager.CreteNewOperation();
                this.Clients.newOperation(RaceManager.Operation.Quest); //new challenge for all players
            }
            else
            {
                this.Clients.resultOperation(0);
            }
        }

        public Task Connect()
        {
            return Task.WhenAll(
                this.Clients.history(RaceManager.History),
                this.Clients.hallOfFame(RaceManager.HallOfFame),
                this.Clients.newOperation(RaceManager.Operation.Quest)
            );
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(() => { });
        }
    }
}