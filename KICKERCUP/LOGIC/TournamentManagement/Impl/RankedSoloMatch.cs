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
    /// Ranglisten Spiel in dem Zwei Spieler einzeln gegeneinander antreten.
    /// Es wird immer nur ein Satz gespielt und es sind 10 Tore nötig um zu gewinnen.
    /// </summary>
    public class RankedSoloMatch : IGameMode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GameModeGuid { get; set; }

        /// <summary>
        /// Winner wird in dieser Implementierung nicht benötigt
        /// </summary>
        public Queue<Team> Winner { get; set; }

        /// <summary>
        /// Loser wird in dieser Implementierung nicht benötigt
        /// </summary>
        public Queue<Team> Loser { get; set; }

        /// <summary>
        /// In diesem Spielmodus wird immer nur ein Satz gespielt
        /// </summary>
        public int AmountGameSets
        {
            get { return 1; }
            set
            {
                //do nothing
            }
        }

        /// <summary>
        /// Die Anzahl der Tore pro Satz sind immer 10 in diesem Spielmodus
        /// </summary>
        public int AmountGoalsPerSet
        {
            get { return 10; }
            set
            {
                //do nothing
            }
        }

        /// <summary>
        /// _played gibt an ob das Match bereits gespielt wurde
        /// </summary>
        private bool _played = false;

        /// <summary>
        /// _teams enthält die Teams die hier gegeinander spielen
        /// </summary>
        private List<Team> _teams;

        /// <summary>
        /// SetWinner enthält den Gewinner des Matches
        /// </summary>
        public Team SetWinner { get; set; }

        /// <summary>
        /// SetLoser enthält den Verlierer des Matches
        /// </summary>
        public Team SetLoser { get; set; }

        /// <summary>
        /// In diesem Turnier ist der Spielmodus implementiert.
        /// Dies ist die innensicht des Tournaments
        /// </summary>
        private ITournament _tournament;

        /// <summary>
        /// Tournament enthält das Turnier in dem der Spielmodus gespielt wird
        /// </summary>
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

        /// <summary>
        /// Konstruktor der Klasse RankedSoloMatch, sie initialsiert Winner und Loser
        /// </summary>
        public RankedSoloMatch()
        {
            Winner = new Queue<Team>();
            Loser = new Queue<Team>();
        }

        /// <summary>
        /// CreateMatch enthält die Strategie mit der das nächste Match erstellt wird
        /// </summary>
        /// <returns>Neues Match</returns>
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

        /// <summary>
        /// Getter für die TeamList
        /// </summary>
        /// <returns>TeamListe</returns>
        public List<Team> GetTournamentTree()
        {
            return _teams;
        }

        /// <summary>
        /// Überprüft die Anzahl der Teams in diesem Spielmodus
        /// </summary>
        /// <param name="teamList">Liste aller Teams</param>
        /// <returns>true wenn die Anzahl korrekt ist, false wenn die Anzahl der Teams nicht korrekt ist</returns>
        public bool CheckAmountTeams(List<Team> teamList)
        {
            bool result;
            if (teamList.Count == 2 && teamList.ElementAt(0).SpielerListe != null &&
                teamList.ElementAt(1).SpielerListe != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Fügt ein Team der Teamliste hinzu
        /// </summary>
        /// <param name="teamList">iste der Teams die hinzugefügt werden soll</param>
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
            return "RankedSoloMatch";
        }
    }
}