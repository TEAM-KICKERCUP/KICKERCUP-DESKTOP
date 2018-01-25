using System.Collections.Generic;
using Logic.TournamentManagement.Et;

namespace Logic.TournamentManagement
{
    /// <summary>
    /// Diese Klasse ist die Factory für das gesamet TournamentManagement. 
    /// </summary>
    public class TournamentFactory
    {
        /// <summary>
        /// Diese Methode erzeugt ein neues Turnier. 
        /// </summary>
        /// <param name="name">Name des Turniers</param>
        /// <param name="competitorList">Liste der Teilnehmer</param>
        /// <param name="amountSets">Anzahl der Sätze</param>
        /// <param name="amountGoalsperSet">Anzahl der Tore pro Set</param>
        /// <param name="gamemode">Spielmodus des Turniers</param>
        /// <param name="isRanked">true wenn das Turnier gewertet ist</param>
        /// <returns>neues Turnier</returns>
        public ITournament CreateTournament(string name, List<ICompetitor> competitorList, int amountSets,
            int amountGoalsperSet, IGameMode gamemode, bool isRanked)
        {
            return new Tournament(name, competitorList, amountSets, amountGoalsperSet, gamemode, isRanked);
        }
    }
}