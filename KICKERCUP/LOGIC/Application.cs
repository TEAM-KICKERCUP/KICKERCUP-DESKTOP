using Logic.TournamentManagement;
using Logic.CompetitorManagement.Impl;
using Logic.ClientManagement.Impl;
using Logic.TournamentManagement.Impl;
using Logic.TournamentManagement.Pers;
using System;

namespace Logic
{
    public class Application
    {
        static void Main(string[] args)
        {
            try
            {
                TMPersistenz.DeleteTournamentInDB("test");
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


         

            //TournamentFactory tF = new TournamentFactory();

            ////Datenbankbefuellung
            //CompetitorIMPL comp = new CompetitorIMPL();
            //ClientIMPL cdl = new ClientIMPL();

            //cdl.AddClient("SamWise", "samwise", "Sam", "Gamdschie", "samwise@web.de", "männlich");
            //cdl.AddClient("Wizard", "wizard", "Gandalf", "TheGrey", "gandalfthegrey@gmail.com", "männlich");

            //comp.AddCompetitor("Frodo", "Beutlin", "männlich", 1500, "global", "SamWise");
            //comp.AddCompetitor("Sam", "Gamdschie", "männlich", 1500, "global", "SamWise");
            //comp.AddCompetitor("Meriadoc", "Brandybock", "männlich", 1500, "lokal", "SamWise");
            //comp.AddCompetitor("Peregrin", "Tuk", "männlich", 1500, "lokal", "SamWise");
            //comp.AddCompetitor("Arwen", "Evenstar", "weiblich", 1500, "global", "SamWise");
            //comp.AddCompetitor("Eowyn", "of Rohan", "weiblich", 1500, "global", "SamWise");
            //comp.AddCompetitor("Tauriel", "Green", "weiblich", 1500, "global", "Wizard");
            //comp.AddCompetitor("Rosie", "Gamdschie", "weiblich", 1500, "global", "Wizard");
            //comp.AddCompetitor("Aragorn", "Strider", "männlich", 1500, "lokal", "Wizard");
            //comp.AddCompetitor("Boromir", "of Gondor", "männlich", 1500, "lokal", "Wizard");
            //comp.AddCompetitor("Legolas", "of Mirkwood", "männlich", 1500, "global", "Wizard");
            //comp.AddCompetitor("Faramir", "of Gondor", "männlich", 1500, "global", "Wizard");

            //TMPersistenz.AddTournament("MiddleEarthCup", "SamWise", "Double Elimination", 3, 10, true);
            //TMPersistenz.AddTournament("METournament", "Wizard", "Double Elimination", 3, 10, false);
            //TMPersistenz.AddTournament("CaradhrasRace", "Wizard", "Ranked Solo Match", 5, 5, true);
            //TMPersistenz.AddTournament("ShireKicker", "SamWise", "Ranked Team Match", 1, 10, true);

            ////So baut man ein RankedTeamMatch

            //List<ICompetitor> teilnehmerListe = new List<ICompetitor>
            //{
            //    new Competitor("x1", "Schmitz", "männlich", 1000, "local"),
            //    new Competitor("x2", "Müller", "weiblich", 1000, "local"),
            //    new Competitor("x3", "Schmitz", "männlich", 1000, "local"),
            //    new Competitor("x4", "Müller", "weiblich", 1000, "local")
            //};

            //ITournament rankedTeamMatch =
            //    tF.CreateTournament("RankedTeamMatch", teilnehmerListe, 2, 5, new RankedTeamMatch(), true);

            //List<Team> teams;
            //int nr = 1;
            //rankedTeamMatch.StartTournament();

            //while (!rankedTeamMatch.IsFinished)
            //{
            //    Console.WriteLine(@"
            //Spiel Nummer " + nr++);
            //    teams = rankedTeamMatch.CurrentMatch.GetTeams();
            //    Console.WriteLine(teams.ElementAt(0).ToString() + " vs " + teams.ElementAt(1).ToString());
            //    rankedTeamMatch.SetGoalForCurrentSet(teams.ElementAt(0), 10);
            //    rankedTeamMatch.SetGoalForCurrentSet(teams.ElementAt(1), 9);
            //}

            //Console.WriteLine(@"************
            //" + rankedTeamMatch.ToString());
            //rankedTeamMatch.GetTournamentTree();


            /*
                        //So Baut man ein Turnier
                       List<ICompetitor> teilnehmerListe = new List<ICompetitor>
                       {
                           new Competitor("x1", "Schmitz", "männlich" , 1000, "local"),
                           new Competitor("x2", "Müller", "weiblich", 1000, "local"),
                           new Competitor("x3", "Schmitz", "männlich" , 1000, "local"),
                           new Competitor("x4", "Müller", "weiblich", 1000, "local"),
                           new Competitor("x5", "Schmitz", "männlich" , 1000, "local"),
                           new Competitor("x6", "Müller", "weiblich", 1000, "local")
                       };



                        //Alle Teilnehmer aus der Datenbank Laden
                        CompetitorIMPL cimpl = new CompetitorIMPL();
                        IDictionary<Guid, Competitor> competitors = new SortedList<Guid, Competitor>();
                        competitors = cimpl.LoadCompetitors("","test");

                        List<ICompetitor> teilnehmerListeDB = new List<ICompetitor>();
                        foreach (KeyValuePair<Guid, Competitor> c in competitors)
                        {
                            teilnehmerListeDB.Add(c.Value);    
                        }

                        // Teilnehmer in DB speichern (Nur einmamlig)

                        //foreach (Competitor c in teilnehmerListe)
                        //{
                        //    cimpl.AddCompetitor(c.Name, c.Surname, c.Gender, c.SkillLevel, c.Visibility, "test");
                        //}



                        //Turnier Basisdaten für Datenbank erstellen
                        TMPersistenz.AddTournament("test", "user", "DoubleElimination", 4, 10, true);

                        //Vorab erstelltes Turnier aus der Datenbank in vollwertiges Laufzeit Turnier umwandeln
                        ITournament tournament = TMPersistenz.GetTournamentFromDB("3", teilnehmerListeDB);

                        //Turnier Simulation
                        tournament.StartTournament();

                        int nr = 1;
                        List<Team> teams;

                        while (!tournament.IsFinished)
                        {
                            Console.WriteLine(@"
            Spiel Nummer " + nr++);
                            teams = tournament.CurrentMatch.GetTeams();
                            Console.WriteLine(teams.ElementAt(0).ToString() + " vs " + teams.ElementAt(1).ToString());
                            tournament.SetGoalForCurrentSet(teams.ElementAt(0), 10);
                            tournament.SetGoalForCurrentSet(teams.ElementAt(1), 9);

                            tournament.SetGoalForCurrentSet(teams.ElementAt(0), 10);
                            tournament.SetGoalForCurrentSet(teams.ElementAt(1), 9);

                            tournament.SetGoalForCurrentSet(teams.ElementAt(0), 10);
                            tournament.SetGoalForCurrentSet(teams.ElementAt(1), 9);

                            tournament.SetGoalForCurrentSet(teams.ElementAt(0), 10);
                            tournament.SetGoalForCurrentSet(teams.ElementAt(1), 9);
                            Console.WriteLine(@"Winner: " + tournament.CurrentMatch.Winner);

                        }

                        Console.WriteLine(@"************
            " + tournament.ToString());
                        tournament.GetTournamentTree();

                        //Basisdaten für eine gespieltes Turnier in der Datenbank aktualisieren
                        TMPersistenz.SaveFinishedTournamendToDB(tournament, "test");

                        //Rankings des Turniers in der Datenbank abspeichern
                        TMPersistenz.SaveRankingsToDB(tournament);

                        //Ranking aus der Datenbank laden und mir Namen anreichern
                        List<RankingPers> l = TMPersistenz.LoadRankings("3");

                        //Ausgabe des Rankings
                        foreach(RankingPers rp in l)
                        {
                            Console.WriteLine(rp.CVornameNachname + " " + rp.Wins + " " + rp.WonFinal);
                        }

                        //Alle Turniere in einer Liste
                        List<TournamentPers> t = TMPersistenz.GetAllTournaments();
                        foreach (TournamentPers tp in t)
                        {
                            Console.WriteLine(tp.Name + " " + tp.Date);
                        }
                    */
        }
    }
}