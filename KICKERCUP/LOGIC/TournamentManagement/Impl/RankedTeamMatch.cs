using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Execution;
using Logic.TournamentManagement.Execution.Impl;

namespace Logic.TournamentManagement.Impl
{
    /// <summary>
    /// Ranglisten Spiel in dem Zwei Teams einzeln gegeneinander antreten.
    /// Es wird immer nur ein Satz gespielt und es sind 10 Tore nötig um zu gewinnen.
    /// </summary>
    public class RankedTeamMatch : IGameMode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GameModeGuid { get; set; }

        public Queue<Team> Winner { get; set; }
        public Queue<Team> Loser { get; set; }

        public int AmountGameSets
        {
            get { return 1; }
            set
            {
                //do nothing
            }
        }

        public int AmountGoalsPerSet
        {
            get { return 10; }
            set
            {
                //do nothing
            }
        }

        private bool _played = false;

        private List<Team> _teams;

        public Team SetWinner { get; set; }
        public Team SetLoser { get; set; }

        /// <summary>
        /// In diesem Turnier ist der Spielmodus implementiert.
        /// </summary>
        private ITournament _tournament;

        public ITournament Tournament
        {
            set
            {
                if (_tournament == null)
                {
                    _tournament = value;
                }
                else if (!_tournament.IsStarted)
                {
                    _tournament = value;
                }
            }
        }

        public RankedTeamMatch()
        {
            Winner = new Queue<Team>();
            Loser = new Queue<Team>();
        }

        public IMatch CreateMatch()
        {
            IMatch m = null;
            if (_tournament.IsStarted && _teams == null)
            {
                _teams = Winner.ToList();
            }

            if (_tournament != null && _tournament.IsStarted && !_played)
            {
                m = new Match(_teams.ElementAt(0), _teams.ElementAt(1), AmountGameSets, AmountGoalsPerSet);
                _played = true;
            }

            return m;
        }


        public List<Team> GetTournamentTree()
        {
            return _teams;
        }

        public bool CheckAmountTeams(List<Team> teamList)
        {
            bool result;
            if (teamList.Count == 2)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public void AddTeams(List<Team> teamList)
        {
            _teams = teamList;

            if (!_tournament.IsStarted)
            {
                foreach (Team t in teamList)
                {
                    Winner.Enqueue(t);
                }
            }
        }

        public override string ToString()
        {
            return "RankedTeamMatch";
        }
    }
}