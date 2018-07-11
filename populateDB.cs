using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Text.RegularExpressions;
using System.Transactions;
using MathNet;
using MathNet.Numerics.Distributions;
using MathNet.Numerics;
using SoccerDB;

namespace SoccerDB
{
    class populateDB
    {
        public double StringCompare(string str1, string str2)
        {
            List<string> pairs1 = WordLetterPairs(str1.ToUpper());
            List<string> pairs2 = WordLetterPairs(str2.ToUpper());

            int intersection = 0;
            int union = pairs1.Count + pairs2.Count;

            for (int i = 0; i < pairs1.Count; i++)
            {
                for (int j = 0; j < pairs2.Count; j++)
                {
                    if (pairs1[i] == pairs2[j])
                    {
                        intersection++;
                        pairs2.RemoveAt(j);//Must remove the match to prevent "GGGG" from appearing to match "GG" with 100% success

                        break;
                    }
                }
            }

            return (2.0 * intersection) / union;
        }

        /// <summary>
        /// Gets all letter pairs for each
        /// individual word in the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<string> WordLetterPairs(string str)
        {
            List<string> AllPairs = new List<string>();

            // Tokenize the string and put the tokens/words into an array
            string[] Words = Regex.Split(str, @"\s");

            // For each word
            for (int w = 0; w < Words.Length; w++)
            {
                if (!string.IsNullOrEmpty(Words[w]))
                {
                    // Find the pairs of characters
                    String[] PairsInWord = LetterPairs(Words[w]);

                    for (int p = 0; p < PairsInWord.Length; p++)
                    {
                        AllPairs.Add(PairsInWord[p]);
                    }
                }
            }

            return AllPairs;
        }

        /// <summary>
        /// Generates an array containing every 
        /// two consecutive letters in the input string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string[] LetterPairs(string str)
        {
            int numPairs = str.Length - 1;

            string[] pairs = new string[numPairs];

            for (int i = 0; i < numPairs; i++)
            {
                pairs[i] = str.Substring(i, 2);
            }

            return pairs;
        }

