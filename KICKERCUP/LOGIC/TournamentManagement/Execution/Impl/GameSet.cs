using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace Logic.TournamentManagement.Execution.Impl
{
    public class GameSet : IGameSet


    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GameSetGuid { get; set; }

        private readonly Dictionary<Team, int> _teams;

        private readonly int _amountGoalsPerSet;

        /// <summary>
        /// Ist dieses Set schon beendet?
        /// Wenn Team1 EXOR Team2 die benötigten Tore erreicht hat ist das Spiel beendet
        /// UND bei beiden Teams muss ein Wert eingetragen sein
        /// </summary>
        public bool IsFinished
        {
            get
            {
                if (_teams != null && (_teams.ElementAt(0).Value != -1 &&
                                       _teams.ElementAt(1).Value != -1 &&
                                       (_teams.ElementAt(0).Value == _amountGoalsPerSet ^
                                        _teams.ElementAt(1).Value == _amountGoalsPerSet)))
                {
                    return true;
                }

                if (_teams != null && (_teams.ElementAt(0).Value == _teams.ElementAt(1).Value &&
                                       _teams.ElementAt(0).Value == _amountGoalsPerSet))
                {
                    throw new ArgumentOutOfRangeException("Es gibt kein Unentschieden!");
                }

                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="amountGoalsPerSet"></param>
        /// <param name="t1">Team 1</param>
        /// <param name="t2">Team 2</param>
        public GameSet(int amountGoalsPerSet, Team t1, Team t2)
        {
            this._amountGoalsPerSet = amountGoalsPerSet;
            _teams = new Dictionary<Team, int>
            {
                {t1, -1},
                {t2, -1}
            };
        }


        public int GetGoals(Team t)
        {
            return _teams[t];
        }


        public bool SetGoals(Team t, int goals)
        {
            if (!IsFinished)
            {
                bool isGoalAmountCorrect;
                if (goals > _amountGoalsPerSet)
                {
                    isGoalAmountCorrect = false;
                }
                else
                {
                    _teams[t] = goals;
                    isGoalAmountCorrect = true;
                }

                return isGoalAmountCorrect;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Das Set ist bereits beendet!");
            }
        }
    }
}