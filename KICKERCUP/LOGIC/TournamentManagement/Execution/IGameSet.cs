namespace Logic.TournamentManagement.Execution
{
    public interface IGameSet
    {
        int GetGoals(Team t);
        bool SetGoals(Team t, int goals);

        /// <summary>
        /// Ist das Set bereits beendet? True = Ja, False = Nein
        /// </summary>
        bool IsFinished { get; }
    }
}