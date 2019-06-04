using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Events;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;
using YGOProAnalyticsServer.Services.Converters.Interfaces;

namespace YGOProAnalyticsServer.EventHandlers
{
    public class YgoProAnalysisBasedOnDataFromYgoProServer : INotificationHandler<DataFromYgoProServerRetrieved>
    {
        IDuelLogNameAnalyzer _duelLogNameAnalyzer;
        YgoProAnalyticsDatabase _db;
        IArchetypeAndDecklistAnalyzer _archetypeAndDecklistAnalyzer;
        IYDKToDecklistConverter _yDKToDecklistConverter;

        public YgoProAnalysisBasedOnDataFromYgoProServer(
            IDuelLogNameAnalyzer duelLogNameAnalyzer,
            YgoProAnalyticsDatabase db,
            IArchetypeAndDecklistAnalyzer archetypeAndDecklistAnalyzer,
            IYDKToDecklistConverter yDKToDecklistConverter)
        {
            _duelLogNameAnalyzer = duelLogNameAnalyzer;
            _db = db;
            _archetypeAndDecklistAnalyzer = archetypeAndDecklistAnalyzer;
            _yDKToDecklistConverter = yDKToDecklistConverter;
        }

        public async Task Handle(DataFromYgoProServerRetrieved notification, CancellationToken cancellationToken)
        {
            var duelLogsFromAllDates = notification.ConvertedDuelLogs;
            var decklistsAsStringsWithFilenames = notification.UnzippedDecklistsWithDecklistFileName;

            foreach (var duelLogsPack in duelLogsFromAllDates)
            {
                await _analyze(
                    duelLogsPack,
                    decklistsAsStringsWithFilenames.Where(x => x.Key == duelLogsPack.Key).First()
                );
            }
        }

        private async Task _analyze(
            KeyValuePair<DateTime, List<DuelLog>> duelLogsFromThePack,
            KeyValuePair<DateTime, List<DecklistWithName>> decklistsAsStringsWithFilenames)
        {
            var allDecksWhichWonFromThePack = new List<Decklist>();
            var allDecksWhichLostFromThePack = new List<Decklist>();
            foreach (var duelLog in duelLogsFromThePack.Value)
            {
                if (!isBanlistOk(duelLog))
                {
                    continue;
                }

                _analyzeBanlist(duelLog);
                _assignConvertedDecklistToProperCollection(
                    decklistsAsStringsWithFilenames,
                    allDecksWhichWonFromThePack,
                    allDecksWhichLostFromThePack,
                    duelLog);
            }

            var decklistsWhichWonWithoutDuplicatesFromThePack = _analyzeDecklistsAndArchetypesAndRemoveDuplicates(
                allDecksWhichWonFromThePack,
                true);
            var decklistsWhichLostWithoutDuplicatesFromThePack = _analyzeDecklistsAndArchetypesAndRemoveDuplicates(
                allDecksWhichLostFromThePack,
                false);
            var allDecklistsFromThePack = _removeDuplicatesAndMerge(
                decklistsWhichWonWithoutDuplicatesFromThePack,
                decklistsWhichLostWithoutDuplicatesFromThePack);

            await _updateDecklistsStatisticsAndAddNewDecksToDatabase(allDecklistsFromThePack);
            await _db.SaveChangesAsync();
        }

        private async Task _updateDecklistsStatisticsAndAddNewDecksToDatabase(
            List<Decklist> allDecklistsFromThePack)
        {
            var newDecks = new List<Decklist>();
            var decklistsFromDb = _db.Decklists.Include(x => x.DecklistStatistics).ToList();
            foreach (var decklist in allDecklistsFromThePack)
            {
                bool isDuplicate = false;
                foreach (var decklistFromDb in decklistsFromDb)
                {
                    if (_archetypeAndDecklistAnalyzer.CheckIfDecklistsAreDuplicate(decklist, decklistFromDb))
                    {
                        isDuplicate = true;
                        if(decklist.WhenDecklistWasFirstPlayed < decklistFromDb.WhenDecklistWasFirstPlayed)
                        {
                            decklistFromDb.WhenDecklistWasFirstPlayed = decklist.WhenDecklistWasFirstPlayed;
                        }
                        _updateDecklistStatistics(decklist, decklistFromDb);
                        break;
                    }
                }

                if (isDuplicate)
                {
                    continue;
                }

                decklist.Name = $"{decklist.Archetype.Name}_{decklist.WhenDecklistWasFirstPlayed}";
                newDecks.Add(decklist);
            }

            await _db.AddRangeAsync(newDecks);
        }

