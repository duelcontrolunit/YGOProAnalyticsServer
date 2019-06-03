using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
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
        readonly YgoProAnalyticsDatabase _db;

        public YgoProServerDataRetriever(
            IFTPDownloader fTPDownloader,
            IAdminConfig adminConfig,
            IFileUnzipper unzipper,
            IDuelLogConverter duelLogConverter,
            IMediator mediator,
            YgoProAnalyticsDatabase db)
        {
            _fTPDownloader = fTPDownloader;
            _adminConfig = adminConfig;
            _unzipper = unzipper;
            _duelLogConverter = duelLogConverter;
            _mediator = mediator;
            _db = db;
        }

        public async Task Handle(
            CardsRelatedUpdatesCompleted notification, 
            CancellationToken cancellationToken)
        {
            var convertedDuelLogs = await _getConvertedDuelLogs();
            var unzippedDecklistsWithDecklistFileName = await _getUnzippedDecklists();
            if (Directory.Exists(_adminConfig.DataFolderLocation))
            {
                Directory.Delete(_adminConfig.DataFolderLocation, true);
            }

            await _mediator.Publish(
                new DataFromYgoProServerRetrieved(convertedDuelLogs, unzippedDecklistsWithDecklistFileName)
            );
        }

        private async Task<Dictionary<DateTime, List<DecklistWithName>>> _getUnzippedDecklists()
        {
            var unzippedDecklists = new Dictionary<DateTime, List<DecklistWithName>>();
            var listOfDecklists = _fTPDownloader
                .DownloadListOfFilesFromFTP(_adminConfig.ServerDataEndpointURL + "/decks_saves/");
            AnalysisMetadata metaData = _getMetaData();
            DateTime dateOfDecklistsPack = new DateTime();
            DateTime dateOfNewestDecklistPack = metaData.LastDecklistsPackDate;

            foreach (string decklistZipName in listOfDecklists)
            {
                string pathToDecklistsZip = await _fTPDownloader
                    .DownloadDeckFromFTP(_adminConfig.ServerDataEndpointURL + "/decks_saves/" + decklistZipName);
                dateOfDecklistsPack = DateTime.ParseExact(_extractDate(decklistZipName, "decks_save_"), "dd MM yy", CultureInfo.InvariantCulture);
                if (dateOfDecklistsPack > metaData.LastDecklistsPackDate)
                {
                    if(dateOfDecklistsPack > dateOfNewestDecklistPack)
                    {
                        dateOfNewestDecklistPack = dateOfDecklistsPack;
                    }
                    unzippedDecklists.Add(
                        dateOfDecklistsPack,
                       _unzipper.GetDecksFromZip(pathToDecklistsZip)
                   );
                }   
            }

            metaData.LastDecklistsPackDate = dateOfNewestDecklistPack;
            await _db.SaveChangesAsync();

            return unzippedDecklists;
        }

        private async Task<Dictionary<DateTime, List<DuelLog>>> _getConvertedDuelLogs()
        {
            var convertedDuelLogs = new Dictionary<DateTime, List<DuelLog>>();
            var listOfDuelLogs = _fTPDownloader.DownloadListOfFilesFromFTP(_adminConfig.ServerDataEndpointURL + "/duel_logs/");
            AnalysisMetadata metaData = _getMetaData();
            DateTime dateOfDuelLog = new DateTime();
            DateTime dateOfNewestDuelLog = metaData.LastDuelLogDateAnalyzed;

            foreach (string duelLogName in listOfDuelLogs)
            {
                dateOfDuelLog = DateTime.ParseExact(_extractDate(duelLogName, "duel_log"),"dd MM yy", CultureInfo.InvariantCulture);
                if (dateOfDuelLog > metaData.LastDuelLogDateAnalyzed)
                {
                    if(dateOfDuelLog > dateOfNewestDuelLog)
                    {
                        dateOfNewestDuelLog = dateOfDuelLog;
                    }
                    convertedDuelLogs.Add(
                    dateOfDuelLog,
                    await _getDuelLogs(duelLogName)
                    );
                }
            }

            metaData.LastDuelLogDateAnalyzed = dateOfNewestDuelLog;
            await _db.SaveChangesAsync();

            return convertedDuelLogs;
        }

        private AnalysisMetadata _getMetaData()
        {
            var metaData = _db.AnalysisMetadata.FirstOrDefault();
            if (metaData == null)
            {
                metaData = new AnalysisMetadata();
                _db.AnalysisMetadata.Add(metaData);
            }

            return metaData;
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
