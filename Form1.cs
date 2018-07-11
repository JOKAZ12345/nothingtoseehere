using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using HtmlAgilityPack;
using MathNet.Numerics.Distributions;

namespace SoccerDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //endDateTextBox.Text = DateTime.ParseExact(DateTime.Today.ToString(), "d/MM/yy", null).ToString();

            var db = new soccerDBDataContext();

            /*var box = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = db.Countries.Select(x => x.Name),
                Name = "country",
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.None
            };

            box.SelectedIndexChanged += new EventHandler(countryBox_SelectedIndexChanged);

            var pt = groupBox1.DisplayRectangle.Location;
            pt.X += (groupBox1.DisplayRectangle.Width - box.Width) / 2;
            pt.Y += (groupBox1.DisplayRectangle.Height - box.Height) / 2;

            groupBox1.Location = pt;

            groupBox1.Controls.Add(box);*/

            comboBox1.DataSource = db.Countries.Select(x => x.Name);
            comboBox2.DataSource = db.Competitions.Where(x => x.Country_name == comboBox1.Text).Select(x => x.Name);

            startDateTextBox.Text = "01/08/2017";
            endDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void countryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(groupBox1.Controls["country"].Text);
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var db = new soccerDBDataContext();
            comboBox3.DataSource = db.Teams.Where(x => x.compName == comboBox2.Text).Select(x => x.Name);
            comboBox4.DataSource = db.Teams.Where(x => x.compName == comboBox2.Text).Select(x => x.Name);
        }

        public void setUnderRobinsonLabel(bool res)
        {
            robinsonLabel.Text = res.ToString();

            robinsonLabel.BackColor = res ? Color.Chartreuse : Color.Red;
        }

        public void setOverRobinsonLabel(bool res)
        {
            robinsonOver.Text = res.ToString();

            robinsonOver.BackColor = res ? Color.Chartreuse : Color.Red;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var p = new populateDB();
            p.populate(comboBox3.Text, comboBox4.Text, comboBox2.Text, startDateTextBox.Text, endDateTextBox.Text, thetaTextBox.Text);

            var Calc = new Calculator();

            setUnderRobinsonLabel(Calc.Robinson_Under25(comboBox3.Text, comboBox4.Text, comboBox2.Text));
            setOverRobinsonLabel(Calc.Robinson_Over25(comboBox3.Text, comboBox4.Text, comboBox2.Text));

            string [] homx = new string[5];
            homx[0] = Poisson5LastX1.Text;
            homx[1] = Poisson5LastX2.Text;
            homx[2] = Poisson5LastX3.Text;
            homx[3] = Poisson5LastX4.Text;
            homx[4] = Poisson5LastX5.Text;

            string[] aways = new string[5];
            aways[0] = Poisson5LastY1.Text;
            aways[1] = Poisson5LastY2.Text;
            aways[2] = Poisson5LastY3.Text;
            aways[3] = Poisson5LastY4.Text;
            aways[4] = Poisson5LastY5.Text;

            Calc.PoissonTheta(homx, aways);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var placard = new PlacardRequest();
            placard.eventos = new List<Event>(await placard.GetEvents(placard.NextEvents, "FOOT"));
            //var eventos = await placard.GetEvents(placard.NextEvents, "FOOT");

            var teams = new List<Team>();

            foreach (var v in placard.eventos)
            {
                //var evn = v.homeOpponentDescription + " vs " + v.awayOpponentDescription + "\n" + v.eventPaths[1].eventPathDescription + "\n" + v.eventPaths[2].eventPathDescription;
                teams.Add(new Team() { Name = v.homeOpponentDescription, Country_name = v.eventPaths[1].eventPathDescription });
                teams.Add(new Team() { Name = v.awayOpponentDescription, Country_name = v.eventPaths[1].eventPathDescription });
            }

            var p = new populateDB();
            p.addTeams(teams);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = "C:\\Users",
                IsFolderPicker = false,
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You selected: " + dialog.FileName);
            }

            var p = new populateDB();
            p.readCSV(dialog.FileName, comboBox1.Text, comboBox2.Text, 1); // Todo: Alter this to generic input
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var db = new soccerDBDataContext();
            comboBox2.DataSource = db.Competitions.Where(x => x.Country_name == comboBox1.Text).Select(x => x.Name);
        }

        private void getCompetitionsLink()
        {
            var db = new soccerDBDataContext();

            var Webget = new HtmlWeb();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var doc = Webget.Load("https://www.academiadasapostas.com/stats/list");

            var classToFind = "toggle_content";
            var allElementsWithClassFloat = doc.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", classToFind));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string league = "Segunda Liga 2017/2018";

            getCompetitionsLink();

            var db = new soccerDBDataContext();

            var Webget = new HtmlWeb();

            // Todo add this
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // https://www.academiadasapostas.com/stats/competition/portugal-stats/100/12625/35896 SEGUNDA LIGA
            var doc = Webget.Load("https://www.academiadasapostas.com/stats/competition/portugal-stats/100");
                //SEGUNDA LIGA

            var ourNode = doc.DocumentNode.SelectNodes("//*[@id=\"s\"]/div/div/div/span[2]/table/tbody/tr");

            var teams = new Dictionary<string, string>();

            // CORRECT XPATH
            // row.SelectNodes("/html[1]/body[1]/div[12]/div[2]/div[1]/div[1]/div[1]/span[2]/table[1]/tbody[1]/tr[1]/td[3]/a")
            var cont = 1;
            foreach (var row in ourNode)
            {
                var t =
                    row.SelectSingleNode(
                        "/html[1]/body[1]/div[12]/div[2]/div[1]/div[1]/div[1]/span[2]/table[1]/tbody[1]/tr[" + cont++ +
                        "]/td[3]/a");
                var n = t.OuterHtml;

                var url = n.Split('"', '"')[1];
                var sub = n.Substring(n.IndexOf("\n") + "\n".Length);

                // Let's clear empty spaces
                var start_index = 0; // Let's start from the beggining
                for (var i = 0; i < sub.Length; i++)
                {
                    if (sub[i] == ' ') continue;
                    start_index = i;
                    break;
                }

                var end_index = sub.Length; // Starts from the end
                for (var j = sub.Length - 5; j >= 0; j--) // Removes the last <a href> tag
                {
                    if (sub[j] == ' ') continue;
                    end_index = j;
                    break;
                }

                var team = sub.Substring(start_index, end_index - start_index + 1);

                teams.Add(team, url);
            }

            var _comp = db.Competitions.FirstOrDefault(c => c.Name == league);
                //TODO: Não preciso de estar sempre aqui Uma vez que a 

            foreach (var team in teams.Keys)
            {
                // Inserir aqui as equipas na BD se não existirem
                if (!db.Teams.Any(x => x.Name == team)) // Todo: Alojar na memória só uma vez
                {
                    db.Teams.InsertOnSubmit(new Team()
                    {
                        Name = team,
                        compName = league,
                        Country_name = "Portugal"
                    });

                    db.SubmitChanges();
                }

                else
                {
                    var _team = db.Teams.First(x => x.Name == team);

                    if (_team != null /* && team.compName == null*/)
                    {
                        if (_comp == null)
                        {
                            // TODO: Não existe a competição tenho de a criar..

                            var x = new Competition()
                            {
                                Name = league,
                                //TODO: Adicionar os países e o nível da liga (1,2,3,4,5)..
                            };

                            _comp = x;
                        }

                        _team.Competition1 = _comp;

                        db.SubmitChanges();

                    }
                }


                doc = Webget.Load(teams[team]);

                var node =
                    doc.DocumentNode.SelectNodes("//*[@id=\"s\"]/div/div/div/div/div[2]/div/div[1]/table/tbody");

                var _cont = 0;
                foreach (var row in node.Elements())
                {
                    _cont = _cont + 1;

                    if (_cont >= node.Elements().Count() / 2) continue;

                    var data =
                        row.SelectSingleNode(
                            "/html[1]/body[1]/div[12]/div[2]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr[" +
                            _cont + "]/td[1]").InnerText;

                    var comp =
                        row.SelectSingleNode(
                            "/html[1]/body[1]/div[12]/div[2]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr[" +
                            _cont + "]/td[3]/a").InnerText;

                    if (!comp.Contains("Segunda Liga 17/18")) continue;

                    var res =
                        row.SelectSingleNode(
                            "/html[1]/body[1]/div[12]/div[2]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr[" +
                            _cont + "]/td[5]/a").InnerText;

                    if (res.ToLower().Contains("vs") || res.ToLower().Contains("adiado") ||
                        res.ToLower().Contains("postponed")) continue;

                    var away =
                        row.SelectSingleNode(
                            "/html[1]/body[1]/div[12]/div[2]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr[" +
                            _cont + "]/td[6]/a").InnerText;

                    var home =
                        row.SelectSingleNode(
                            "/html[1]/body[1]/div[12]/div[2]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr[" +
                            _cont + "]/td[4]/a").InnerText;

                    res = res.Replace("\r\n", "");
                    away = away.Replace("\r\n", "");
                    home = home.Replace("\r\n", "");

                    // Let's clear empty spaces
                    var start_index = 0; // Let's start from the beggining
                    for (var i = 0; i < res.Length; i++)
                    {
                        if (res[i] == ' ') continue;
                        start_index = i;
                        break;
                    }

                    var end_index = res.Length; // Starts from the end
                    for (var j = res.Length - 5; j >= 0; j--) // Removes the last <a href> tag
                    {
                        if (res[j] == ' ') continue;
                        end_index = j;
                        break;
                    }

                    res = res.Substring(start_index, end_index - start_index + 1);

                    start_index = 0; // Let's start from the beggining
                    for (var i = 0; i < away.Length; i++)
                    {
                        if (away[i] == ' ') continue;
                        start_index = i;
                        break;
                    }

                    end_index = away.Length; // Starts from the end
                    for (var j = away.Length - 5; j >= 0; j--) // Removes the last <a href> tag
                    {
                        if (away[j] == ' ') continue;
                        end_index = j;
                        break;
                    }

                    away = away.Substring(start_index, end_index - start_index + 1);

                    start_index = 0; // Let's start from the beggining
                    for (var i = 0; i < home.Length; i++)
                    {
                        if (home[i] == ' ') continue;
                        start_index = i;
                        break;
                    }

                    end_index = home.Length; // Starts from the end
                    for (var j = home.Length - 5; j >= 0; j--) // Removes the last <a href> tag
                    {
                        if (home[j] == ' ') continue;
                        end_index = j;
                        break;
                    }

                    home = home.Substring(start_index, end_index - start_index + 1);

                    var gamesOnDB = db.Matches.Where(c => c.Competition_name == league).ToList();
                    // Todo: Put generic

                    var s = res.Split('-');
                    var h = Convert.ToInt32(s[0]);
                    var a = Convert.ToInt32(s[1]);

                    char resultado;

                    if (h > a)
                        resultado = 'H';
                    else if (a > h)
                        resultado = 'A';
                    else
                        resultado = 'D';


                    var game = new Match
                    {
                        Home_team = home,
                        Away_team = away,
                        away_goals = a,
                        home_goals = h,
                        date = Convert.ToDateTime(data),
                        final_result = resultado,
                        Competition_name = league //,
                        //Team = db.Teams.Single(c => c.Country_name == country && c.Name == games[j].HomeTeam),
                        //Team1 = db.Teams.Single(c => c.Country_name == country && c.Name == games[j].AwayTeam)
                    };

                    if (GamesExistsDb(game, gamesOnDB)) continue;

                    db.Matches.InsertOnSubmit(game);
                    // Bug: Don't commit outside! If you commit outside it will bug the SubmitChanges() submitting already stuff on DB!
                    db.SubmitChanges();

                    // Inserir aqui os dados na BD
                }
            }

            db.Dispose();
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            var p = new populateDB();
            p.populate2(comboBox2.Text, startDateTextBox.Text, endDateTextBox.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            var p = new populateDB();
            p.testTheta(comboBox2.Text, startDateTextBox.Text, endDateTextBox.Text, Convert.ToDouble(thetaTextBox.Text));
        }
    }
}
