using System.Collections.Generic;
using Logic.TournamentManagement;
using Logic.TournamentManagement.Execution;

namespace Logic
{
    public interface ITournament
    {
        void AddCompetitor(ICompetitor c);
        void StartTournament();
        void GetTournamentTree();
        bool CheckAmountCompetitor();

        /// <summary>
        /// Mit dieser Methode kann man die Tore für einen Satz setzen
        /// </summary>
        /// <param name="t1">Team 1</param>
        /// <param name="goalsT1">Tore von Team 1</param>
        /// <param name="t2">Team 2</param>
        /// <param name="goalsT2">Tore von Team 2</param>
        void SetGoalForCurrentSet(Team t1, int goalsT1, Team t2, int goalsT2);


        bool IsStarted { get; }
        bool IsFinished { get; }

        Dictionary<Team, int> Ranking { get; }


        string Name { get; }
        List<Team> TeamList { get; }
        IMatch CurrentMatch { get; }

        IGameSet GetCurrentSet();

        int AmountSets { get; }
        int AmountGoalsperSet { get; }
        IGameMode GameMode { get; }
        bool IsRanked { get; }
    }
}