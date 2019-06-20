using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Events;

namespace YGOProAnalyticsServer.EventHandlers
{
    public class YgoProServerActivityAnalysisHandler : INotificationHandler<DataFromYgoProServerRetrieved>
    {
        readonly YgoProAnalyticsDatabase _db;

        public YgoProServerActivityAnalysisHandler(YgoProAnalyticsDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(DataFromYgoProServerRetrieved notification, CancellationToken cancellationToken)
        {
            var convertedDuelLogsListsDictionary = notification.ConvertedDuelLogs;
            var allStatistics = _db.ServerActivityStatistics.ToList();

            foreach (var duelLogsWithTimeFrom in convertedDuelLogsListsDictionary)
            {
                DateTime duelLogsFrom = duelLogsWithTimeFrom.Key;
                var duelLogs = duelLogsWithTimeFrom.Value;

               var statistics = allStatistics
                    .Where(x => x.FromDate.Date == duelLogsFrom.Date)
                    .FirstOrDefault();
                if(statistics == null)
                {
                    statistics = new ServerActivityStatistics(duelLogsFrom);
                    statistics.NumberOfGames += duelLogs.Count();
                    _db.ServerActivityStatistics.Add(statistics);
                }
                else
                {
                    statistics.NumberOfGames += duelLogs.Count();
                } 
            }

            await _db.SaveChangesAsync();
        }
    }
}
