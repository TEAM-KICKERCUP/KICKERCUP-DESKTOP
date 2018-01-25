using System;
using System.Collections.Generic;
using Logic.CompetitorManagement.Et;

namespace Logic.Database
{
    public class CompetitorDL
    {
        public CompetitorDL()
        {
        }

        //alle Teilnehmer laden
        public IDictionary<Guid, Competitor> LoadCompetitors()
        {
            IDictionary<Guid, Competitor> competitors = new SortedList<Guid, Competitor>();
            using (DatabaseContextInstance db = DatabaseContext.GetContext())
            {
                IEnumerable<Competitor> competitors1 = db.Competitors;
                foreach (Competitor c in competitors1)
                {
                    competitors.Add(c.CompetitorID, c);
                }
            }

            return competitors;
        }

        //Teilnehmer hinzufuegen
        public void CreateCompetitor(Competitor participant)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Competitors.Add(participant);
            cdl.SaveChanges();
        }

        //Teilnehmer anhand der ID finden
        public Competitor FindCompetitor(Guid CompetitorID)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            Competitor c = cdl.Competitors.Find(CompetitorID);
            return c;
        }

        //Teilnehmer anhand der ID loeschen
        public void DeleteCompetitor(Guid CompetitorID)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Competitors.Remove(cdl.Competitors.Find(CompetitorID));
            cdl.SaveChanges();
        }

        //Teilnehmer anhand der ID updaten
        public void UpdateCompetitor(Guid compID, Competitor newParticipant)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            Competitor oldParticipant = cdl.Competitors.Find(compID);
            oldParticipant.Name = newParticipant.Name;
            oldParticipant.Surname = newParticipant.Surname;
            oldParticipant.Gender = newParticipant.Gender;
            oldParticipant.SkillLevel = newParticipant.SkillLevel;
            oldParticipant.Visibility = newParticipant.Visibility;
            cdl.SaveChanges();
        }

        //Alle Teilnehmer loeschen
        public void DeleteAllCompetitors()
        {
            using (DatabaseContextInstance cdl = DatabaseContext.GetContext())
            {
                cdl.Competitors.RemoveRange(cdl.Competitors);
                cdl.SaveChanges();
            }
        }
    }
}