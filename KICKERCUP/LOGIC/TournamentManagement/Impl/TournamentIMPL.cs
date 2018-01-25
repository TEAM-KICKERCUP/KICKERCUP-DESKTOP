//using System;
//using System.Collections.Generic;
//using Logic.TournamentManagement.Et;
//using Logic.Database.DataLayer;

//namespace Logic.TournamentManagement.Impl
//{
//    public class TournamentImpl
//    {
//        private readonly TournamentDL _cd1 = new TournamentDL();

//        public void AddTournament(string name, List<ICompetitor> competitorList, int amountSets, int amountGoalsperSet, IGameMode gamemode, bool isRanked)
//        {
//            Tournament t = new Tournament(name, competitorList, amountSets, amountGoalsperSet, gamemode, isRanked);
//            _cd1.CreateTournament(t);
//        }

//        public void UpdateTournament(string name, List<ICompetitor> competitorList, int amountSets, int amountGoalsperSet, IGameMode gamemode, bool isRanked)
//        {
//            Tournament t = new Tournament(name, competitorList, amountSets, amountGoalsperSet, gamemode, isRanked);
//            _cd1.UpdateTournament(t);
//        }

//        public Tournament FindTournament(String name)
//        {
//            return _cd1.FindTournament(name);
//        }

//        public void DeleteTournament(String name)
//        {
//            _cd1.DeleteTournament(name);
//        }
//    }
//}

