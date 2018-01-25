using Logic.CompetitorManagement.Et;
using Logic.CompetitorManagement.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace Logic.TournamentManagement
{
    /// <summary>
    /// Klasse Team. Jedes Team hat genau zwei Competitor
    /// </summary>
    public class Team : IComparable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TeamGuid { get; set; }

        /// <summary>
        /// Maximale SkillLevel Differenz zweier Teams die belohnt oder bestraft wird
        /// </summary>
        private const double MaxDif = 400;

        /// <summary>
        /// Gewichtung des Spiels,
        ///  sie ist üblicherweise 20, bei Top-Spielern 10, 
        ///  bei weniger als 30 gewerteten Partien 40, für Jugendspieler auch 40
        /// </summary>
        private const double Weighting = 40;

        public IList<ICompetitor> SpielerListe { get; }

        public int SkillLevel => _skillLevel;
        public int NewSkillLevel => _newSkillLevel;

        private int _skillLevel;
        private int _newSkillLevel;

        /// <summary>
        /// Für alle TeamMatches
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        public Team(ICompetitor c1, ICompetitor c2)
        {
            SpielerListe = new List<ICompetitor>
            {
                c1,
                c2
            };
            CalcTeamSkillLevel();
        }

        public Team(ICompetitor c)
        {
            SpielerListe = new List<ICompetitor> {c};
            CalcTeamSkillLevel();
        }

        public override string ToString()
        {
            if (SpielerListe.Count == 2)
            {
                return SpielerListe.ElementAt(0) + "\n" + SpielerListe.ElementAt(1);
            }
            else
            {
                return SpielerListe.ElementAt(0) + "";
            }
        }

        /// <summary>
        /// interne Methode um das eigene SkillLevel zu berrechnen
        /// </summary>
        private void CalcTeamSkillLevel()
        {
            if (SpielerListe.Count == 2)
            {
                //berrechne SkillLevel des Teams
                _skillLevel = (SpielerListe.ElementAt(0).SkillLevel + SpielerListe.ElementAt(1).SkillLevel) / 2;
            }
            else
            {
                _skillLevel = SpielerListe.ElementAt(0).SkillLevel;
            }

            _newSkillLevel = SkillLevel;
        }


        /// <summary>
        /// Berrechnet SkillLevel des Teams
        /// </summary>
        /// <param name="enemySkillLevel">SkillLevel des gegnerischen Teams</param>
        /// <param name="isWon">true wenn gewonnen und false wenn verloren</param>
        public void CalcTeamSkillLevel(int enemySkillLevel, bool isWon)
        {
            //Quelle https://de.wikipedia.org/wiki/Elo-Zahl
            //Erwartungswert des Matches
            double expResult = CalcWinExpectation(enemySkillLevel);
            double result = 0;
            if (isWon)
            {
                result = 1;
            }

            _newSkillLevel = Convert.ToInt32(_skillLevel + Weighting * (result - expResult));
        }

        public double CalcWinExpectation(int enemySkillLevel)
        {
            double expResult;
            if (enemySkillLevel - _skillLevel > MaxDif)
            {
                expResult = 1 / (1 + Math.Pow(10, -1));
            }
            else if (enemySkillLevel - _skillLevel < -MaxDif)
            {
                expResult = 1 / (1 + Math.Pow(10, 1));
            }
            else
            {
                expResult = 1 / (1 + Math.Pow(10, (enemySkillLevel - _skillLevel) / MaxDif));
            }

            return expResult;
        }

        /// <summary>
        /// Verteilt das Gewonnene / Verlorene gleichmäßig SkillLevel auf die beiden Spieler
        /// </summary>
        public void DistribiuteSkillLevel()
        {
            CompetitorIMPL cimpl = new CompetitorIMPL();
            int distributedSkillLevel = NewSkillLevel - SkillLevel;
            if (SpielerListe.Count == 2)
            {
                SpielerListe.ElementAt(0).SetSkillLevel(distributedSkillLevel / 2);
                SpielerListe.ElementAt(1).SetSkillLevel(distributedSkillLevel / 2);

                Competitor c1 = (Competitor) SpielerListe.ElementAt(0);
                Competitor c2 = (Competitor) SpielerListe.ElementAt(1);
                cimpl.UpdateCompetitor(c1.CompetitorID, (distributedSkillLevel / 2));
                cimpl.UpdateCompetitor(c2.CompetitorID, (distributedSkillLevel / 2));
            }
            else
            {
                SpielerListe.ElementAt(0).SetSkillLevel(distributedSkillLevel);
                Competitor c1 = (Competitor) SpielerListe.ElementAt(0);
                cimpl.UpdateCompetitor(c1.CompetitorID, (distributedSkillLevel / 2));
            }
        }

        public int CompareTo(object obj)
        {
            int result = 0;
            if (obj.GetType() == GetType())
            {
                Team t = (Team) obj;
                if (t.NewSkillLevel < NewSkillLevel)
                {
                    result = 1;
                }
                else if (t.NewSkillLevel > NewSkillLevel)
                {
                    result = -1;
                }
            }

            return result;
        }
    }
}