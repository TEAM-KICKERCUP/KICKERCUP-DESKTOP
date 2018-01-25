using Logic.CompetitorManagement.Impl;
using Logic.Database;
using Logic.Database.DataLayer;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Logic.TournamentManagement.Pers
{
    public static class TMPersistenz
    {
        public static void AddTournament(String turniername, String username, String gamemode, int amountSets,
            int amountGoals, bool isRanked)
        {
            TournamentDL tdl = new TournamentDL();
            tdl.CreateTournament(new TournamentPers(turniername, username, gamemode, amountSets, amountGoals, isRanked,
                false));
        }

        public static TournamentPers FindTournament(String turniername)
        {
            TournamentDL tdl = new TournamentDL();
            return tdl.FindTournament(turniername);
        }


        public static ITournament GetTournamentFromDB(String turniername, List<ICompetitor> teilnehmerListe)
        {
            TournamentFactory tF = new TournamentFactory();
            IGameMode gm;

            if (FindTournament(turniername).IsFinished == true)
            {
                throw new Exception("Das Turnier wurde bereits gespielt");
            }

            if (FindTournament(turniername).Gamemode == "Double Elimination")
            {
                gm = new DoubleElimination();
            }
            else if (FindTournament(turniername).Gamemode == "Ranked Solo Match")
            {
                gm = new RankedSoloMatch();
            }
            else if (FindTournament(turniername).Gamemode.Equals("Ranked Team Match"))
            {
                gm = new RankedTeamMatch();
            }
            else
            {
                throw new Exception("GameMode existiert nicht");
            }

            return tF.CreateTournament(turniername, teilnehmerListe, FindTournament(turniername).AmountSets,
                FindTournament(turniername).AmountGoals, gm, FindTournament(turniername).IsRanked);
        }

        public static List<TournamentPers> GetAllTournaments(String username)
        {
            using (DatabaseContextInstance db = DatabaseContext.GetContext())
            {
                List<TournamentPers> query = db.Tournaments
                    .SqlQuery("select * from Tournaments where Username = '" + username + "'").ToList();
                return query.OrderByDescending(o => o.Date).ToList();
            }
        }


        public static void SaveFinishedTournamendToDB(ITournament tournament, String username)
        {
            TournamentDL tdl = new TournamentDL();
            tdl.UpdateTournament(new TournamentPers(tournament.Name, username, tournament.GameMode.ToString(),
                tournament.AmountSets, tournament.AmountGoalsperSet, tournament.IsRanked, true));
        }


        public static void SaveRankingsToDB(ITournament tournament)
        {
            if (!tournament.IsRanked)
            {
                throw new Exception("Das Turnier ist nicht gerankt. Daher darf auch kein Ranking gespeichert werden");
            }

            int maxSiegeAnzahl = 0;
            Team siegerTeam = null;

            foreach (Team t in tournament.Ranking.Keys)
            {
                int anzahlSiege = tournament.Ranking[t];
                if (anzahlSiege > maxSiegeAnzahl)
                {
                    siegerTeam = t;
                    maxSiegeAnzahl = anzahlSiege;
                }

                foreach (ICompetitor c in t.SpielerListe)
                {
                    RankingDL rdl = new RankingDL();
                    rdl.CreateRanking(new RankingPers(tournament.Name, c.CompetitorID, anzahlSiege, false));
                }
            }

            foreach (ICompetitor c in siegerTeam.SpielerListe)
            {
                RankingDL rdl = new RankingDL();
                rdl.SetWinner(tournament.Name, c.CompetitorID);
            }
        }


        public static List<RankingPers> LoadRankings(String turniername)
        {
            CompetitorIMPL cimpl = new CompetitorIMPL();
            using (DatabaseContextInstance db = DatabaseContext.GetContext())
            {
                List<RankingPers> query = db.Rankings
                    .SqlQuery("select * from Rankings r where r.Tournament = " + "'" + turniername + "'").ToList();

                //Vorname + Nachname der jeweiligen CompetitorID als String anfügen.
                foreach (RankingPers rp in query)
                {
                    rp.CVornameNachname = cimpl.FindCompetitor(rp.CompetitorID).Name + " " +
                                          cimpl.FindCompetitor(rp.CompetitorID).Surname;
                }

                return query.OrderByDescending(o => o.Wins).ToList();
            }
        }

        public static void DeleteTournamentInDB(String turniername) {
            using (DatabaseContextInstance db = DatabaseContext.GetContext())
            {
                try
                {

                    db.Database.ExecuteSqlCommand("delete from Rankings where Tournament = " + "'" + turniername + "'");
                    db.Database.ExecuteSqlCommand("delete from Tournaments where Name = " + "'" + turniername + "'");
                } catch
                {
                    throw new Exception("Fehler beim löschen eines Turniers");
                }
            
            }

            }
        }
}