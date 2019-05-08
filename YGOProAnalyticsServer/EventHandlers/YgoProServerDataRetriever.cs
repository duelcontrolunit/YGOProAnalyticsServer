using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Events;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using YGOProAnalyticsServer.Services.Unzippers.Interfaces;

namespace YGOProAnalyticsServer.EventHandlers
{
    public class YgoProServerDataRetriever : INotificationHandler<CardsRelatedUpdatesCompleted>
    {
        readonly IFTPDownloader _fTPDownloader;
        readonly IAdminConfig _adminConfig;
        readonly IFileUnzipper _unzipper;
        readonly IDuelLogConverter _duelLogConverter;
        readonly IMediator _mediator;

        public async Task Handle(CardsRelatedUpdatesCompleted notification, CancellationToken cancellationToken)
        {
            var convertedDuelLogs = await GetConvertedDuelLogs();
            var unzippedDecklistsWithDecklistFileName = await GetUnzippedDecklists();

            await _mediator.Publish(new DataFromYgoProServerRetrieved(convertedDuelLogs, unzippedDecklistsWithDecklistFileName));
        }

        private async Task<Dictionary<DateTime, List<DecklistWithName>>> GetUnzippedDecklists()
        {
            var unzippedDecklists = new Dictionary<DateTime, List<DecklistWithName>>();
            var listOfDecklists = _fTPDownloader.DownloadListOfFilesFromFTP(_adminConfig.ServerDataEndpointURL + "/decks_saves/");
            foreach (string decklistZipName in listOfDecklists)
            {
                string pathToDecklistsZip = await _fTPDownloader.DownloadDeckFromFTP(_adminConfig.ServerDataEndpointURL + "/decks_saves/" + decklistZipName);
                unzippedDecklists.Add(
                     DateTime.Parse(_extractDate(decklistZipName, "decks_save_")),
                    _unzipper.GetDecksFromZip(pathToDecklistsZip)
                );
            }

            return unzippedDecklists;
        }

        private async Task<Dictionary<DateTime, List<DuelLog>>> GetConvertedDuelLogs()
        {
            var convertedDuelLogs = new Dictionary<DateTime, List<DuelLog>>();
            var listOfDuelLogs = _fTPDownloader.DownloadListOfFilesFromFTP(_adminConfig.ServerDataEndpointURL + "/duel_logs/");
            foreach (string duelLogName in listOfDuelLogs)
            {
                //TODO
                convertedDuelLogs.Add(
                    DateTime.Parse(_extractDate(duelLogName, "duel_log")),
                    await _getDuelLogs(duelLogName)
                );
            }

            return convertedDuelLogs;
        }

        private async Task<List<DuelLog>> _getDuelLogs(string duelLogName)
        {
            string pathToDuelLog = await _fTPDownloader.DownloadDuelLogFromFTP(_adminConfig.ServerDataEndpointURL + "/duel_logs/" + duelLogName);
            string duelLogContent = _unzipper.GetDuelLogFromZip(pathToDuelLog);

            return _duelLogConverter.Convert(duelLogContent);
        }

        private string _extractDate(string fileName, string prefix)
        {
            return Path.GetFileNameWithoutExtension(fileName)
                .Replace(prefix, "")
                .Replace("_", " ");
        }
    }
}