        public void readCSV(string path, string country, string league, int tipo)
        {
            //league += " 17/18";

            var games = new List<GamesCSV>();

            using (var parser = new TextFieldParser(path))
            {
                parser.CommentTokens = new[] { "#" };
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = false;

                parser.ReadLine(); // Skip first line

                while (!parser.EndOfData)
                {
                    var a = parser.ReadFields();

                    games.Add(new GamesCSV(a));
                }
            }

            // TODO: We have to make sure the country already exists in the DB
            //var teams = games.Select(p => p.HomeTeam).Distinct().Select(t => new Team {name = t, country = "Itália"});
            // Vai buscar as equipas ao ficheiro CSV Italiano
            var teams = games.Select(p => p.HomeTeam).Distinct();

            var db = new soccerDBDataContext();

            var teamdb = db.Teams.Where(c => c.Country_name == country).Select(n => n.Name); // I only want team names

            var comp = db.Competitions.FirstOrDefault(c => c.Name == league); //TODO: Não preciso de estar sempre aqui Uma vez que a 

            // Todo: Make a new algorithm for the team names (see if it already exists) SOLUTION: ADD ALTERNATIVE NAMES
            foreach (var t in teams) // Iterate each team on .csv
            {
                var similarity = 0.0;
                string f = null;

                if (teamdb.Contains(t))
                {
                    var team = db.Teams.FirstOrDefault(c => c.Country_name == country && c.Name == t);

                    if (team != null/* && team.compName == null*/)
                    {
                        if (comp == null)
                        {
                            // TODO: Não existe a competição tenho de a criar..

                            var x = new Competition()
                            {
                                Name = league,
                                //TODO: Adicionar os países e o nível da liga (1,2,3,4,5)..
                            };

                            comp = x;
                        }

                        team.Competition1 = comp;

                        db.SubmitChanges();
                    }
                }

                else
                {
                    var te = new Team
                    {
                        Name = t,
                        Country_name = country,
                        compName = league
                    };

                    db.Teams.InsertOnSubmit(te); // TODO: SEE IF THE TEAM ALREADY EXISTS IN DB PROPERLY
                    //db.SubmitChanges();
                }

                /*foreach (var s in teamdb) // Iterate each team on DB
                {
                    var sim = StringCompare(s, t);
                    if (sim > similarity)
                    {
                        similarity = sim;
                        f = s;
                    }
                }

                if (f != null && similarity > 0.7 && similarity != 1) // Let's put similarity at 43%
                {
                    foreach (var d in games)
                    {
                        if (d.HomeTeam == t)
                        {
                            d.HomeTeam = f;
                        }
                        else if (d.AwayTeam == t)
                        {
                            d.AwayTeam = f;
                        }
                    }
                }
                else if (similarity <= 0.8)
                {
                    if (!db.Competitions.Any(c => c.Name == league)) // The league doesn't exist
                    {
                        var comp = new Competition()
                        {
                            Name = league,
                            Country_name = country,
                            Type = tipo
                        };

                        db.Competitions.InsertOnSubmit(comp);

                        db.SubmitChanges();
                    }

                    var te = new Team
                    {
                        Name = t,
                        Country_name = country,
                        compName = league
                    };

                    db.Teams.InsertOnSubmit(te);

                    //teamsToAdd.Add(new Team() { Name = t, Country_name = country, Competition = new Competition() {Name = league} }); // Adds to the list to add
                }*/
                db.SubmitChanges();
            }

            var gamesOnDB = db.Matches.Where(c => c.Competition_name == league).ToList();

            for (var j = 0; j < games.Count(); j++) // TODO: Check if match is already on the database
            {
                var game = new Match
                {
                    Home_team = games[j].HomeTeam,
                    Away_team = games[j].AwayTeam,
                    away_goals = games[j].AwayScore,
                    home_goals = games[j].HomeScore,
                    date = games[j].Date,
                    final_result = games[j].FinalScore,
                    Competition_name = league//,
                    //Team = db.Teams.Single(c => c.Country_name == country && c.Name == games[j].HomeTeam),
                    //Team1 = db.Teams.Single(c => c.Country_name == country && c.Name == games[j].AwayTeam)
                };

                if (GamesExistsDb(game, gamesOnDB)) continue; // TODO: Caution! I've added teams from different leagues in a match!

                db.Matches.InsertOnSubmit(game); // Bug: Don't commit outside! If you commit outside it will bug the SubmitChanges() submitting already stuff on DB!
                db.SubmitChanges();
            }

            db.Dispose();

            MessageBox.Show("Added games!");
        }

        public void testTheta(string league, string startDate, string endDate, double theta)
        {
            var calc = new Calculator();
            var db = new soccerDBDataContext();
            int numTeams = db.Teams.Count(x => x.Competition1.Name == league);
            int numJogos = numTeams / 2;
            int numJornadas = 6;

            int aw = 0;
            int hw = 0;
            int under = 0;
            int over = 0;

            int i = numJornadas - 1;
            //var vals = new Dictionary<double, int>();

            //for (int i = 0; i < 5; i++) // 5 jornadas
            //{
                var matches = db.Matches.Where(x => x.Competition_name == league).OrderByDescending(x => x.date).Take((i + 1) * numJogos);

                foreach (var match in matches)
                {
                    var d = calc.CalculateGoalExpectancy(match.Home_team, match.Away_team, league, startDate,
                        Convert.ToString(match.date, null).Split()[0]);

                    if (d.homeGoalExpectancy <= 0.2)
                        d.homeGoalExpectancy = 0.5;

                    if (d.awayGoalExpectancy <= 0.2)
                        d.awayGoalExpectancy = 0.5;

                    //var p = calc.ReturnPoissonProbScores(d.homeGoalExpectancy, d.awayGoalExpectancy); // 40, 57
                    var p = calc.MaxWell(d.homeGoalExpectancy, d.awayGoalExpectancy, theta); // 246, 316
                    var maxwell =
                    (new Calculator.ProbabilityOdds()
                    {
                        Away = p.Away,
                        awayGoalExpectancy = d.awayGoalExpectancy,
                        Draw = p.Draw,
                        Home = p.Home,
                        homeGoalExpectancy = d.homeGoalExpectancy,
                        Over25 = p.Over25,
                        Under25 = p.Under25,
                        ThetaMaxWell = theta,
                        numJornadas = 5,
                        over_under = 0,
                        cont_win_lose = 0
                    });

                    if (p.Home > 40 && match.final_result == 'H')
                        hw++;

                    if (p.Away > 40 && match.final_result == 'A')
                        aw++;

                    if (p.Under25 > 50 && match.away_goals + match.home_goals < 2.5)
                        under++;

                    if (p.Over25 > 50 && match.away_goals + match.home_goals > 2.5)
                        over++;
                //}
            }

            MessageBox.Show(numJogos * numJornadas + "\n\nHome Win: " + hw + "\nAWAY WIN: " + aw + "\nUNDER: " + under + "\nOVER: " + over);
        }

