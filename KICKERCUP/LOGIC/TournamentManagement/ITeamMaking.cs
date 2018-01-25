using System.Collections.Generic;

namespace Logic.TournamentManagement
{
    public interface ITeamMaking
    {
        /// <summary>
        /// Erstellt alle Teams des Turniers
        /// </summary>
        /// <param name="competitorList">Liste aller Spieler</param>
        /// <returns>Liste aller Teams</returns>
        List<Team> CreateAllTeams(List<ICompetitor> competitorList);

        /// <summary>
        /// Erstellt alle Teams anhand des SkillLevel des Spielers
        /// </summary>
        /// <param name="competitorList">Liste aller Spieler</param>
        /// <returns>Liste aller Teams</returns>
        List<Team> CreateAllTeamsBySkill(List<ICompetitor> competitorList);

        /// <summary>
        /// Erstellt alle Teams des Turniers
        /// </summary>
        /// <param name="competitorList">Liste aller Spieler</param>
        /// <returns>Liste aller Teams</returns>
        List<Team> CreateAllSoloTeams(List<ICompetitor> competitorList);
    }
}