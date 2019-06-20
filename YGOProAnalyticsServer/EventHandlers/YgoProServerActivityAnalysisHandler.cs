using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Events;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;

namespace YGOProAnalyticsServer.EventHandlers
{
    /// <summary>
    /// Initialize <see cref="ServerActivityStatistics"/> update process.
    /// </summary>
    /// <seealso cref="MediatR.INotificationHandler{YGOProAnalyticsServer.Events.DataFromYgoProServerRetrieved}" />
    public class YgoProServerActivityAnalysisHandler : INotificationHandler<DataFromYgoProServerRetrieved>
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IServerActivityUpdater _updater;

        /// <summary>
        /// Initializes a new instance of the <see cref="YgoProServerActivityAnalysisHandler"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="updater">The updater.</param>
        /// <exception cref="ArgumentNullException">
        /// db
        /// or
        /// updater
        /// </exception>
        public YgoProServerActivityAnalysisHandler(YgoProAnalyticsDatabase db, IServerActivityUpdater updater)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));
        }

        /// <summary>
        /// Handle <see cref="DataFromYgoProServerRetrieved"/> event.
        /// </summary>
        /// <param name="notification">The event data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
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
