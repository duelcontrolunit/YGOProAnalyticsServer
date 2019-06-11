using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Helpers;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    /// <summary>
    /// Provide features related with banlists.
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Others.Interfaces.IBanlistService" />
    public class BanlistService : IBanlistService
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IMemoryCache _cache;
        readonly IAdminConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanlistService"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// db
        /// or
        /// cache
        /// or
        /// config
        /// </exception>
        public BanlistService(
            YgoProAnalyticsDatabase db,
            IMemoryCache cache,
            IAdminConfig config)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<BanlistIdAndNameDTO>> GetListOfBanlistsNamesAndIdsAsNoTrackingFromCache(bool shouldIgnoreCache = false)
        {
            IEnumerable<BanlistIdAndNameDTO> dtos;
            if (shouldIgnoreCache)
            {
                return await _getBanlistsIdAndNameDtosAsNoTracking();
            }
            else
            if (!_cache.TryGetValue(CacheKeys.BanlistsIdAndNameDtos, out dtos))
            {
                dtos = await _getBanlistsIdAndNameDtosAsNoTracking();
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSlidingExpiration(
                        TimeSpan.FromHours(_config.BanlistSlidingCacheExpirationInHours))
                    .SetAbsoluteExpiration(
                        TimeSpan.FromHours(_config.BanlistAbsoluteCacheExpirationInHours));

                _cache.Set(CacheKeys.BanlistsIdAndNameDtos, dtos, cacheOptions);
            }

            return dtos;
        }

        private async Task<IEnumerable<BanlistIdAndNameDTO>> _getBanlistsIdAndNameDtosAsNoTracking()
        {
            return await _db
                .Banlists
                .AsNoTracking()
                .Select(x => new BanlistIdAndNameDTO(x.Id, x.Name))
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Banlist> GetBanlistWithAllCardsIncludedAsync(int banlistId)
        {
            return await _db
                    .Banlists
                    .Where(x => x.Id == banlistId)
                    .Include(Banlist.IncludeWithForbiddenCards)
                    .Include(Banlist.IncludeWithLimitedCards)
                    .Include(Banlist.IncludeWithSemiLimitedCards)
                    .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public bool CanDeckBeUsedOnGivenBanlist(Decklist decklist, Banlist banlist)
        {
            var countedCards = _countCards(decklist);

            bool isForbiddenCardInDecklist = _deckContainsNotAllowedCards(
                banlist.ForbiddenCards,
                countedCards,
                (card, countedCard) => { return true; });

            bool isMoreThanOneCopyOfLimitedCardInDeck = _deckContainsNotAllowedCards(
                banlist.LimitedCards,
                countedCards,
                (card, countedCard) =>
                {
                    return countedCard.NumberOfCopies > 1;
                });

            bool isMoreThanTwoCopiesOfSemiLimitedCardInDeck = _deckContainsNotAllowedCards(
              banlist.SemiLimitedCards,
              countedCards,
              (card, countedCard) =>
              {
                  return countedCard.NumberOfCopies > 2;
              });

            return !isForbiddenCardInDecklist
                   && !isMoreThanOneCopyOfLimitedCardInDeck
                   && !isMoreThanTwoCopiesOfSemiLimitedCardInDeck;
        }

        /// <inheritdoc />
        public async Task<IQueryable<Banlist>> FindAllQuery(
            int minNumberOfGames,
            string formatOrName = "",
            DateTime? statisticsFromDate = null,
            DateTime? statisticsToDate = null)
        {
            var banlistQuery = _db
                .Banlists
                .Where(x => x.Name.ToLower().Contains(formatOrName.ToLower()));

            if(statisticsFromDate != null && statisticsToDate == null)
            {
                return banlistQuery
                    .Where(x => x.
                        Statistics
                            .Where(y => y.DateWhenBanlistWasUsed >= statisticsFromDate)
                            .Sum(y => y.HowManyTimesWasUsed) >= minNumberOfGames
                    )
                    .OrderBy(x => x.Statistics
                        .Where(y => y.DateWhenBanlistWasUsed >= statisticsFromDate)
                        .Sum(y => y.HowManyTimesWasUsed)
                     );
            }
            else
            if (statisticsFromDate == null && statisticsToDate != null)
            {
                return banlistQuery
                    .Where(x => x.
                        Statistics
                            .Where(y => y.DateWhenBanlistWasUsed <= statisticsToDate)
                            .Sum(y => y.HowManyTimesWasUsed) >= minNumberOfGames
                    )
                     .OrderBy(x => x.Statistics
                        .Where(y => y.DateWhenBanlistWasUsed <= statisticsToDate)
                        .Sum(y => y.HowManyTimesWasUsed)
                     );
            }
            else
            if (statisticsFromDate != null && statisticsToDate != null)
            {
                return banlistQuery
                    .Where(x => x.
                        Statistics
                            .Where(y => y.DateWhenBanlistWasUsed <= statisticsToDate 
                                        && y.DateWhenBanlistWasUsed >= statisticsFromDate)
                            .Sum(y => y.HowManyTimesWasUsed) >= minNumberOfGames
                    )
                    .OrderBy(x => x.Statistics
                        .Where(y => y.DateWhenBanlistWasUsed <= statisticsToDate
                                        && y.DateWhenBanlistWasUsed >= statisticsFromDate)
                       .Sum(y => y.HowManyTimesWasUsed)
                    );
            }
            else
            {
                return banlistQuery
                    .Where(x => x.Statistics.Sum(y => y.HowManyTimesWasUsed) >= minNumberOfGames)
                     .OrderBy(x => x.Statistics
                       .Sum(y => y.HowManyTimesWasUsed)
                    );
            }   
        }

        private bool _deckContainsNotAllowedCards(
            ICollection<Card> cardsOnBanlist,
            IEnumerable<CardWithInfoAboutNumberOfCopiesInDeck> countedCards,
            Func<Card, CardWithInfoAboutNumberOfCopiesInDeck, bool> additionalCondition)
        {
            foreach (var countedCard in countedCards)
            {
                var card = cardsOnBanlist
                    .Where(x => x.Id == countedCard.CardId)
                    .FirstOrDefault();
                if (card != null && additionalCondition(card, countedCard))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<CardWithInfoAboutNumberOfCopiesInDeck> _countCards(Decklist decklist)
        {
            var cardsWithInfoAboutNumberOfCopiesInDeck = new List<CardWithInfoAboutNumberOfCopiesInDeck>();
            _countCardsFromDeck(cardsWithInfoAboutNumberOfCopiesInDeck, decklist.MainDeck);
            _countCardsFromDeck(cardsWithInfoAboutNumberOfCopiesInDeck, decklist.ExtraDeck);
            _countCardsFromDeck(cardsWithInfoAboutNumberOfCopiesInDeck, decklist.SideDeck);

            return cardsWithInfoAboutNumberOfCopiesInDeck;
        }

        private void _countCardsFromDeck(
            List<CardWithInfoAboutNumberOfCopiesInDeck> cardsWithInfoAboutNumberOfCopiesInDeck,
            ICollection<Card> cards)
        {
            foreach (var card in cards)
            {
                var cardWithInfoAboutNumberOfCopiesInDeck = cardsWithInfoAboutNumberOfCopiesInDeck
                    .Where(x => x.CardId == card.Id)
                    .FirstOrDefault();
                if (cardWithInfoAboutNumberOfCopiesInDeck == null)
                {
                    cardWithInfoAboutNumberOfCopiesInDeck = new CardWithInfoAboutNumberOfCopiesInDeck(card.Id);
                    cardsWithInfoAboutNumberOfCopiesInDeck.Add(
                        cardWithInfoAboutNumberOfCopiesInDeck
                    );
                }
                else
                {
                    cardWithInfoAboutNumberOfCopiesInDeck.IncrementNumberOfCopies();
                }
            }
        }
  
        /// <summary>
        /// CardId with info about how many copies this card is in deck.
        /// </summary>
        public class CardWithInfoAboutNumberOfCopiesInDeck
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CardWithInfoAboutNumberOfCopiesInDeck"/> class.
            /// </summary>
            /// <param name="cardId">The card identifier.</param>
            public CardWithInfoAboutNumberOfCopiesInDeck(int cardId)
            {
                CardId = cardId;
            }

            /// <summary>
            /// Card identifier.
            /// </summary>
            public int CardId { get; private set; }

            /// <summary>
            /// Number of card copies. One by default.
            /// </summary>
            public int NumberOfCopies { get; private set; } = 1;

            /// <summary>
            /// Increments the number of copies.
            /// </summary>
            public void IncrementNumberOfCopies()
            {
                NumberOfCopies++;
            }
        }
    }
}
