using System.Collections.Generic;
using Logic.TournamentManagement.Execution;

namespace Logic.TournamentManagement
{
    public interface IGameMode
    {
        /// <summary>
        /// Diese Methode erzeugt ein neues Match. 
        /// </summary>      
        /// <returns>Neues Match</returns>
        IMatch CreateMatch();

        /// <summary>
        /// Diese Methode gibt die Rangliste zurück. 
        /// </summary>
        /// <returns>Rangliste</returns>
        List<Team> GetTournamentTree();

        /// <summary>
        /// Diese Methode prüft ob die Anzahl der Competitor
        /// für diesen Spielmodus zulässig ist. 
        /// </summary>
        /// <returns>bool</returns>
        bool CheckAmountTeams(List<Team> teamList);

        /// <summary>
        /// Fügt die Liste der Teams in den Spielmodus ein.
        /// </summary>
        /// <param name="teamList">Liste der Teams die am Spielmodus teilnehmen</param>
        void AddTeams(List<Team> teamList);

        int AmountGameSets { get; set; }
        int AmountGoalsPerSet { get; set; }

        Team SetWinner { set; }
        Team SetLoser { set; }

        ITournament Tournament { set; }
    }
}