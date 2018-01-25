using System;
using System.Collections.Generic;
using Logic.TournamentManagement.Execution;
using Logic.TournamentManagement.Impl;
using System.ComponentModel.DataAnnotations;

namespace Logic.TournamentManagement.Et
{
    /// <summary>
    /// Entität "Tournament"
    /// Steuer-Klasse die auf Komponente Plan und Execution zugreift
    /// </summary>
    public class Tournament : ITournament
    {
        #region Variablen Deklaration

        private readonly List<ICompetitor> _competitorList;
        private readonly ITeamMaking _teamMaking;

        #endregion

        #region Datenbank Variablen

        [Key] public string Name { get; set; }
        public List<Team> TeamList { set; get; }
        public IMatch CurrentMatch { set; get; }

        public IGameSet GetCurrentSet()
        {
            return CurrentMatch.GetCurrentGameSet;
        }


        [Required] public int AmountSets { get; set; }
        [Required] public int AmountGoalsperSet { get; set; }
        public IGameMode GameMode { get; set; }
        [Required] public bool IsRanked { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }


        /// <summary>
        /// Der Value gibt an wie viele Matches das Team bereits gewonnen hat;
        /// </summary>
        public Dictionary<Team, int> Ranking { get; set; }

        private delegate List<Team> TeamMakinDelegate(List<ICompetitor> competitorList);

        private TeamMakinDelegate teamMakinDelegate;

        #endregion


        //Leerer Konstruktor für das Entity Framework
        public Tournament()
        {
        }

        /// <summary>
        /// Diese Klasse steuert alle Komponenten des Turnier Managements an
        /// </summary>
        /// <param name="name">Name des Turniers</param>
        /// <param name="competitorList">Liste aller Teilnehmer</param>
        /// <param name="amountSets">Anzahl der Sätze pro Match muss ungerade sein</param>
        /// <param name="amountGoalsperSet">Anzahl der Tore pro Satz</param>
        /// <param name="gamemode">Spielmodus der gespielt werden soll</param>
        /// <param name="isRanked">True wenn das Turnier gewertet ist</param>
        public Tournament(string name, List<ICompetitor> competitorList, int amountSets, int amountGoalsperSet,
            IGameMode gamemode, bool isRanked)
        {
            //Prüfe ob amount sets eine gerade Zahl ist
            if (amountSets % 2 == 0 && GameMode is DoubleElimination)
            {
                throw new ArgumentOutOfRangeException("Amount Sets muss ungerade sein.");
            }

            Name = name;
            this._competitorList = competitorList;
            AmountSets = amountSets;
            AmountGoalsperSet = amountGoalsperSet;
            GameMode = gamemode;
            IsRanked = isRanked;
            _teamMaking = new TeamMaking();
            Ranking = new Dictionary<Team, int>();
            IsStarted = false;
            IsFinished = false;
            GameMode.Tournament = this;
            TeamList = new List<Team>();

            if (GameMode is DoubleElimination && !isRanked)
            {
                teamMakinDelegate = _teamMaking.CreateAllTeams;
            }
            else if (GameMode is DoubleElimination)
            {
                teamMakinDelegate = _teamMaking.CreateAllTeamsBySkill;
            }
            else if (GameMode is RankedTeamMatch)
            {
                teamMakinDelegate = _teamMaking.CreateAllTeams;
                amountSets = gamemode.AmountGameSets;
                amountGoalsperSet = gamemode.AmountGoalsPerSet;
            }
            else if (GameMode is RankedSoloMatch)
            {
                teamMakinDelegate = _teamMaking.CreateAllSoloTeams;
                AmountSets = gamemode.AmountGameSets;
                AmountGoalsperSet = gamemode.AmountGoalsPerSet;
            }
            else
            {
                throw new ArgumentException("Dieser Spielmodus wird leider noch nicht unterstützt");
            }
        }

        /// <summary>
        /// Füge Teilnehmer dem Turnier hinzu solange das Turnier noch nicht gestartet ist
        /// </summary>
        /// <param name="c">Teilnehmer der hinzugefügt wird</param>
        public void AddCompetitor(ICompetitor c)
        {
            // Teilnehmer können nur hinzugefügt werden wenn das Tournament noch nicht gestartet ist
            if (!IsStarted)
            {
                _competitorList.Add(c);
            }
        }

        public bool CheckAmountCompetitor()
        {
            bool result;
            if (TeamList == null)
            {
                result = false;
            }
            else
            {
                result = GameMode.CheckAmountTeams(TeamList);
            }

            return result;
        }

