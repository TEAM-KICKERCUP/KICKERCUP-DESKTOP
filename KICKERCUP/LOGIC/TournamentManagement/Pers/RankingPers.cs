using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.TournamentManagement.Pers
{
    [Table("Rankings")]
    public class RankingPers
    {
        [Key, Required, Column(Order = 0)] public Guid CompetitorID { get; set; }
        [Key, Required, Column(Order = 1)] public String Tournament { get; set; }
        public int Wins { get; set; }
        public bool WonFinal { get; set; }

        [NotMapped] public String CVornameNachname { get; set; }

        public RankingPers(String tournament, Guid competitorID, int wins, bool wonFinal)
        {
            this.Tournament = tournament;
            this.CompetitorID = competitorID;
            this.Wins = wins;
            this.WonFinal = wonFinal;
        }

        //Empty constructor is required by Entity Framework for some cases
        public RankingPers()
        {
        }
    }
}