using System.Collections.Generic;

namespace Logic.TournamentManagement.Execution
{
    public interface IMatch
    {
        Team Winner { get; }
        Team Loser { get; }
        IGameSet GetCurrentGameSet { get; }
        bool IsFinished { get; }
        List<Team> GetTeams();

        int GetWinExpectationThatTeamAWins();
        int GetWinExpectationThatTeamBWins();
    }
}