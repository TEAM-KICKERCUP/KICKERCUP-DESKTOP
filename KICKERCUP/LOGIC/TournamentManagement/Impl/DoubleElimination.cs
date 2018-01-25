using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Execution;
using Logic.TournamentManagement.Execution.Impl;

namespace Logic.TournamentManagement.Impl
{
    public class DoubleElimination : IGameMode
    {
        #region Variablen Deklaration

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GameModeGuid { get; set; }

        /// <summary>
        /// Warteschlange des Verlierer Teams
        /// </summary>
        public Queue<Team> Winner { get; set; }

        /// <summary>
        /// Warteschlange des Verlierer Baums
        /// </summary>
        public Queue<Team> Loser { get; set; }

        private int _amountGameSets;
        private int _amountGoalsperSet;

        /// <summary>
        /// In diesem Turnier ist der Spielmodus implementiert.
        /// </summary>
        private ITournament _tournament;

        /// <summary>
        /// loserHistory merkt sich welche Teams bereits ein Spiel verloren haben
        /// </summary>
        private List<Team> loserHistory;

        #endregion

        #region Aussensicht der Attribute

        /// <summary>
        /// Anzahl der Sätze die gespielt werden sollen
        /// </summary>
        public int AmountGameSets
        {
            get => _amountGameSets;
            set
            {
                if (_tournament != null && !_tournament.IsStarted)
                {
                    _amountGameSets = value;
                }
            }
        }

        /// <summary>
        /// Anzahl der Tore die benötigt werden um einen Satz zu gewinnen
        /// </summary>
        public int AmountGoalsPerSet
        {
            get => _amountGoalsperSet;
            set
            {
                if (_tournament != null && !_tournament.IsStarted)
                {
                    _amountGoalsperSet = value;
                }
            }
        }

        /// <summary>
        /// Teams die das vorherige Match gewonnen haben werden dem Gewinner Baum hinzugefügt und noch nie verloren hat
        /// Wenn das Team allerdings schon einmal verloren hat wird es nur dem Gewinner Baum hinzugefügt
        /// </summary>
        public Team SetWinner
        {
            set
            {
                if (_tournament != null && _tournament.IsStarted && !loserHistory.Contains(value))
                {
                    Winner.Enqueue(value);
                }
                else if (_tournament != null && _tournament.IsStarted && loserHistory.Contains(value))
                {
                    Loser.Enqueue(value);
                }
            }
        }

        /// <summary>
        /// Teams die das vorherige Match verloren haben und zuvor noch keines verloren haben werden dem Loser Baum hinzugefügt
        /// </summary>
        public Team SetLoser
        {
            set
            {
                if (_tournament != null && _tournament.IsStarted && !loserHistory.Contains(value))
                {
                    loserHistory.Add(value);
                    Loser.Enqueue(value);
                }
            }
        }

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

        #endregion

        //Drei verschiedene Konstruktoren je nachdem wie viele der Attribute bereits bekannt sind, fehlende Attribute müssen
        //bis zum Start des Turniers mit addParam nachgetragen werden

        #region Konstuktor

        /// <summary>
        /// Erstellt einen neuen Spielmodus:
        /// Bei der Double Elimination scheidet ein Teilnehmer erst nach der zweiten Niederlage aus dem Turnier aus. 
        /// </summary>
        public DoubleElimination()
        {
            Winner = new Queue<Team>();
            Loser = new Queue<Team>();
            loserHistory = new List<Team>();
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Bei DoubleElimination muss die Anzahl der Teams gleich 2^x sein.
        /// </summary>
        /// <param name="teamList">Team Liste</param>
        /// <returns></returns>
        public bool CheckAmountTeams(List<Team> teamList)
        {
            //2^7 = 128
            double x = 0;
            for (double i = 1; x <= 128; i++)
            {
                x = Math.Pow(2, i);
                if (Math.Abs(teamList.Count - x) < 0.5)
                {
                    return true;
                }
            }

            return false;
        }

        //** WICHTIGE FRAGE ** würfelt die Listen erstellung die ursprücngliche Queue durcheinander?
        //Eventuell ist es besser den Baum als Queue zurückzugeben.
        public List<Team> GetTournamentTree() => Winner.ToList();

        /// <summary>
        /// Fügt Weitere Teams dem Turnierbaum hinzu
        /// </summary>
        /// <param name="teamList"></param>
        public void AddTeams(List<Team> teamList)
        {
            if (!_tournament.IsStarted)
            {
                foreach (Team t in teamList)
                {
                    Winner.Enqueue(t);
                }
            }
        }

        /// <summary>
        /// create Match gibt das nächste Match zurück. Sollte das Tournament noch nicht gestartet sein oder das Finale bereits erstellt ist die rückgabe null.
        /// </summary>
        /// <returns>return null wenn Tournament noch nicht gestartet oder Finale bereits gespielt. Sonst Rückgabe eines neuen Matches</returns>
        public IMatch CreateMatch()
        {
            IMatch match;
            if (_tournament.IsStarted && Winner.Count >= Loser.Count && Winner.Count >= 2)
            {
                match = new Match(Winner.Dequeue(), Winner.Dequeue(), _amountGameSets, _amountGoalsperSet);
            }
            else if (_tournament.IsStarted && Winner.Count < Loser.Count && Loser.Count >= 2)
            {
                match = new Match(Loser.Dequeue(), Loser.Dequeue(), _amountGameSets, _amountGoalsperSet);
            }
            else if (_tournament.IsStarted && Loser.Count == 1 && Winner.Count == 1)
            {
                match = new Match(Winner.Dequeue(), Loser.Dequeue(), _amountGameSets, _amountGoalsperSet);
            }

            else
            {
                match = null;
            }

            return match;
        }

        public override string ToString()
        {
            return "Double Elimination";
        }

        #endregion
    }
}