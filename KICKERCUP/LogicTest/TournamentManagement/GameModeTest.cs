using System;
using System.Collections.Generic;
using System.Linq;
using Logic;
using Logic.CompetitorManagement.Et;
using Logic.TournamentManagement;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Execution.Impl;
using Logic.TournamentManagement.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTest.TournamentManagement
{
    [TestClass]
    public class GameModeTest
    {
        [TestMethod]
        public void TestSets()
        {
            Team t1 = new Team(new Competitor("x1", "Schmitz", "männlich", 2806, "local"));

            Team t2 = new Team(new Competitor("x3", "Schmitz", "männlich", 2577, "local"));

            Match m = new Match(t1, t2, 3, 10);

            //Satz 1
            m.GetCurrentGameSet.SetGoals(t1, 10);
            m.GetCurrentGameSet.SetGoals(t2, 9);


            //Satz 2
            m.GetCurrentGameSet.SetGoals(t1, 8);
            m.GetCurrentGameSet.SetGoals(t2, 10);

            //Satz 3
            m.GetCurrentGameSet.SetGoals(t1, 10);
            m.GetCurrentGameSet.SetGoals(t2, 7);

            Assert.AreEqual(t1, m.Winner);
            Assert.AreEqual(t2, m.Loser);
            Assert.AreEqual(true, m.IsFinished);
        }

        [TestMethod]
        public void TestSoloRanked()
        {
            List<ICompetitor> competitorList = new List<ICompetitor>
            {
                new Competitor("Johannes", "Schweer", "male", 1500, "local"),
                new Competitor("Christopher", "Heid", "male", 1500, "local")
            };

            Tournament t = new Tournament("TestRankedSolo", competitorList, 3, 12, new RankedSoloMatch(), false);

            t.StartTournament();
            List<Team> team = t.CurrentMatch.GetTeams();
            t.SetGoalForCurrentSet(team.ElementAt(0), 10, team.ElementAt(1), 8);

            Assert.AreEqual(competitorList.ElementAt(0), t.CurrentMatch.Winner.SpielerListe.ElementAt(0));
        }
    }
}