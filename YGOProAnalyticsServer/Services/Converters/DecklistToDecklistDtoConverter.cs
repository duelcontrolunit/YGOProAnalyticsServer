using System;
using System.Linq;
using System.Collections.Generic;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// Provide convert decklist to dto decklist feature.
    /// </summary>
    public class DecklistToDecklistDtoConverter : IDecklistToDecklistDtoConverter
    {
        readonly IDecksDtosFactory _decksDtosFactory;

        public DecklistToDecklistDtoConverter(IDecksDtosFactory decksDtosFactory)
        {
            _decksDtosFactory = decksDtosFactory ?? throw new ArgumentNullException(nameof(decksDtosFactory));
        }

        /// <inheritdoc />
        public DecklistWithStatisticsDTO Convert(Decklist decklist)
        {
            var decklistDto = new DecklistWithStatisticsDTO() {
                DecklistId = decklist.Id,
                WhenDeckWasFirstPlayed = decklist.WhenDecklistWasFirstPlayed,
                Name = decklist.Name,
                Archetype = decklist.Archetype.Name,
            };

            var decklistStatisticsDtos = new List<DeckStatisticsDTO>();
            foreach (var statistics in decklist.DecklistStatistics)
            {
                decklistStatisticsDtos.Add(
                    new DeckStatisticsDTO(){
                        DateWhenDeckWasUsed = statistics.DateWhenDeckWasUsed,
                        NumberOfTimesWhenDeckWasUsed = statistics.NumberOfTimesWhenDeckWasUsed,
                        NumberOfTimesWhenDeckWon = statistics.NumberOfTimesWhenDeckWon
                    }
                );
            }

            decklistDto.Statistics = decklistStatisticsDtos;
            decklistDto.MainDeck = _decksDtosFactory.CreateMainDeckDto(decklist);
            decklistDto.ExtraDeck = _decksDtosFactory.CreateExtraDeckDto(decklist);
            decklistDto.SideDeck = _decksDtosFactory.CreateDeckDto(decklist);

            return decklistDto;
        }

        /// <inheritdoc />
        public IEnumerable<DecklistWithStatisticsDTO> Convert(IEnumerable<Decklist> decklists)
        {
            var decklistsDtos = new List<DecklistWithStatisticsDTO>();
            foreach (var decklist in decklists)
            {
                decklistsDtos.Add(
                    Convert(decklist)
                );
            }

            return decklistsDtos;
        }

        /// <inheritdoc />
        public IEnumerable<DecklistWithoutAnyAdditionalDataDTO> Convert(
            IEnumerable<Decklist> decklists,
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null)
        {
            var dtos = new List<DecklistWithoutAnyAdditionalDataDTO>();
            foreach (var decklist in decklists)
            {
                var dto = new DecklistWithoutAnyAdditionalDataDTO(
                    id: decklist.Id,
                    name: decklist.Name,
                    whenDecklistWasFirstPlayed: decklist.WhenDecklistWasFirstPlayed,
                    numberOfWins: _getNumberOfWinsInRange(decklist, statisticsFrom, statisticsTo),
                    numberOfGames: _getNumberOfGamesInRange(decklist, statisticsFrom, statisticsTo)
                );

                dtos.Add(dto);
            }

            return dtos;
        }

        private int _getNumberOfGamesInRange(Decklist decklist, DateTime? statisticsFrom, DateTime? statisticsTo)
        {
            if(statisticsFrom != null && statisticsTo == null)
            {
                return decklist
                    .DecklistStatistics
                    .Where(x => x.DateWhenDeckWasUsed >= statisticsFrom)
                    .Sum(x => x.NumberOfTimesWhenDeckWasUsed);
            }
            else
            if (statisticsFrom == null && statisticsTo != null)
            {
                return decklist
                    .DecklistStatistics
                    .Where(x => x.DateWhenDeckWasUsed <= statisticsTo)
                    .Sum(x => x.NumberOfTimesWhenDeckWasUsed);
            }
            else if(statisticsFrom != null && statisticsTo != null)
            {
                return decklist
                    .DecklistStatistics
                    .Where(x => x.DateWhenDeckWasUsed >= statisticsFrom && x.DateWhenDeckWasUsed >= statisticsFrom)
                    .Sum(x => x.NumberOfTimesWhenDeckWasUsed);
            }
            else
            {
                return decklist
                    .DecklistStatistics
                    .Sum(x => x.NumberOfTimesWhenDeckWasUsed);
            }
        }

        private int _getNumberOfWinsInRange(Decklist decklist, DateTime? statisticsFrom, DateTime? statisticsTo)
        {
            if (statisticsFrom != null && statisticsTo == null)
            {
                return decklist
                    .DecklistStatistics
                    .Where(x => x.DateWhenDeckWasUsed >= statisticsFrom)
                    .Sum(x => x.NumberOfTimesWhenDeckWon);
            }
            else
            if (statisticsFrom == null && statisticsTo != null)
            {
                return decklist
                    .DecklistStatistics
                    .Where(x => x.DateWhenDeckWasUsed <= statisticsTo)
                    .Sum(x => x.NumberOfTimesWhenDeckWon);
            }
            else if (statisticsFrom != null && statisticsTo != null)
            {
                return decklist
                    .DecklistStatistics
                    .Where(x => x.DateWhenDeckWasUsed >= statisticsFrom && x.DateWhenDeckWasUsed >= statisticsFrom)
                    .Sum(x => x.NumberOfTimesWhenDeckWon);
            }
            else
            {
                return decklist
                    .DecklistStatistics
                    .Sum(x => x.NumberOfTimesWhenDeckWon);
            }
        }
    }
}
