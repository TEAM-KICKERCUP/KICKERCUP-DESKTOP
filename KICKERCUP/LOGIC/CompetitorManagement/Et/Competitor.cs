using Logic.ClientManagement.Et; // ´neu
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logic.CompetitorManagement.Et
{
    /// <summary>
    /// Entität "Competitor", also Turnierteilnehmer, besitzt ID (automatisch), Vorname, Name, 
    /// Geschlecht, Sichtbarkeit und Skill Level.
    /// Ueber die GUI nur folgende Eingaben moeglich:
    /// Für Geschlecht: "männlich" und "weiblich"
    /// Für Sichtbarkeit: "global" und "lokal"
    /// Das Skill Level wird berechnet und hat einen Startwert von 1500.
    /// </summary>
    public class Competitor : ICompetitor, IComparable
    {
        private int skillLevel;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CompetitorID { get; set; }

        [Required] public String Name { get; set; }
        [Required] public String Surname { get; set; }
        [Required] public String Gender { get; set; }
        [Required] public String Visibility { get; set; }

        [Required]
        public int SkillLevel
        {
            get => skillLevel;
            set => skillLevel = value;
        }

        //Foreign Key for Client
        [Required] public String Username { get; set; }
        public Client Client { get; set; }

        //Leerer Konstruktor in manchen Faellen vom Entity Framework benoetigt
        public Competitor()
        {
        }

        //Aufruf ohne Username fuer das Updaten von Teilnehmern
        public Competitor(String name, String surname, String gender, int skillLevel, String visibility)
        {
            this.Name = name;
            this.Surname = surname;
            this.Gender = gender;
            this.SkillLevel = skillLevel;
            this.Visibility = visibility;
        }

        //Aufruf mit Username fuer das Hinzufuegen von Teilnehmern
        public Competitor(String name, String surname, String gender, int skillLevel, String visibility,
            String username)
        {
            this.Name = name;
            this.Surname = surname;
            this.Gender = gender;
            this.skillLevel = skillLevel;
            this.Visibility = visibility;
            this.Username = username;
        }

        /// <summary>
        /// Mit dieser Methode kann das SkillLevel für 1vs1 berrechnet werden
        /// </summary>
        /// <param name="enemySkillLevel">SkillLevel des gegnerischen Teams</param>
        /// <param name="isWon">true wenn gewonnen und false wenn verloren</param>
        /// <param name="MAX_DIF">Maximale SkillLevel Differenz zweier Teams die belohnt oder bestraft wird</param>
        /// <param name="WEIGHTING">Gewichtung des Spiels,
        ///  sie ist üblicherweise 20, bei Top-Spielern 10, 
        ///  bei weniger als 30 gewerteten Partien 40, für Jugendspieler auch 40</param>
        public void CalcSkillLevel(int enemySkillLevel, bool isWon, double MAX_DIF, double WEIGHTING)
        {
            //Quelle https://de.wikipedia.org/wiki/Elo-Zahl
            //Erwartungswert des Matches
            double expResult;
            if (enemySkillLevel - skillLevel > MAX_DIF)
            {
                expResult = 1 / (1 + Math.Pow(10, -1));
            }
            else if (enemySkillLevel - skillLevel < -MAX_DIF)
            {
                expResult = 1 / (1 + Math.Pow(10, 1));
            }
            else
            {
                expResult = 1 / (1 + Math.Pow(10, (enemySkillLevel - skillLevel) / MAX_DIF));
            }

            double result = 0;
            if (isWon)
            {
                result = 1;
            }

            skillLevel = Convert.ToInt32(skillLevel + WEIGHTING * (result - expResult));
        }

        public override string ToString()
        {
            return Name;
        }

        public void SetSkillLevel(int skillChange)
        {
            skillLevel = skillLevel + skillChange;
        }

        public int CompareTo(object obj)
        {
            int result = 0;
            if (obj.GetType() == this.GetType())
            {
                Competitor c = (Competitor) obj;
                if (c.SkillLevel < this.SkillLevel)
                {
                    result = 1;
                }
                else if (c.SkillLevel > this.SkillLevel)
                {
                    result = -1;
                }
            }

            return result;
        }
    }
}