using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using MathRace.Model;
using SignalR;
using SignalR.Hubs;

namespace MathRace
{
    public static class RaceManager
    {
        private static Timer timer;
        private static IHubContext hub;
        private static DateTime gameStarted;
        private static Dictionary<string, int> scores;

        public static Operation Operation { get; private set; }

        public static List<PlayerScore> Scores
        {
            get
            {
                return scores
                    .OrderByDescending(x => x.Value)
                    .Select(x => new PlayerScore { Player = x.Key, Score = x.Value })
                    .ToList();
            }
        }

        public static List<RaceHistory> History { get; private set; }
        public static List<HallOfFamePlayerScore> HallOfFame { get; private set; }

        public static void Start()
        {
            hub = GlobalHost.ConnectionManager.GetHubContext<Race>();
            Operation = Operation.Create();
            scores = new Dictionary<string, int>();
            History = new List<RaceHistory>();
            HallOfFame = new List<HallOfFamePlayerScore>();
            gameStarted = DateTime.Now;

            timer = new Timer(o =>
                                  {
                                      var elapsed = DateTime.Now.Subtract(gameStarted);
                                      var remaining = 30 - (int)elapsed.TotalSeconds;

                                      if (remaining < 0)
                                      {
                                          if (scores.Count > 0)
                                          {
                                              AddScoresToHistory();
                                              UpdateHallOfFame();

                                              hub.Clients.history(History);
                                              hub.Clients.hallOfFame(HallOfFame);
                                          }
                                          
                                          scores.Clear();
                                          gameStarted = DateTime.Now;
                                          hub.Clients.scores(Scores);
                                          hub.Clients.newGame();
                                      }
                                      else
                                      {
                                          hub.Clients.time(remaining);
                                      }

                                  }, null, 0, 1000);
        }

        public static void CreteNewOperation()
        {
            Operation = Operation.Create();
        }

        public static void AddWinnerToScores(string winnerName)
        {
            if (scores.ContainsKey(winnerName))
            {
                scores[winnerName] += 1;
            }
            else
            {
                scores.Add(winnerName, 1);
            }
        }

        private static void AddScoresToHistory()
        {
            History.Insert(0, new RaceHistory
                                  {
                                      Timestamp = gameStarted,
                                      Scores = Scores
                                  });

            if (History.Count > 20)
            {
                History.RemoveAt(History.Count - 1);
            }
        }

        private static void UpdateHallOfFame()
        {
            HallOfFame.AddRange(Scores
                .Select(x => new HallOfFamePlayerScore
                               {
                                   Timestamp = gameStarted,
                                   Player = x.Player,
                                   Score = x.Score
                               }));

            HallOfFame = HallOfFame
                .OrderByDescending(x => x.Score)
                .Take(20)
                .ToList();
        }
    }
}