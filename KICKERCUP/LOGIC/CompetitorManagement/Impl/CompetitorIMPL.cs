using System;
using System.Collections.Generic;
using Logic.CompetitorManagement.Et;
using Logic.Database;
using Logic.Database.DataLayer;

namespace Logic.CompetitorManagement.Impl
{
    /// <summary>
    /// CompetitorManagement.Impl
    /// Methoden fuer die Interaktion mit der Datenbank
    /// Werden in der GUI aufgerufen
    /// </summary>
    public class CompetitorIMPL
    {
        private IDictionary<Guid, Competitor> competitors = new SortedList<Guid, Competitor>();

        // Einen Teilnehmer zur Datenbank hinzufuegen
        public void AddCompetitor(String name, String surname, String gender, int skilllevel, String visibility,
            String username)
        {
            CompetitorDL comp = new CompetitorDL();
            Competitor competitor = new Competitor(name, surname, gender, skilllevel, visibility, username);
            comp.CreateCompetitor(competitor);
        }

        // Einen Teilnehmer in der Datenbank updaten
        public void UpdateCompetitor(Guid compID, String name, String surname, String gender, int skilllevel,
            String visibility)
        {
            CompetitorDL comp = new CompetitorDL();
            Competitor competitor = new Competitor(name, surname, gender, skilllevel, visibility);
            comp.UpdateCompetitor(compID, competitor);
        }

        public void UpdateCompetitor(Guid comID, int skillChange)
        {
            CompetitorDL comp = new CompetitorDL();
            Competitor c = FindCompetitor(comID);
            int newSkilllevel = skillChange + c.SkillLevel;
            c.SkillLevel = newSkilllevel;
            comp.UpdateCompetitor(c.CompetitorID, c);
        }


        /*Alle Teilnehmer laden, Dictionary competitors in List umwandeln, durchsuchen und 
          ein Dictionary mit den passenden Teilnehmern zurueckgeben*/
        public IDictionary<Guid, Competitor> LoadCompetitors(String searchTerm, String username)
        {
            CompetitorDL comp = new CompetitorDL();
            this.competitors = comp.LoadCompetitors();
            IList<Competitor>
                competitorsList =
                    (IList<Competitor>) this.competitors
                        .Values; // um es mit foreach mit dem Suchbegriff durchsuchbar zu machen
            IDictionary<Guid, Competitor> result = new SortedList<Guid, Competitor>();

            foreach (Competitor c in competitorsList)
            {
                //gibt -1 zurueck wenn substring nicht gefunden
                if (c.Name.IndexOf(searchTerm) != -1 || c.Surname.IndexOf(searchTerm) != -1)
                {
                    if (c.Visibility == "global" || username == c.Username)
                        result.Add(c.CompetitorID, c);
                }
            }

            return result;
        }

        //Einen Teilnehmer finden anhand der ID
        public Competitor FindCompetitor(Guid CompetitorID)
        {
            CompetitorDL comp = new CompetitorDL();
            Competitor c = comp.FindCompetitor(CompetitorID);
            return c;
        }

        //Einen Teilnehmer loeschen anhand der ID
        public void DeleteCompetitor(Guid CompetitorID)
        {
            RankingDL rdl = new RankingDL();
            rdl.DeleteRankingPerCompetitor(CompetitorID);
            CompetitorDL comp = new CompetitorDL();
            comp.DeleteCompetitor(CompetitorID);
        }

        //Alle Teilnehmer loeschen
        public void DeleteAllCompetitors()
        {
            RankingDL rdl = new RankingDL();
            rdl.DeleteAllRankings();
            CompetitorDL comp = new CompetitorDL();
            comp.DeleteAllCompetitors();
        }
    }
}