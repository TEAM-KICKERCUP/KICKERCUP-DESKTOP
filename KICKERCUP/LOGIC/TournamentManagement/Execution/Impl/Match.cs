using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logic.TournamentManagement.Execution.Impl
{
    public class Match : IMatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MatchGuid { get; set; }

        //Die Speicherung der Teams ist für die SkillLevel berrechnung relevant
        private readonly Team _teamA;
        private readonly Team _teamB;

        /// <summary>
        /// Anzahl der Siege von teamA
        /// </summary>
        private int _winsTeamA;

        /// <summary>
        /// Anzahl der Siege von teamB
        /// </summary>
        private int _winsTeamB;

        /// <summary>
        /// Ist das Match bereits beendet? Ruft implizit Update() auf
        /// </summary>
        private bool _isFinished;

        public bool IsFinished
        {
            get
            {
                UpdateMatch();
                return _isFinished;
            }
        }


        /// <summary>
        /// Gibt Gewinner zurück
        /// Sollte noch nicht alle Sets beendet sein, ist die Rückgabe null
        /// </summary>
        public Team Winner
        {
            get
            {
                UpdateMatch();
                Team t;
                if (!_isFinished)
                {
                    t = null;
                }
                else if (_winsTeamA > _winsTeamB)
                {
                    t = _teamA;
                }
                else
                {
                    t = _teamB;
                }

                return t;
            }
        }

        public Team Loser
        {
            get
            {
                Team t;
                if (Winner == null)
                {
                    t = null;
                }
                else if (_teamB == Winner)
                {
                    t = _teamA;
                }
                else
                {
                    t = _teamB;
                }

                return t;
            }
        }

        private readonly int _amountGameSets;

        /// <summary>
        /// Queue der Sets die gespielt werden muss
        /// </summary>
        private Queue<IGameSet> setQueue;

        /// <summary>
        /// Aussensicht der Queue
        /// </summary>
        public IGameSet GetCurrentGameSet
        {
            get
            {
                UpdateMatch();
                return setQueue.Peek();
            }
        }

        /// <summary>
        /// Erstelle ein neues Match
        /// </summary>
        /// <param name="teamA"></param>
        /// <param name="teamB"></param>
        /// <param name="amountSets"></param>
        /// <param name="amountGoalsPerSet"></param>
        public Match(Team teamA, Team teamB, int amountSets, int amountGoalsPerSet)
        {
            this._teamA = teamA;
            this._teamB = teamB;
            _amountGameSets = amountSets;
            _isFinished = false;
            setQueue = new Queue<IGameSet>();

            //AmountSets und AmountGoalsPerSet darf nicht null sein
            if (amountSets <= 0 || amountGoalsPerSet <= 0)
            {
                throw new Exception("amountSet oder amountGoalsPerSet darf nicht 0 oder kleiner sein");
            }

            for (var i = 0; i < amountSets; i++)
            {
                setQueue.Enqueue(new GameSet(amountGoalsPerSet, teamA, teamB));
            }

            _winsTeamA = 0;
            _winsTeamB = 0;
        }

        /// <summary>
        /// Updatet alle Attribute von Match. Speziell wenn ein Set abgeschlossen ist sollte diese Methode aufgerufen werden.
        /// </summary>
        private void UpdateMatch()
        {
            while (setQueue.Count > 0 && setQueue.Peek().IsFinished)
            {
                if (setQueue.Peek().GetGoals(_teamA) > setQueue.Peek().GetGoals(_teamB))
                {
                    _winsTeamA++;
                }
                else
                {
                    _winsTeamB++;
                }

                setQueue.Dequeue();
            }

            if (setQueue.Count == 0 && _winsTeamA + _winsTeamB == _amountGameSets)
            {
                _isFinished = true;
            }
        }

        public List<Team> GetTeams()
        {
            return new List<Team>
            {
                _teamA,
                _teamB
            };
        }

        public int GetWinExpectationThatTeamAWins()
        {
            return (int) (_teamA.CalcWinExpectation(_teamB.NewSkillLevel) * 100);
        }

        public int GetWinExpectationThatTeamBWins()
        {
            return (int) (_teamB.CalcWinExpectation(_teamA.NewSkillLevel) * 100);
        }

        public override string ToString()
        {
            return "Team A " + _teamA + " mit " + _winsTeamA + " Siegen und " + @" 
Team B " + _teamB + " mit " + _winsTeamB + " Siegen.";
        }
    }
}