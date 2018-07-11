using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet;
using MathNet.Numerics.Distributions;
using MathNet.Numerics;
using System.Data;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace SoccerDB
{
    class Calculator
    {
        private const int MAX_GOALS = 10;

        internal class ProbabilityOdds
        {
            public int over_under { get; set; }
            public int cont_win_lose { get; set; }
            public int numJornadas { get; set; }
            public double ThetaMaxWell { get; set; }
            public double Home { get; set; }
            public double Away { get; set; }
            public double Draw { get; set; }
            public double Under25 { get; set; }
            public double Over25 { get; set; }

            public double? homeGoalExpectancy { get; set; }
            public double? awayGoalExpectancy { get; set; }
        }

        public bool Robinson_Over25(string home, string away, string league)
        {
            var db = new soccerDBDataContext();

            var home_games = db.Matches.Where(x => x.Home_team == home && x.Competition_name == league).OrderByDescending(x => x.date).Take(3);

            var cont1 = 0;
            var cont2 = 0;
            foreach (var game in home_games)
            {
                cont1 += (int)game.home_goals;

                if (game.home_goals + game.away_goals > 2)
                    cont2++;
            }

            if (cont1 >= 7 && cont2 >= 2)
            {
                var away_games = db.Matches.Where(x => x.Away_team == away && x.Competition_name == league).OrderByDescending(x => x.date).Take(3);

                cont1 = 0; //>=7
                cont2 = 0; //>=2
                var cont3 = 0; // >=2
                var cont4 = 0; // >= 2

                int i = 0;
                foreach (var game in away_games)
                {
                    if (i == 0) // Its the last game
                    {
                        cont4 = (int)game.home_goals + (int)game.away_goals;
                        i++;
                    }

                    cont1 += (int) game.away_goals;

                    if (game.home_goals + game.away_goals > 2)
                        cont2++;

                    if (game.away_goals > 0)
                        cont3++;
                }

                if (cont1 >= 7 && cont2 >= 2 && cont3 >= 2 && cont4 >= 2)
                    return true;
            }

            return false;
        }

        public bool Robinson_Under25(string home, string away, string league)
        {
            var db = new soccerDBDataContext();

            var home_games = db.Matches.Where(x => x.Home_team == home && x.Competition_name == league).OrderByDescending(x => x.date).Take(3);

            int cont1 = 0;
            int cont2 = 0;
            foreach (var game in home_games)
            {
                if (game.home_goals + game.away_goals < 3)
                    cont1++;

                if (game.home_goals == 0 || game.away_goals == 0)
                    cont2++;
            }

            if (cont1 >= 2 && cont2 >= 1)
            {
                var away_games = db.Matches.Where(x => x.Away_team == away && x.Competition_name == league).OrderByDescending(x => x.date).Take(3);

                cont1 = 0;
                cont2 = 0;
                foreach (var game in away_games)
                {
                    if (game.home_goals + game.away_goals < 3)
                        cont1++;

                    if (game.away_goals == 0)
                        cont2++;
                }

                if (cont1 >= 2 && cont2 >= 1)
                    return true;
            }


            return false;
        }

        public ProbabilityOdds MaxWell(double? home, double? away, double theta)
        {
            var pp = new ConwayMaxwellPoisson((double)home, theta);
            var aa = new ConwayMaxwellPoisson((double)away, theta);

            var scores = new double[MAX_GOALS, MAX_GOALS];
            var scores2 = new double[MAX_GOALS, MAX_GOALS];

            // Prever só até 10 golos (0-0, 0-1, .., 0-9)
            for (var home_s = 0; home_s < MAX_GOALS; home_s++)
            {
                for (var away_s = 0; away_s < MAX_GOALS; away_s++)
                {
                    scores2[home_s, away_s] = pp.Probability(home_s) * aa.Probability(away_s) * 100; //Tem aqui o resultado em percentagem
                }
            }

            var under2 = CalculateGoalsUnderOverProbability(scores2, 2.5, "Under");
            var over2 = CalculateGoalsUnderOverProbability(scores2, 2.5, "Over");

            var _home = CalculateHomeWinProbability(scores2);
            var _draw = CalculateDrawProbability(scores2);
            var _away = CalculateAwayWinProbability(scores2);

            //MessageBox.Show("--MAXWELL--\nHOME: " + _home + "\nAWAY: " + _away + "\nDRAW: " + _draw + "\nUNDER:" + under2 +"\nOVER:" + over2);

            return new ProbabilityOdds { Home = _home, Away = _away, Draw = _draw, Under25 = under2, Over25 = over2 };
        }

        internal ProbabilityOdds ReturnPoissonProbScores(double? home, double? away)
        {
            var homeP = new Poisson((double)home);
            var awayP = new Poisson((double)away);

            var scores = new double[MAX_GOALS, MAX_GOALS];

            // Prever só até 10 golos (0-0, 0-1, .., 0-9)
            for (var home_s = 0; home_s < MAX_GOALS; home_s++)
            {
                for (var away_s = 0; away_s < MAX_GOALS; away_s++)
                {
                    scores[home_s, away_s] = homeP.Probability(home_s) * awayP.Probability(away_s) * 100; //Tem aqui o resultado em percentagem
                }
            }

            var _home = CalculateHomeWinProbability(scores);
            var _draw = CalculateDrawProbability(scores);
            var _away = CalculateAwayWinProbability(scores);

            var under = CalculateGoalsUnderOverProbability(scores, 2.5, "Under");
            var over = CalculateGoalsUnderOverProbability(scores, 2.5, "Over");

            //MessageBox.Show("--POISSON--\nHOME: " + _home + "\nAWAY: " + _away + "\nDRAW: " + _draw + "\nUNDER: " + under + "\nOVER:" + over);


            //MessageBox.Show("HOME=" + _home + "\nDRAW=" + _draw + "\nAWAY=" + _away + "\nUNDER=" + under + "\nOVER=" + over);

            return new ProbabilityOdds { Home = _home, Away = _away, Draw = _draw, Under25 = under, Over25 = over};
        }

        private static double CalculateGoalsUnderOverProbability(double[,] scores, double goals, string type)
        {
            double prob = 0;

            for (var i = 0; i < MAX_GOALS; i++)
            {
                for (var j = 0; j < MAX_GOALS; j++)
                {
                    switch (type)
                    {
                        case "Under":
                            if (i + j < goals)
                                prob += scores[i, j];
                            break;
                        case "Over":
                            if (i + j > goals)
                                prob += scores[i, j];
                            break;
                    }
                }
            }

            return prob;
        }

        public static double CalculateHomeWinProbability(double[,] scores)
        {
            return CalculateFinalResultProbability(scores, "HOME_WIN");
        }

        public static double CalculateAwayWinProbability(double[,] scores)
        {
            return CalculateFinalResultProbability(scores, "AWAY_WIN");
        }

        public static double CalculateDrawProbability(double[,] scores)
        {
            return CalculateFinalResultProbability(scores, "DRAW");
        }

        private static double CalculateFinalResultProbability(double [,] scores, string score)
        {
            var sum = 0.0;
            for (var i = 0; i < MAX_GOALS; i++)
            {
                for (var j = 0; j < MAX_GOALS; j++)
                {
                    switch (score)
                    {
                        case "DRAW":
                            if (i == j)
                                sum += scores[i, j];
                            break;
                        case "HOME_WIN":
                            if (i > j)
                                sum += scores[i, j];
                            break;
                        case "AWAY_WIN":
                            if (j > i)
                                sum += scores[i, j];
                            break;
                    }
                }
            }

            return sum;
        }

        public ProbabilityOdds CalculateGoalExpectancy(string home, string away, string league, string startDate, string endDate)
        {
            var db = new soccerDBDataContext();

            double? output = null;

            // TODO: Ver esta situação da data no store procedure da DB dd-mm-yyyy
            // TODO: Ver as equipas da nova liga que foram adicionadas.. Para não haver conflitos com épocas passadas
            var _avg = db.AVG_GOALS_GAME(league, endDate, startDate, ref output); // % total de golos por jogo (Marcados e sofridos)

            foreach (var s in _avg)
            {
                output = s.Column1;
            }

            double? away_favor = 0;
            double? away_against = 0;
            var avg_away = db.AVG_AWAY_TEAM(league, away, endDate, startDate); // % GOLOS MARCADOS / SOFRIDOS FORA (POR EQUIPA)

            foreach (var s in avg_away)
            {
                away_favor = s.__MARCADOS;
                away_against = s.__SOFRIDOS;
            }

            double? home_favor = 0;
            double? home_against = 0;
            var avg_home = db.AVG_HOME_TEAM(league, home, endDate, startDate); // % GOLOS MARCADOS / SOFRIDOS CASA (POR EQUIPA))

            foreach (var s in avg_home)
            {
                home_favor = s.__MARCADOS;
                home_against = s.__SOFRIDOS;
            }

            double? home_avg_favor = 0;
            double? home_avg_against = 0;
            var avg_home_league = db.AVG_HOME_GOALS(league, endDate, startDate);

            foreach (var s in avg_home_league)
            {
                home_avg_favor = s.Média_golos_marcados_casa;
                home_avg_against = s.Média_golos_sofridos_casa;
            }

            double? away_avg_favor = 0;
            double? away_avg_against = 0;
            var away_home_league = db.AVG_AWAY_GOALS(league, startDate, endDate);

            foreach (var s in away_home_league)
            {
                away_avg_favor = s.Média_golos_marcados_fora;
                away_avg_against = s.Média_golos_sofridos_fora;
            }

            /*MessageBox.Show("--HOME--\n\n" + home_favor + "\n" + home_against + "\n\n--AWAY--\n\n" + away_favor + "\n" +
                            away_against + "\n\n--LEAGUE HOME--\n\n" + home_avg_favor + "\n" + home_avg_against);*/

            var tye = db.POWER_HOME(league, home, endDate, startDate); // % total de golos por jogo (Marcados e sofridos)
            double? home_att = 0;
            double? home_def = 0;

            foreach (var x in tye)
            {
                home_att = x.PODER_ATAQUE;
                home_def = x.PODER_DEFENSIVO;
            }

            var rxs = db.POWER_AWAY(league, away, startDate, endDate);
            double? away_att = 0;
            double? away_def = 0;

            foreach (var x in rxs)
            {
                away_att = x.PODER_ATAQUE;
                away_def = x.PODER_DEFENSIVO;
            }

            // TODO: To prevent 0 theta in poisson (it means they are 1% better or worst then average)
            if (home_att == 0.0)
                home_att = 0.1;
            if (away_def == 0.0)
                away_def = 0.1;
            if (away_att == 0.0)
                away_att = 0.1;
            if (home_def == 0.0)
                home_def = 0.1;

            var homeTeamGoalExpectancy = home_att * away_def * home_avg_favor;
            var awayTeamGoalExpectancy = away_att * home_def * away_avg_favor;

            //MessageBox.Show("Calculated!\n" + homeTeamGoalExpectancy + "\n" + awayTeamGoalExpectancy);

            //CalculatePoissonProbScores(homeTeamGoalExpectancy, awayTeamGoalExpectancy);

            //var scores = ReturnPoissonProbScores(homeTeamGoalExpectancy, awayTeamGoalExpectancy);

            return new ProbabilityOdds() {homeGoalExpectancy = homeTeamGoalExpectancy, awayGoalExpectancy = awayTeamGoalExpectancy};
        }

        public void PoissonTheta(string [] home, string [] away)
        {
            Poisson5LastX1 = home[0];
            Poisson5LastX2 = home[1];
            Poisson5LastX3 = home[2];
            Poisson5LastX4 = home[3];
            Poisson5LastX5 = home[4];

            Poisson5LastY1 = away[0];
            Poisson5LastY2 = away[1];
            Poisson5LastY3 = away[2];
            Poisson5LastY4 = away[3];
            Poisson5LastY5 = away[4];

            PoisL5Kom = new decimal();
            PoisL5Kom = Poisson5LastX1 == "" ? PoisL5Kom : decimal.Add(PoisL5Kom, decimal.One);
            PoisL5Kom = Poisson5LastX2 == "" ? PoisL5Kom : decimal.Add(PoisL5Kom, decimal.One);
            PoisL5Kom = Poisson5LastX3 == "" ? PoisL5Kom : decimal.Add(PoisL5Kom, decimal.One);
            PoisL5Kom = Poisson5LastX4 == "" ? PoisL5Kom : decimal.Add(PoisL5Kom, decimal.One);
            PoisL5Kom = Poisson5LastX5 == "" ? PoisL5Kom : decimal.Add(PoisL5Kom, decimal.One);
            if (decimal.Compare(PoisL5Kom, decimal.Zero) == 0) return;
            PoissL5X1 = (string) Poisson5LastX1 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastX1);
            PoissL5X2 = Poisson5LastX2 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastX2);
            PoissL5X3 = Poisson5LastX3 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastX3);
            PoissL5X4 = Poisson5LastX4 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastX4);
            PoissL5X5 = Poisson5LastX5 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastX5);
            PoissL5Y1 = Poisson5LastY1 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastY1);
            PoissL5Y2 = Poisson5LastY2 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastY2);
            PoissL5Y3 = Poisson5LastY3 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastY3);
            PoissL5Y4 = Poisson5LastY4 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastY4);
            PoissL5Y5 = Poisson5LastY5 == "" ? new decimal() : Conversions.ToDecimal(Poisson5LastY5);
            PoissSumL5X = decimal.Divide(decimal.Add(decimal.Add(decimal.Add(decimal.Add(PoissL5X1, PoissL5X2), PoissL5X3), PoissL5X4), PoissL5X5), PoisL5Kom);
            PoissSumL5Y = decimal.Divide(decimal.Add(decimal.Add(decimal.Add(decimal.Add(PoissL5Y1, PoissL5Y2), PoissL5Y3), PoissL5Y4), PoissL5Y5), PoisL5Kom);
            PoissA1 = Poisson5LastX1 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5X1, PoissSumL5X)) + 1E-12);
            PoissA2 = Poisson5LastX2 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5X2, PoissSumL5X)) + 1E-12);
            PoissA3 = Poisson5LastX3 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5X3, PoissSumL5X)) + 1E-12);
            PoissA4 = Poisson5LastX4 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5X4, PoissSumL5X)) + 1E-12);
            PoissA5 = Poisson5LastX5 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5X5, PoissSumL5X)) + 1E-12);
            PoissB1 = Poisson5LastY1 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5Y1, PoissSumL5Y)) + 1E-12);
            PoissB2 = Poisson5LastY2 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5Y2, PoissSumL5Y)) + 1E-12);
            PoissB3 = Poisson5LastY3 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5Y3, PoissSumL5Y)) + 1E-12);
            PoissB4 = Poisson5LastY4 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5Y4, PoissSumL5Y)) + 1E-12);
            PoissB5 = Poisson5LastY5 == "" ? new decimal() : new decimal(Convert.ToDouble(decimal.Subtract(PoissL5Y5, PoissSumL5Y)) + 1E-12);
            ThetaPoisson = decimal.Compare(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Multiply(PoissA1, PoissB1), decimal.Multiply(PoissA2, PoissB2)), decimal.Multiply(PoissA3, PoissB3)), decimal.Multiply(PoissA4, PoissB4)), decimal.Multiply(PoissA5, PoissB5)), decimal.Zero) == 0 ? Conversions.ToString(0) : Strings.FormatNumber(Convert.ToDouble(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Divide(decimal.Multiply(PoissA1, PoissB1), decimal.Subtract(PoisL5Kom, decimal.One)), decimal.Divide(decimal.Multiply(PoissA2, PoissB2), decimal.Subtract(PoisL5Kom, decimal.One))), decimal.Divide(decimal.Multiply(PoissA3, PoissB3), decimal.Subtract(PoisL5Kom, decimal.One))), decimal.Divide(decimal.Multiply(PoissA4, PoissB4), decimal.Subtract(PoisL5Kom, decimal.One))), decimal.Divide(decimal.Multiply(PoissA5, PoissB5), decimal.Subtract(PoisL5Kom, decimal.One)))) / (Math.Pow(((((Math.Pow(Convert.ToDouble(PoissA1), 2.0) + Math.Pow(Convert.ToDouble(PoissA2), 2.0)) + Math.Pow(Convert.ToDouble(PoissA3), 2.0)) + Math.Pow(Convert.ToDouble(PoissA4), 2.0)) + Math.Pow(Convert.ToDouble(PoissA5), 2.0)) / Convert.ToDouble(decimal.Subtract(PoisL5Kom, decimal.One)), 0.5) * Math.Pow(((((Math.Pow(Convert.ToDouble(PoissB1), 2.0) + Math.Pow(Convert.ToDouble(PoissB2), 2.0)) + Math.Pow(Convert.ToDouble(PoissB3), 2.0)) + Math.Pow(Convert.ToDouble(PoissB4), 2.0)) + Math.Pow(Convert.ToDouble(PoissB5), 2.0)) / Convert.ToDouble(decimal.Subtract(PoisL5Kom, decimal.One)), 0.5)), 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
            BvPSumDifXY = Conversions.ToString(decimal.Subtract(PoissSumL5X, PoissSumL5Y));
            BvPSumAvG = Conversions.ToString(decimal.Add(PoissSumL5X, PoissSumL5Y));
            BvPAvgPoenaY = Conversions.ToString((double)(((Conversions.ToDouble(BvPSumAvG) * 10.0) - (Conversions.ToDouble(BvPSumDifXY) * 10.0)) / 20.0));
            BvPAvgPoenaX = Conversions.ToString((double)(((Conversions.ToDouble(BvPSumAvG) * 10.0) - (Conversions.ToDouble(BvPAvgPoenaY) * 10.0)) / 10.0));
            decimal num = Conversions.ToDecimal(ThetaPoisson);
            Home1 = Strings.FormatNumber((((Conversions.ToDouble(BvPAvgPoenaX) * Convert.ToDouble(Math.Abs(num))) + (Conversions.ToDouble(Home) * Convert.ToDouble(decimal.Subtract(decimal.One, Math.Abs(num))))) / 2.0) + (((Conversions.ToDouble(BvPAvgPoenaX) * 0.5) + (Conversions.ToDouble(Home) * 0.5)) / 2.0), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
            Away1 = Strings.FormatNumber((((Conversions.ToDouble(BvPAvgPoenaY) * Convert.ToDouble(Math.Abs(num))) + (Conversions.ToDouble(Away) * Convert.ToDouble(decimal.Subtract(decimal.One, Math.Abs(num))))) / 2.0) + (((Conversions.ToDouble(BvPAvgPoenaY) * 0.5) + (Conversions.ToDouble(Away) * 0.5)) / 2.0), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
            Home = Home1;
            Away = Away1;
            TextBoxSTD1 = Strings.FormatNumber(Math.Pow(((((Math.Pow(Convert.ToDouble(PoissA1), 2.0) + Math.Pow(Convert.ToDouble(PoissA2), 2.0)) + Math.Pow(Convert.ToDouble(PoissA3), 2.0)) + Math.Pow(Convert.ToDouble(PoissA4), 2.0)) + Math.Pow(Convert.ToDouble(PoissA5), 2.0)) / Convert.ToDouble(decimal.Subtract(PoisL5Kom, decimal.One)), 0.5), 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
            TextBoxSTD2 = Strings.FormatNumber(Math.Pow(((((Math.Pow(Convert.ToDouble(PoissB1), 2.0) + Math.Pow(Convert.ToDouble(PoissB2), 2.0)) + Math.Pow(Convert.ToDouble(PoissB3), 2.0)) + Math.Pow(Convert.ToDouble(PoissB4), 2.0)) + Math.Pow(Convert.ToDouble(PoissB5), 2.0)) / Convert.ToDouble(decimal.Subtract(PoisL5Kom, decimal.One)), 0.5), 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
            TextBoxKOV = Strings.FormatNumber(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Divide(decimal.Multiply(PoissA1, PoissB1), decimal.Subtract(PoisL5Kom, decimal.One)), decimal.Divide(decimal.Multiply(PoissA2, PoissB2), decimal.Subtract(PoisL5Kom, decimal.One))), decimal.Divide(decimal.Multiply(PoissA3, PoissB3), decimal.Subtract(PoisL5Kom, decimal.One))), decimal.Divide(decimal.Multiply(PoissA4, PoissB4), decimal.Subtract(PoisL5Kom, decimal.One))), decimal.Divide(decimal.Multiply(PoissA5, PoissB5), decimal.Subtract(PoisL5Kom, decimal.One))), 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
        }

        public decimal PoissA3 { get; set; }

        public decimal PoissA4 { get; set; }

        public decimal PoissA5 { get; set; }

        public decimal PoissL5X5 { get; set; }

        public decimal PoissL5Y2 { get; set; }

        public decimal PoissL5Y1 { get; set; }

        public decimal PoissL5X4 { get; set; }

        public decimal PoissL5X3 { get; set; }

        public object Poisson5LastY1 { get; set; }

        public object Poisson5LastY2 { get; set; }

        public object Poisson5LastY3 { get; set; }

        public decimal PoissB3 { get; set; }

        public decimal PoissL5Y4 { get; set; }

        public decimal PoissL5Y3 { get; set; }

        public decimal PoissL5X2 { get; set; }

        public decimal PoissB2 { get; set; }

        public decimal PoissA2 { get; set; }

        public object TextBoxKOV { get; set; }

        public object TextBoxSTD2 { get; set; }

        public object TextBoxSTD1 { get; set; }

        public object Away { get; set; }

        public object Home { get; set; }

        public object Away1 { get; set; }

        public object Home1 { get; set; }

        public object BvPAvgPoenaX { get; set; }

        public object BvPAvgPoenaY { get; set; }

        public decimal PoissSumL5X { get; set; }

        public object BvPSumAvG { get; set; }

        public object BvPSumDifXY { get; set; }

        public decimal PoissL5X1 { get; set; }

        public decimal PoissB1 { get; set; }

        public decimal PoissA1 { get; set; }

        public decimal PoissSumL5Y { get; set; }

        public decimal PoissL5Y5 { get; set; }

        public object Poisson5LastY4 { get; set; }

        public decimal PoissB4 { get; set; }

        public object Poisson5LastX5 { get; set; }

        public object Poisson5LastY5 { get; set; }

        public decimal PoissB5 { get; set; }

        public object Poisson5LastX4 { get; set; }

        public object Poisson5LastX3 { get; set; }

        public object Poisson5LastX2 { get; set; }

        public decimal PoisL5Kom { get; set; }

        
        public object Poisson5LastX1 { get; set; }
        public object ThetaPoisson { get; private set; }
    }
}
