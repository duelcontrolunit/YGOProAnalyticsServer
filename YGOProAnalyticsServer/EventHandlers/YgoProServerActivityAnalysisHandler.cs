using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.Events;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;

namespace YGOProAnalyticsServer.EventHandlers
{
    public class YgoProServerActivityAnalysisHandler : INotificationHandler<DataFromYgoProServerRetrieved>
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IServerActivityUpdater _updater;

        public YgoProServerActivityAnalysisHandler(YgoProAnalyticsDatabase db, IServerActivityUpdater updater)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));
        }

        public async Task Handle(DataFromYgoProServerRetrieved notification, CancellationToken cancellationToken)
        {
            var convertedDuelLogsListsDictionary = notification.ConvertedDuelLogs;

            foreach (var duelLogsWithTimeFrom in convertedDuelLogsListsDictionary)
            {
                var duelLogs = duelLogsWithTimeFrom.Value;
                await _updater.UpdateWithoutSavingChanges(duelLogs);
            }

            await _db.SaveChangesAsync();
        }
    }
}
