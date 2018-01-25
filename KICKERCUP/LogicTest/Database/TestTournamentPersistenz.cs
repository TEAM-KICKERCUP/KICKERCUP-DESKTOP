using System;
using System.Collections.Generic;
using System.Linq;
using Logic;
using Logic.ClientManagement.Impl;
using Logic.CompetitorManagement.Et;
using Logic.Database;
using Logic.Database.DataLayer;
using Logic.TournamentManagement;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Impl;
using Logic.TournamentManagement.Pers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTest.Database
{
    [TestClass]
    public class TestTournamentPersistenz
    {
        [TestMethod]
        public void TestSaveTournament()
        {
            List<ICompetitor> competitorList = new List<ICompetitor>
            {
                new Competitor("jkghjk", "Schwsadfsdfeer", "male", 1500, "local"),
                new Competitor("Chrisasdfsadtopher", "Heiasdfsdfd", "male", 1500, "local")
            };

            Tournament t = new Tournament("TURNIER_TESTfjlvdslvdslsdvlhsdhshdvlas", competitorList, 3, 12, new RankedSoloMatch(), false);

            t.StartTournament();
            List<Team> team = t.CurrentMatch.GetTeams();
            t.SetGoalForCurrentSet(team.ElementAt(0), 10, team.ElementAt(1), 8);

            ClientIMPL cdl = new ClientIMPL();  
            if (cdl.FindClient("lhglhgljljlhlhbvhlhbkjhlj") != null)
            {
                cdl.AddClient("lhglhgljljlhlhbvhlhbkjhlj", "samwise", "Sam", "Gamdschie", "samwise@web.de", "männlich");
            }

            TMPersistenz.SaveFinishedTournamendToDB(t, "lhglhgljljlhlhbvhlhbkjhlj");

            Assert.AreEqual(t, (Tournament) TMPersistenz.GetTournamentFromDB("TURNIER_TESTfjlvdslvdslsdvlhsdhshdvlas", competitorList));

            //Lösche das Ranking und Tournament
            TMPersistenz.DeleteTournamentInDB("TURNIER_TESTfjlvdslvdslsdvlhsdhshdvlas");


        }
    }
}
