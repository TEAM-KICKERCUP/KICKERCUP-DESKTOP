using System;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Pers;

namespace Logic.Database.DataLayer
{
    public class TournamentDL
    {
        /// <summary>
        /// Erstelle ein neues Torunament in der Datenbank
        /// </summary>
        /// <param name="tournament">Objekt des Toournaments das erstellt werden soll</param>
        public void CreateTournament(TournamentPers tournament)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Tournaments.Add(tournament);
            cdl.SaveChanges();
        }


        /// <summary>
        /// Update Tournament in der Datenbank
        /// </summary>
        /// <param name="tournament">Objekt des Tournaments das Aktualisiert werden soll</param>
        public void UpdateTournament(TournamentPers newTournament)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            TournamentPers oldTournament = cdl.Tournaments.Find(newTournament.Name);
            oldTournament.IsFinished = newTournament.IsFinished;
            cdl.SaveChanges();
        }

        /// <summary>
        /// Ein Turnier finden und zurückgeben
        /// </summary>
        public TournamentPers FindTournament(String name)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            return cdl.Tournaments.Find(name);
        }

        /// <summary>
        /// Lösche ein Tournament in der Datenbank
        /// </summary>
        /// <param name="name">Name des Tournaments das gelöscht werden soll</param>
        public void DeleteTournament(String name)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Tournaments.Remove(cdl.Tournaments.Find(name));
            cdl.SaveChanges();
        }
    }
}