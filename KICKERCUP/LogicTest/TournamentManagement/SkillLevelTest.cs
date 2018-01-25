using System;
using Logic.CompetitorManagement.Et;
using Logic.TournamentManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LogicTest.TournamentManagement
{
    [TestClass]
    public class SkillLevelTest
    {
        [TestMethod]
        public void TestTeamSkillLevel()
        {
            Team t1 = new Team(new Competitor("x1", "Schmitz", "männlich", 2806, "local"),
                new Competitor("x2", "Müller", "weiblich", 2806, "local"));

            Team t2 = new Team(new Competitor("x3", "Schmitz", "männlich", 2577, "local"),
                new Competitor("x4", "Müller", "weiblich", 2577, "local"));


            t2.CalcTeamSkillLevel(t1.NewSkillLevel, false);

            // Console.WriteLine("Team 2 mit " + t2.SkillLevel + " hat nun ein SkillLevel von " + t2.NewSkillLevel );

            //Quelle für den Wert 2569: http://www.schachbundesliga.de/elo-rechner#auswertung
            Assert.AreEqual(t2.NewSkillLevel, 2569);
        }

        [TestMethod]
        public void TestCompetitorSkillLevel()
        {
            Team t1 = new Team(new Competitor("x1", "Schmitz", "männlich", 2806, "local"));

            Team t2 = new Team(new Competitor("x3", "Schmitz", "männlich", 2577, "local"));

            t2.CalcTeamSkillLevel(t1.NewSkillLevel, false);

            // Console.WriteLine("Team 2 mit " + t2.SkillLevel + " hat nun ein SkillLevel von " + t2.NewSkillLevel );

            //Quelle für den Wert 2569: http://www.schachbundesliga.de/elo-rechner#auswertung
            Assert.AreEqual(t2.NewSkillLevel, 2569);
        }

        [TestMethod]
        public void TestExpectedValue()
        {
            Team t1 = new Team(new Competitor("x1", "Schmitz", "männlich", 2000, "local"));

            Team t2 = new Team(new Competitor("x3", "Schmitz", "männlich", 2000, "local"));

            Assert.AreEqual(50, (int) (t1.CalcWinExpectation(t2.SkillLevel) * 100));
        }
    }
}