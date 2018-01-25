using System;
using Logic.TournamentManagement.Et;
using Logic.TournamentManagement.Pers;

namespace Logic.Database.DataLayer
{
    public class RankingDL
    {
        public void CreateRanking(RankingPers ranking)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Rankings.Add(ranking);
            cdl.SaveChanges();
        }

        public void SetWinner(String turniername, Guid compID)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            RankingPers rp = cdl.Rankings.Find(compID, turniername);
            rp.WonFinal = true;
            cdl.SaveChanges();
        }

        public RankingPers FindRanking(String tournament, Guid competitorID)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            return cdl.Rankings.Find(tournament, competitorID);
        }

        public void DeleteRankingPerCompetitor(Guid competitorID)
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Rankings.SqlQuery("delete from Rankings where CompetitorID = '" + competitorID.ToString() + "'");
            cdl.SaveChanges();
        }

        public void DeleteAllRankings()
        {
            DatabaseContextInstance cdl = DatabaseContext.GetContext();
            cdl.Rankings.RemoveRange(cdl.Rankings);
            cdl.SaveChanges();
        }
    }
}