        private void _updateDecklistStatistics(Decklist decklist, Decklist decklistFromDb)
        {
            foreach (var statistics in decklist.DecklistStatistics)
            {
                var statisticsFromDb = decklistFromDb
                    .DecklistStatistics
                    .Where(x => x.DateWhenDeckWasUsed.Date == statistics.DateWhenDeckWasUsed.Date)
                    .FirstOrDefault();
                if (statisticsFromDb == null)
                {
                    statisticsFromDb = DecklistStatistics.Create(
                        decklistFromDb,
                        statistics.DateWhenDeckWasUsed.Date
                    );
                    _db.DecklistStatistics.Add(statisticsFromDb);
                }

                statisticsFromDb.IncrementNumberOfTimesWhenDeckWasUsedByAmount(statistics.NumberOfTimesWhenDeckWasUsed);
                statisticsFromDb.IncrementNumberOfTimesWhenDeckWonByAmount(statistics.NumberOfTimesWhenDeckWon);
            }
        }

        private List<Decklist> _analyzeDecklistsAndArchetypesAndRemoveDuplicates(
            List<Decklist> allDecksFromThePack,
            bool decksWon)
        {
            List<Decklist> decklistsWithoutDuplicates = new List<Decklist>();
            foreach (var decklist in allDecksFromThePack)
            {
                bool decklistAlreadyExisting = false;
                foreach (var decklistChecked in decklistsWithoutDuplicates)
                {
                    if (_archetypeAndDecklistAnalyzer.CheckIfDecklistsAreDuplicate(decklist, decklistChecked))
                    {
                        decklistAlreadyExisting = true;
                    }
                }

                if (decklistAlreadyExisting)
                {
                    continue;
                }

                var numberOfDuplicatesWithListOfDecklists = _archetypeAndDecklistAnalyzer.
                    RemoveDuplicateDecklistsFromListOfDecklists(
                    decklist,
                    allDecksFromThePack.OrderBy(x => x.WhenDecklistWasFirstPlayed));
                decklistsWithoutDuplicates.Add(numberOfDuplicatesWithListOfDecklists.DecklistThatWasChecked);
                var statistics = decklist.DecklistStatistics
                    .FirstOrDefault(x => x.DateWhenDeckWasUsed == decklist.WhenDecklistWasFirstPlayed);
                if (statistics == null)
                {
                    statistics = DecklistStatistics.Create(decklist, decklist.WhenDecklistWasFirstPlayed);
                    decklist.DecklistStatistics.Add(statistics);
                }

                statistics.
                    IncrementNumberOfTimesWhenDeckWasUsedByAmount(numberOfDuplicatesWithListOfDecklists.DuplicateCount);
                if (decksWon)
                {
                    statistics.
                        IncrementNumberOfTimesWhenDeckWonByAmount(numberOfDuplicatesWithListOfDecklists.DuplicateCount);
                }

                var archetype = _archetypeAndDecklistAnalyzer.GetArchetypeOfTheDecklistWithStatistics(
                    decklist,
                    decklist.WhenDecklistWasFirstPlayed);
                var archetypeStatisticsFromDay = archetype.Statistics
                    .First(x => x.DateWhenArchetypeWasUsed == decklist.WhenDecklistWasFirstPlayed);

                archetypeStatisticsFromDay.IncrementNumberOfDecksWhereWasUsedByAmount(
                    numberOfDuplicatesWithListOfDecklists.DuplicateCount);
                if (decksWon)
                {
                    archetypeStatisticsFromDay.IncrementNumberOfTimesWhenArchetypeWonByAmount(
                        numberOfDuplicatesWithListOfDecklists.DuplicateCount);
                }
            }

            return decklistsWithoutDuplicates;
        }

