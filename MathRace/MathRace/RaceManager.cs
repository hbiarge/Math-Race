namespace MathRace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using MathRace.Model;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    public class RaceManager
    {
        private readonly IHubContext hub;

        private readonly Dictionary<string, int> scores;

        private Timer timer;

        private DateTime gameStarted;

        public RaceManager()
        {
            // Obtenemos la instancia del hub
            this.hub = GlobalHost.ConnectionManager.GetHubContext<Race>();

            // Inicializamos las estructuras que almacenan la info del juego
            this.scores = new Dictionary<string, int>();
            this.History = new List<RaceHistory>();
            this.HallOfFame = new List<HallOfFamePlayerScore>();

            // Creamos una nueva operación y establecemos la fecha de inicio del juego
            Operation = Operation.Create();
            this.gameStarted = DateTime.Now;

            this.timer = new Timer(o =>
            {
                var elapsed = DateTime.Now.Subtract(gameStarted);
                var remaining = 30 - (int)elapsed.TotalSeconds;

                if (remaining < 0)
                {
                    if (scores.Count > 0)
                    {
                        AddScoresToHistory();
                        UpdateHallOfFame();

                        hub.Clients.All.history(History);
                        hub.Clients.All.hallOfFame(HallOfFame);
                    }

                    scores.Clear();
                    gameStarted = DateTime.Now;
                    hub.Clients.All.scores(Scores);
                    hub.Clients.All.newGame();
                }
                else
                {
                    hub.Clients.All.time(remaining);
                }
            },
            null,
            0,
            1000);
        }

        public Operation Operation { get; private set; }

        public List<PlayerScore> Scores
        {
            get
            {
                return this.scores
                    .OrderByDescending(x => x.Value)
                    .Select(x => new PlayerScore { Player = x.Key, Score = x.Value })
                    .ToList();
            }
        }

        public List<RaceHistory> History { get; private set; }

        public List<HallOfFamePlayerScore> HallOfFame { get; private set; }

        public void CreteNewOperation()
        {
            Operation = Operation.Create();
        }

        public void AddWinnerToScores(string winnerName)
        {
            if (this.scores.ContainsKey(winnerName))
            {
                this.scores[winnerName] += 1;
            }
            else
            {
                this.scores.Add(winnerName, 1);
            }
        }

        private void AddScoresToHistory()
        {
            this.History.Insert(
                0,
                new RaceHistory
                {
                    Timestamp = this.gameStarted,
                    Scores = this.Scores
                });

            if (this.History.Count > 20)
            {
                this.History.RemoveAt(this.History.Count - 1);
            }
        }

        private void UpdateHallOfFame()
        {
            this.HallOfFame.AddRange(this.Scores
                .Select(x => new HallOfFamePlayerScore
                               {
                                   Timestamp = this.gameStarted,
                                   Player = x.Player,
                                   Score = x.Score
                               }));

            this.HallOfFame = this.HallOfFame
                .OrderByDescending(x => x.Score)
                .Take(20)
                .ToList();
        }
    }
}