using System;

namespace Logic
{
    public interface ICompetitor
    {
        /// <summary>
        /// Mit dieser Methode kann das SkillLevel für 1vs1 berrechnet werden
        /// </summary>
        /// <param name="enemySkillLevel">SkillLevel des gegnerischen Teams</param>
        /// <param name="isWon">true wenn gewonnen und false wenn verloren</param>
        /// <param name="MAX_DIF">Maximale SkillLevel Differenz zweier Teams die belohnt oder bestraft wird</param>
        /// <param name="WEIGHTING">Gewichtung des Spiels,
        ///  sie ist üblicherweise 20, bei Top-Spielern 10, 
        ///  bei weniger als 30 gewerteten Partien 40, für Jugendspieler auch 40</param>
        void CalcSkillLevel(int enemySkillLevel, bool isWon, double MAX_DIF, double WEIGHTING);

        /// <summary>
        /// Aktualisiert das SkillLevel des Spielers
        /// </summary>
        /// <param name="skillChange">Veränderung des SkillLevels, kann sowohl negativ als auch positiv sein</param>
        void SetSkillLevel(int skillChange);

        int SkillLevel { get; }

        String Visibility { get; set; }

        Guid CompetitorID { get; }
    }
}