        public void populate2(string league, string startDate, string endDate) // TODO: Today date
        {
            var calc = new Calculator();
            var db = new soccerDBDataContext();

            int best1 = 0;
            int best2 = 0;
            double final = 0.0;
            int cont_win_lose = 0;
            int over_under = 0;
            int cont = 0;

            int numTeams = 18;
            int numJogos = numTeams / 2;

            var xasd = new Dictionary<Match, List<Calculator.ProbabilityOdds>>();

            for (int i = 0; i < 5; i++) // 5 jornadas
            {
                var matches = db.Matches.Where(x => x.Competition_name == league).OrderByDescending(x => x.date).Take((i+1) * numJogos);
                //var poisson = new List<Calculator.ProbabilityOdds>();

                // best = 1.13 com 90 jogos (2ª LIGA)
                //best = 1.1 com 99 jogos (1ª LIGA) [43, 72] / 99 || POISSON COM 90 JOGOS CHEGA A 71%

                for (var theta = 0.3; theta < 0.7; theta += 0.1)
                {
                    foreach (var match in matches)
                    {
                        var d = calc.CalculateGoalExpectancy(match.Home_team, match.Away_team, league, startDate, Convert.ToString(match.date, null).Split()[0]);

                        if (d.homeGoalExpectancy <= 0.2)
                            d.homeGoalExpectancy = 0.5;

                        if (d.awayGoalExpectancy <= 0.2)
                            d.awayGoalExpectancy = 0.5;

                        //var p = calc.ReturnPoissonProbScores(d.homeGoalExpectancy, d.awayGoalExpectancy); // 40, 57
                        var p = calc.MaxWell(d.homeGoalExpectancy, d.awayGoalExpectancy, theta); // 246, 316
                        var maxwell =
                        (new Calculator.ProbabilityOdds()
                        {
                            Away = p.Away,
                            awayGoalExpectancy = d.awayGoalExpectancy,
                            Draw = p.Draw,
                            Home = p.Home,
                            homeGoalExpectancy = d.homeGoalExpectancy,
                            Over25 = p.Over25,
                            Under25 = p.Under25,
                            ThetaMaxWell = theta,
                            numJornadas = i + 1,
                            over_under = 0,
                            cont_win_lose = 0
                        });

                        if (xasd.ContainsKey(match)) // A partida já existe com outras odds
                            xasd[match].Add(maxwell);

                        else // Ainda não foi calculado.. Inserir o 1º elemento
                            xasd.Add(match, new List<Calculator.ProbabilityOdds> { maxwell });
                    }
                }
            }

            foreach (var match in xasd.Keys) // Vou ver em quantos jogos acertei..
            {
                var odds = xasd[match];

                foreach (var p in odds)
                {
                    if (p.Home > 40 && match.final_result == 'H') // TODO: Ter atenção a distribuição de probabilidades..
                        p.cont_win_lose++;

                    if (p.Away > 40 && match.final_result == 'A')
                        p.cont_win_lose++;

                    if (p.Under25 > 50 && match.away_goals + match.home_goals < 2.5)
                        p.over_under++;

                    if (p.Over25 > 50 && match.away_goals + match.home_goals > 2.5)
                        p.over_under++;
                }
            }

            foreach (var match in xasd.Keys)
            {
                var odds = xasd[match];

                var group = odds.GroupBy(x => x.numJornadas).ToList();

                foreach (var t in group)
                {
                    var f = t.OrderByDescending(x => x.cont_win_lose + x.over_under).Take(1).Select(x => x.ThetaMaxWell);

                    double bart = f.First();

                    var d = odds.Where(p => p.ThetaMaxWell == bart && p.cont_win_lose + p.over_under >= 1);
                }
            }

        }

