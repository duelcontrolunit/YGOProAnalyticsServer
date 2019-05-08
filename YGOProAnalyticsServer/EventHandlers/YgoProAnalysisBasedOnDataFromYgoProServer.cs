using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Events;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServer.EventHandlers
{
    public class YgoProAnalysisBasedOnDataFromYgoProServer : INotificationHandler<DataFromYgoProServerRetrieved>
    {
        IDuelLogNameAnalyzer _duelLogNameAnalyzer;
        YgoProAnalyticsDatabase _db;

        public async Task Handle(DataFromYgoProServerRetrieved notification, CancellationToken cancellationToken)
        {
            foreach (var duelLogsKeyValue in notification.ConvertedDuelLogs)
            {
                var listOfDuelLogsFromOneDay = duelLogsKeyValue.Value;
                var dateFromDuelLogs = duelLogsKeyValue.Key;

                foreach (var duelLog in listOfDuelLogsFromOneDay)
                {
                    var banlist = _duelLogNameAnalyzer.GetBanlist(duelLog.Name, duelLog.DateOfTheEndOfTheDuel);
                    var banlistStatisticsFromOneDay = banlist.Statistics.FirstOrDefault(x => x.DateWhenBanlistWasUsed == dateFromDuelLogs);

                    if (banlistStatisticsFromOneDay == null)
                    {
                        banlistStatisticsFromOneDay = BanlistStatistics.Create(dateFromDuelLogs, banlist);
                        banlist.Statistics.Add(banlistStatisticsFromOneDay);
                    }
                    banlistStatisticsFromOneDay.IncrementHowManyTimesWasUsed();
                    
                }
            }
        }
    }
}
