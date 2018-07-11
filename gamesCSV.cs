using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerDB
{
    class GamesCSV
    {
        public GamesCSV(IReadOnlyList<string> vars)
        {
            League = vars[0];
            Date = DateTime.ParseExact(vars[1], "d/MM/yy", null); // TODO: Warning wrong data was being parsed
            HomeTeam = vars[2];
            AwayTeam = vars[3];
            HomeScore = Convert.ToInt32(vars[4]);
            AwayScore = Convert.ToInt32(vars[5]);
            FinalScore = vars[6][0];
        }

        public string League { get; }
        public DateTime? Date { get; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; }
        public int AwayScore { get; }
        public char FinalScore { get; }
    }

    class TeamCSV
    {
        public string Name;
        public int homeGoals;
        public int awayGoals;
    }
}