        private List<Decklist> _removeDuplicatesAndMerge(List<Decklist> decklists1, List<Decklist> decklists2)
        {
            var allDecklists = decklists1.Concat(decklists2).ToList();
            foreach (var decklistInDecklists1 in decklists1)
            {
                foreach (var decklistInDecklists2 in decklists2)
                {
                    if (_archetypeAndDecklistAnalyzer.CheckIfDecklistsAreDuplicate(decklistInDecklists1, decklistInDecklists2))
                    {
                        foreach (var decklistStatistics in decklistInDecklists2.DecklistStatistics)
                        {
                            var stats = decklistInDecklists1
                                .DecklistStatistics
                                .FirstOrDefault(x => x.DateWhenDeckWasUsed == decklistStatistics.DateWhenDeckWasUsed);
                            if (stats == null)
                            {
                                decklistInDecklists1.DecklistStatistics.Add(decklistStatistics);
                            }
                            else
                            {
                                stats.IncrementNumberOfTimesWhenDeckWasUsedByAmount(decklistStatistics.NumberOfTimesWhenDeckWasUsed);
                                stats.IncrementNumberOfTimesWhenDeckWonByAmount(decklistStatistics.NumberOfTimesWhenDeckWon);
                            }
                        }
                        allDecklists.Remove(decklistInDecklists2);
                    }
                }
            }

            return allDecklists;
        }

        private bool isBanlistOk(DuelLog duelLog)
        {
            return _duelLogNameAnalyzer.IsAnyBanlist(duelLog.Name)
                   && !_duelLogNameAnalyzer.IsDuelVersusAI(duelLog.Name)
                   && !_duelLogNameAnalyzer.IsNoDeckCheckEnabled(duelLog.Name)
                   && !_duelLogNameAnalyzer.IsNoDeckShuffleEnabled(duelLog.Name);
        }

        private void _assignConvertedDecklistToProperCollection(
            KeyValuePair<DateTime, List<DecklistWithName>> decklistsAsStringsWithFilenames,
            List<Decklist> allDecksWhichWonFromThePack,
            List<Decklist> allDecksWhichLostFromThePack,
            DuelLog duelLog)
        {
            foreach (var deckWhichWonFileName in duelLog.DecksWhichWonFileNames)
            {

                var decklistWithFileName = decklistsAsStringsWithFilenames.Value.FirstOrDefault(x => x.DecklistFileName == deckWhichWonFileName);
                if (decklistWithFileName == null)
                {
                    //TODO log if file is missing
                    continue;
                }

                Decklist decklist = _yDKToDecklistConverter.Convert(decklistWithFileName.DecklistData);
                decklist.WhenDecklistWasFirstPlayed = duelLog.DateOfTheBeginningOfTheDuel.Date;
                allDecksWhichWonFromThePack.Add(
                    decklist
                );
            }

            foreach (var deckWhichLostFileName in duelLog.DecksWhichLostFileNames)
            {
                var decklistWithFileName = decklistsAsStringsWithFilenames.Value.FirstOrDefault(x => x.DecklistFileName == deckWhichLostFileName);
                if (decklistWithFileName == null)
                {
                    //TODO log if file is missing
                    continue;
                }

                Decklist decklist = _yDKToDecklistConverter.Convert(decklistWithFileName.DecklistData);
                decklist.WhenDecklistWasFirstPlayed = duelLog.DateOfTheBeginningOfTheDuel.Date;
                allDecksWhichLostFromThePack.Add(
                   decklist
                );
            }
        }

        private void _analyzeBanlist(DuelLog duelLog)
        {
            Banlist banlist = _duelLogNameAnalyzer.GetBanlist(duelLog.Name, duelLog.DateOfTheBeginningOfTheDuel.Date);
            var banlistStatistics = banlist.Statistics.FirstOrDefault(x => x.DateWhenBanlistWasUsed == duelLog.DateOfTheBeginningOfTheDuel.Date);
            if (banlistStatistics == null)
            {
                banlistStatistics = BanlistStatistics.Create(duelLog.DateOfTheBeginningOfTheDuel.Date, banlist);
                banlist.Statistics.Add(banlistStatistics);
            }

            banlistStatistics.IncrementHowManyTimesWasUsed();
        }
    }
}
