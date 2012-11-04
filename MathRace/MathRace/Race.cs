namespace MathRace
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR.Hubs;

    public class Race : Hub
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
            if (operation == this.raceManager.Operation.Solution)
            {
                return Task.Factory.StartNew(() =>
                                          {
                                              // result_operation: 1:you win, 2:other player won, 0: bad operation
                                              // msg to rest of players. someone else won!
                                              this.Clients.AllExcept(Context.ConnectionId).resultOperation(2);

                                              // msg to winner
                                              this.Clients.Caller.resultOperation(1);

                                              // avoid long names
                                              var safeName = name.Length > 20 ? name.Substring(0, 20) : name;

                                              raceManager.AddWinnerToScores(safeName);

                                              // broacast scores
                                              this.Clients.All.scores(raceManager.Scores);

                                              // new challenge
                                              raceManager.CreteNewOperation();

                                              // new challenge for all players
                                              this.Clients.All.newOperation(raceManager.Operation.Quest);
                                          });
            }
            else
            {
                return this.Clients.Caller.resultOperation(0);
            }
        }

        public override Task OnConnected()
        {
            return Task.WhenAll(
                this.Clients.Caller.history(this.raceManager.History),
                this.Clients.Caller.hallOfFame(this.raceManager.HallOfFame),
                this.Clients.Caller.newOperation(this.raceManager.Operation.Quest));
        }

        public Task Connect()
        {
            return Task.Factory.StartNew(() => { });
        }
    }
}