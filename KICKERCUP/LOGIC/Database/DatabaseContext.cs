using System;
using System.Data.Entity;
using Logic.CompetitorManagement.Et;
using Logic.ClientManagement.Et;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Pers;

namespace Logic.Database
{
    public static class DatabaseContext
    {
        private static String DBConnectionString = ConnectionTool.GetValue<String>("DBConnectionString");

        public static DatabaseContextInstance GetContext()
        {
            if (DBConnectionString == null)
            {
                // Ohne Connection String
                return new DatabaseContextInstance();
            }
            else
            {
                // Mit Connection String
                return new DatabaseContextInstance(true);
            }
        }
    }

    public class DatabaseContextInstance : DbContext
    {
        //Konstruktor mit Connection String aus der GUI
        public DatabaseContextInstance(bool differentSignature) : base(
            ConnectionTool.GetValue<String>("DBConnectionString"))
        {
        }

        //Konstruktor ohne Connection String mit Datenbankname "KICKERCUP"
        public DatabaseContextInstance() : base("KICKERCUP")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Competitor> Competitors { get; set; }

        public virtual DbSet<TournamentPers> Tournaments { get; set; }

        public virtual DbSet<RankingPers> Rankings { get; set; }
    }
}