using System.Collections.Generic;
using System.Linq;

namespace Logic.TournamentManagement.Impl
{
    /// <summary>
    /// Teammaking das Teilnehmer der Reihe nach in Teams zuweist
    /// </summary>
    public class TeamMaking : ITeamMaking
    {
        public List<Team> CreateAllSoloTeams(List<ICompetitor> competitorList)
        {
            int amountTeams = competitorList.Count;
            Queue<ICompetitor> queue = new Queue<ICompetitor>(competitorList);
            List<Team> teamList = new List<Team>(amountTeams);
            for (int index = 0; index < amountTeams; index++)
            {
                teamList.Add(new Team(queue.Dequeue()));
            }

            return teamList;
        }

        /// <summary>
        /// Erstellt alle Teams und gibt diese zurück
        /// </summary>
        /// <param name="competitorList">Teilnehmerliste</param>
        /// <returns>Liste aller Teams</returns>
        public List<Team> CreateAllTeams(List<ICompetitor> competitorList)
        {
            int amountTeams = competitorList.Count / 2;
            Queue<ICompetitor> queue = new Queue<ICompetitor>(competitorList);
            List<Team> teamList = new List<Team>(amountTeams);
            for (int index = 0; index < amountTeams; index++)
            {
                teamList.Add(new Team(queue.Dequeue(), queue.Dequeue()));
            }

            return teamList;
        }

        /// <summary>
        /// Erstellt alle Teams anhand des SkillLevel des Spielers
        /// </summary>
        /// <param name="competitorList">Liste aller Spieler</param>
        /// <returns>Liste aller Teams</returns>
        public List<Team> CreateAllTeamsBySkill(List<ICompetitor> competitorList)
        {

            competitorList.Sort();
            int size = competitorList.Count - 1;
            int amountTeams = competitorList.Count / 2;
            List<Team> teamList = new List<Team>(amountTeams);
            for (int index = 0; index < amountTeams; index++)
            {
                //Der beste wird mit dem schlechtesten in ein Team gesteckt
                teamList.Add(new Team(competitorList.ElementAt(index), competitorList.ElementAt(size--)));
            }

            return teamList;
        }
    }
}