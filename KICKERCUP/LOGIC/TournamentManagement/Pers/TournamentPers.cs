using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.TournamentManagement.Pers
{
    [Table("Tournaments")]
    public class TournamentPers
    {
        [Key] public String Name { get; set; }
        [Required] public String Username { get; set; }
        [Required] public String Gamemode { get; set; }
        [Required] public int AmountSets { get; set; }
        [Required] public int AmountGoals { get; set; }
        [Required] public bool IsRanked { get; set; }
        [Required] public bool IsFinished { get; set; }

        [Required] public DateTime Date { get; set; }

        //public ICollection<RankingPers> Rankings { get; set; }

        public TournamentPers(String name, String username, String gamemode, int amountSets, int amountGoals,
            bool isRanked, bool isFinished)
        {
            this.Name = name;
            this.Username = username;
            this.Gamemode = gamemode;
            this.AmountGoals = amountGoals;
            this.AmountSets = amountSets;
            this.IsRanked = isRanked;
            this.IsFinished = isFinished;
            this.Date = DateTime.Now;
        }

        //Empty constructor is required by Entity Framework for some cases
        public TournamentPers()
        {
        }
    }
}