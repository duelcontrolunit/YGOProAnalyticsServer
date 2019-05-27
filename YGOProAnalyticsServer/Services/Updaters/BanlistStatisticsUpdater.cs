using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServer.Services.Updaters
{
    public class BanlistStatisticsUpdater
    {
        YgoProAnalyticsDatabase _db;
        IDuelLogNameAnalyzer _duelLogNameAnalyzer;

        public BanlistStatisticsUpdater(YgoProAnalyticsDatabase db, IDuelLogNameAnalyzer duelLogNameAnalyzer)
        {
            _db = db;
            _duelLogNameAnalyzer = duelLogNameAnalyzer;
        }

        public void Analyze(DuelLog duelLog)
        {
            var banlist = _duelLogNameAnalyzer.GetBanlist(duelLog.Name, duelLog.DateOfTheEndOfTheDuel);
            var banlistStatistics = _db.BanlistStatistics.Where
                (x => x.DateWhenBanlistWasUsed.Date == duelLog.DateOfTheEndOfTheDuel.Date).FirstOrDefault()
                ?? BanlistStatistics.Create(duelLog.DateOfTheEndOfTheDuel.Date, banlist);
            banlistStatistics.IncrementHowManyTimesWasUsed();
            if (banlist.Statistics.Where
                (x => x.DateWhenBanlistWasUsed == banlistStatistics.DateWhenBanlistWasUsed) == null)
            {
                banlist.Statistics.Add(banlistStatistics);
            }
        }

    }
}