        public void populate(string home, string away, string league, string startDate, string endDate, string _maxwell) // TODO: Today date
        {
            var calc = new Calculator();
            var x = calc.CalculateGoalExpectancy(home, away, league, startDate, endDate);
            var scores = calc.ReturnPoissonProbScores(x.homeGoalExpectancy, x.awayGoalExpectancy);

            double maxwel = 1.4;

            if (league == "Primeira Liga")
                maxwel = 1.1;
            else if (league == "Primeira Liga 17/18")
                maxwel = 1.2;
            else if (league == "Segunda Liga 16/17")
                maxwel = 1.13;
            else if (league == "Serie A" || league == "Championship")
                maxwel = 0.9;
            else if (league == "Serie B")
                maxwel = 0.75;

            if (!string.IsNullOrWhiteSpace(_maxwell))
                maxwel = Convert.ToDouble(_maxwell);

            var maxwell = calc.MaxWell(x.homeGoalExpectancy, x.awayGoalExpectancy, maxwel);

            MessageBox.Show("--POISSON--\nHOME: " + scores.Home + "\nAWAY: " + scores.Away + "\nDRAW: " + scores.Draw + "\nUNDER: " + scores.Under25 + "\nOVER:" + scores.Over25 + "\n\n" +
                            "--MAXWELL--\nHOME: " + maxwell.Home + "\nAWAY: " + maxwell.Away + "\nDRAW: " + maxwell.Draw + "\nUNDER:" + maxwell.Under25 +"\nOVER:" + maxwell.Over25);
        }

        public void addTeams(List<Team> teams)
        {
            var db = new soccerDBDataContext();

            var teamdb = (from t in db.Teams select t); // Todos os resultados da db

            var countries = (from t in db.Countries select t); // Gets the countries from db

            var countriesToAdd = new List<string>();
            var teamsToAdd = new List<Team>();

            foreach (var t in teams)
            {
                if (!teamdb.Any(p => p.Name == t.Name)) // Doesn't exist on DB
                {
                    teamsToAdd.Add(t); // Adds to the list to add
                    countriesToAdd.Add(t.Country_name); // TODO: If the team already exists most likely the country exists aswell
                }
            }

            var _countries = countriesToAdd.Distinct().Select(s => new Country() {Name = s}).ToList().Where(c => !countries.Any(p => p.Name == c.Name)).ToList();

            db.Teams.InsertAllOnSubmit(teamsToAdd);
            db.Countries.InsertAllOnSubmit(_countries);
            db.SubmitChanges();
            MessageBox.Show("Added " + teamsToAdd.Count + " teams\n" + _countries.Count + " countries to the DB from placard!");
        }

        private bool GamesExistsDb(Match m, IEnumerable<Match> l)
        {
            // DONT NEED TO MAKE A DB CALL EACH TIME SINCE I KNOW ALREADY THE LEAGUE I'M SEARCHING FOR
            /*var db = new soccerDBDataContext();

            return db.Matches.Any(c => c.Competition_name == m.Competition_name && c.Home_team == m.Home_team && c.Away_team == m.Away_team);*/

            return
                l.Any(
                    c =>
                        c.Competition_name == m.Competition_name && c.Home_team == m.Home_team &&
                        c.Away_team == m.Away_team);
        }
    }
}