        public void GetTournamentTree()
        {
            foreach (KeyValuePair<Team, int> t in Ranking)
            {
                Console.WriteLine(t.Key + @" hat " + t.Value + @" Siege");
            }
        }


        /// <summary>
        /// Diese Methode startet das Turnier
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Die Anzahl der Competitor ist nicht korrekt.</exception>
        public void StartTournament()
        {
            //Erstelle Teams
            TeamList = teamMakinDelegate(_competitorList);

            foreach (Team t in TeamList)
            {
                Ranking.Add(t, 0);
            }

            if (!CheckAmountCompetitor())
            {
                throw new ArgumentOutOfRangeException(_competitorList.Count + " ist keine korrekte Teilnehmer Anzahl");
            }

            //Initialisiere GameMode
            GameMode.AddTeams(TeamList);
            GameMode.AmountGameSets = AmountSets;
            GameMode.AmountGoalsPerSet = AmountGoalsperSet;

            //Starte Turnier
            IsStarted = true;

            //Erstelle erstes Match
            CurrentMatch = GameMode.CreateMatch();
        }

        private void UpdateTournament()
        {
            if (IsStarted && !IsFinished)
            {
                //Sollte das aktuelle Match fertig sein wird es beendet und ein neues erstellt
                if (CurrentMatch.IsFinished)
                {
                    //Dem Match wird mitgeteilt wer Gewonnen und verloren hat
                    GameMode.SetWinner = CurrentMatch.Winner;
                    GameMode.SetLoser = CurrentMatch.Loser;

                    //Gewinner wird im Ranking aktualisiert
                    Ranking[CurrentMatch.Winner]++;

                    //Aufruf SkillLevel Berrechnung
                    //Das SkillLevel muss vorher gespeichert werden, da nach der ersten Berechnung sich
                    //eines der SkillLevel direkt verändert.
                    if (IsRanked)
                    {
                        int skillLevelLoser = CurrentMatch.Loser.NewSkillLevel;
                        int skillLevelWinner = CurrentMatch.Winner.NewSkillLevel;

                        CurrentMatch.Winner.CalcTeamSkillLevel(skillLevelLoser, true);
                        CurrentMatch.Loser.CalcTeamSkillLevel(skillLevelWinner, false);
                    }

                    //Neues Match wird erstellt
                    IMatch m = GameMode.CreateMatch();
                    if (m == null)
                    {
                        IsFinished = true;
                        //Turnier ist fertig oder es kann aktuell kein Match mehr erstellt werden (Fehler)
                    }
                    else
                    {
                        CurrentMatch = m;
                    }
                }

                //Wenn das Turnier beendet und gelistet ist so werden die gesammelten Skillpunkte auf die Spieler verteilt
                if (IsFinished && IsRanked)
                {
                    foreach (Team t in TeamList)
                    {
                        t.DistribiuteSkillLevel();
                    }
                }
            }
        }

        /// <summary>
        /// Mit dieser Methode kann man die Tore für einen Satz setzen
        /// </summary>
        /// <param name="t1">Team 1</param>
        /// <param name="goalsT1">Tore von Team 1</param>
        /// <param name="t2">Team 2</param>
        /// <param name="goalsT2">Tore von Team 2</param>
        public void SetGoalForCurrentSet(Team t1, int goalsT1, Team t2, int goalsT2)
        {
            if (!IsStarted)
            {
                throw new ArgumentOutOfRangeException("Das Turnier ist noch nicht gestartet.");
            }
            else if (goalsT1 < AmountGoalsperSet && goalsT2 < AmountGoalsperSet)
            {
                throw new ArgumentOutOfRangeException(
                    "Wenigstens eines der Teams muss genügend Tore haben um das Set zu gewinnen.");
            }
            else if (goalsT1 > AmountGoalsperSet || goalsT2 > AmountGoalsperSet)
            {
                throw new ArgumentOutOfRangeException("Eines der Teams hat eine ungültige Toranzahl.");
            }
            else if (IsStarted && (goalsT1 == AmountGoalsperSet || goalsT2 == AmountGoalsperSet))
            {
                GetCurrentSet().SetGoals(t1, goalsT1);
                GetCurrentSet().SetGoals(t2, goalsT2);
                UpdateTournament();
            }
        }

        public override string ToString()
        {
            return "Name: " + Name + " Spielmodus: " + GameMode + " Anzahl der Teams " + TeamList.Count;
        }
    }
}