using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    /// <summary>
    /// Provide features related with decklists.
    /// </summary>
    public class DecklistService : IDecklistService
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IBanlistService _banlistService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecklistService"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="banlistService">The banlist service.</param>
        /// <exception cref="ArgumentNullException">
        /// db
        /// or
        /// banlistService
        /// </exception>
        public DecklistService(
            YgoProAnalyticsDatabase db,
            IBanlistService banlistService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _banlistService = banlistService ?? throw new ArgumentNullException(nameof(banlistService));
        }

        /// <inheritdoc />
        public async Task<IQueryable<Decklist>> FindAll(
            int minNumberOfGames = 10,
            int banlistId = -1,
            string archetypeName = "",
            DateTime? statisticsFrom = null,
            DateTime? statisticsTo = null,
            bool orderByDescendingByNumberOfGames = false,
            int[] wantedCardsInDeck = null)
        {
            IQueryable<Decklist> localDecklistsQuery = _getOrderedNoTrackedDecklists();
            if (statisticsTo == null && statisticsFrom == null)
            {
                localDecklistsQuery = _addMinNumberOfGamesFilterToLocalDecklistQuery(minNumberOfGames, localDecklistsQuery);
                if (orderByDescendingByNumberOfGames)
                {
                    localDecklistsQuery = _orderByDescendingByNumberOfGames(localDecklistsQuery);
                }
            }
            else
            {
                localDecklistsQuery = _addStatisticsDateLimitIfRequiredAndThenMinNumberOfGamesFilter(
                statisticsFrom,
                statisticsTo,
                minNumberOfGames,
                localDecklistsQuery);

                if (orderByDescendingByNumberOfGames)
                {
                     localDecklistsQuery = _orderDescendingByNumberOfGamesUsingDateLimitAndMinNumberOfGamesCriteria(
                     statisticsFrom,
                     statisticsTo,
                     minNumberOfGames,
                     localDecklistsQuery);
                }
                else
                {
                    localDecklistsQuery = _orderUsingDateLimitAndMinNumberOfGamesCriteria(
                    statisticsFrom,
                    statisticsTo,
                    minNumberOfGames,
                    localDecklistsQuery);
                }
            }

            localDecklistsQuery = _addArchetypeNameFilterToLocalDecklistQueryIfRequired(archetypeName, localDecklistsQuery);
            localDecklistsQuery = await _addBanlistFilterToLocalDecklistQueryIfRequired(banlistId, localDecklistsQuery);
            localDecklistsQuery = _addWantedCardsFilter(wantedCardsInDeck, localDecklistsQuery);

            return localDecklistsQuery;
        }

        private IQueryable<Decklist> _orderDescendingByNumberOfGamesUsingDateLimitAndMinNumberOfGamesCriteria(
            DateTime? statisticsFrom,
            DateTime? statisticsTo,
            int minNumberOfGames,
            IQueryable<Decklist> localDecklistsQuery)
        {
            if (statisticsFrom != null && statisticsTo == null)
            {
                localDecklistsQuery = localDecklistsQuery
                    .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames)
                    .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed)
                 );
            }
            else
           if (statisticsTo != null && statisticsFrom == null)
            {
                localDecklistsQuery = localDecklistsQuery
                    .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames)
                    .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed)
                );
            }
            else
           if (statisticsFrom != null && statisticsTo != null)
            {
                localDecklistsQuery = localDecklistsQuery
                    .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom
                               && z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames)
                    .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom
                              && z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed)
                );
            }

            return localDecklistsQuery;
        }

        private IQueryable<Decklist> _orderByDescendingByNumberOfGames(IQueryable<Decklist> localDecklistsQuery)
        {
            return localDecklistsQuery
                .OrderByDescending(x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWasUsed));
        }

        private IQueryable<Decklist> _addWantedCardsFilter(int[] wantedCardsInDeck, IQueryable<Decklist> localDecklistsQuery)
        {
            foreach (var wantedCardPasscode in wantedCardsInDeck)
            {
                localDecklistsQuery = localDecklistsQuery
                    .Where(x =>
                           x.CardsInMainDeckJoin.Where(y => y.Card.PassCode == wantedCardPasscode).Count() > 0 ||
                           x.CardsInExtraDeckJoin.Where(y => y.Card.PassCode == wantedCardPasscode).Count() > 0 ||
                           x.CardsInSideDeckJoin.Where(y => y.Card.PassCode == wantedCardPasscode).Count() > 0);
            }

            return localDecklistsQuery;
        }

        private IQueryable<Decklist> _orderUsingDateLimitAndMinNumberOfGamesCriteria(
            DateTime? statisticsFrom,
            DateTime? statisticsTo,
            int minNumberOfGames,
            IQueryable<Decklist> localDecklistsQuery)
        {
            if (statisticsFrom != null && statisticsTo == null)
            {
                localDecklistsQuery = localDecklistsQuery
                    .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames)
                    .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom)
                        .Sum(y => y.NumberOfTimesWhenDeckWon)
                 );
            }
            else
            if (statisticsTo != null && statisticsFrom == null)
            {
                localDecklistsQuery = localDecklistsQuery
                    .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames)
                    .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWon)
                );
            }
            else
            if (statisticsFrom != null && statisticsTo != null)
            {
                localDecklistsQuery = localDecklistsQuery
                     .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom
                               && z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames)
                     .OrderByDescending(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom
                               && z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWon)
                );
            }

            return localDecklistsQuery;
        }

        private IQueryable<Decklist> _addStatisticsDateLimitIfRequiredAndThenMinNumberOfGamesFilter(
            DateTime? statisticsFrom,
            DateTime? statisticsTo,
            int minNumberOfGames,
            IQueryable<Decklist> localDecklistsQuery)
        {

            if (statisticsFrom != null && statisticsTo == null)
            {
                localDecklistsQuery = localDecklistsQuery
                     .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
                );
            }
            else
            if (statisticsTo != null && statisticsFrom == null)
            {
                localDecklistsQuery = localDecklistsQuery
                     .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
                );
            }
            else
            if (statisticsFrom != null && statisticsTo != null)
            {
                localDecklistsQuery = localDecklistsQuery
                     .Where(x => x.DecklistStatistics
                        .Where(z => z.DateWhenDeckWasUsed >= statisticsFrom
                               && z.DateWhenDeckWasUsed <= statisticsTo)
                        .Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
                );
            }

            return localDecklistsQuery;
        }

        private async Task<IQueryable<Decklist>> _addBanlistFilterToLocalDecklistQueryIfRequired(
            int banlistId,
            IQueryable<Decklist> localDecklistsQuery)
        {
            if (banlistId > 0)
            {
                var banlistExist = _db.Banlists.Any(x => x.Id == banlistId);
                if (banlistExist == true)
                {
                    localDecklistsQuery = localDecklistsQuery
                        .Where(x => x.DecklistPlayableOnBanlistsJoin.Any(y=>y.BanlistId == banlistId && x.Id == y.DecklistId));
                }
            }

            return localDecklistsQuery;
        }

        private static IQueryable<Decklist> _addArchetypeNameFilterToLocalDecklistQueryIfRequired(
            string archetypeName,
            IQueryable<Decklist> localDecklistsQuery)
        {
            if (!string.IsNullOrEmpty(archetypeName))
            {
                localDecklistsQuery = localDecklistsQuery.Where(x => x.Archetype.Name.Contains(archetypeName));
            }

            return localDecklistsQuery;
        }

        private IQueryable<Decklist> _addMinNumberOfGamesFilterToLocalDecklistQuery(
            int minNumberOfGames,
            IQueryable<Decklist> localDecklistsQuery)
        {
            localDecklistsQuery = localDecklistsQuery.Where(
                x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWasUsed) >= minNumberOfGames
            );

            return localDecklistsQuery;
        }

        /// <summary>
        /// Ordered by NumberOfTimesWhenDeckWon.
        /// </summary>
        private IQueryable<Decklist> _getOrderedNoTrackedDecklists()
        {
            return _getDecklistsQuery(false)
                .OrderByDescending(
                    x => x.DecklistStatistics.Sum(y => y.NumberOfTimesWhenDeckWon)
                 );
        }

        /// <inheritdoc />
        public async Task<Decklist> GetByIdWithAllDataIncluded(int id)
        {
            var query = _getDecklistsQuery();

            return await query
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the decklists query with all data explicitly included.
        /// </summary>
        private IQueryable<Decklist> _getDecklistsQuery(bool shouldBeTracked = true)
        {
            IQueryable<Decklist> query;
            if (shouldBeTracked)
            {
                query = includeMainDeckWithAllData(_db.Decklists);
            }
            else
            {
                query = includeMainDeckWithAllData(_db.Decklists.AsNoTracking());
            }

            query = includeExtraDeckWithAllData(query);
            query = includeSideDeckWithAllData(query);
            query = query.Include(x => x.Archetype);
            query = query.Include(x => x.DecklistStatistics);

            return query;
        }

        /// <summary>
        /// Includes the main deck with data about all cards and banlists.
        /// </summary>
        /// <param name="query">The query.</param>
        protected IQueryable<Decklist> includeMainDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
                .Include($"{Decklist.IncludeMainDeckCards}")

                .Include($"{Decklist.IncludeMainDeckCards}.{nameof(Card.Archetype)}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{nameof(Card.MonsterCard)}" +
                         $".{nameof(MonsterCard.PendulumMonsterCard)}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{Card.IncludeWithForbiddenCardsBanlist}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{Card.IncludeWithLimitedCardsBanlist}")

                .Include($"{Decklist.IncludeMainDeckCards}" +
                         $".{Card.IncludeWithSemiLimitedCardsBanlist}")
               ;
        }
        /// <summary>
        /// Includes the extra deck with data about all cards and banlists.
        /// </summary>
        /// <param name="query">The query.</param>
        protected IQueryable<Decklist> includeExtraDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
               .Include($"{Decklist.IncludeExtraDeckCards}")

               .Include($"{Decklist.IncludeExtraDeckCards}.{nameof(Card.Archetype)}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{nameof(Card.MonsterCard)}" +
                        $".{nameof(MonsterCard.PendulumMonsterCard)}")

                .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{nameof(Card.MonsterCard)}" +
                        $".{nameof(MonsterCard.LinkMonsterCard)}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{Card.IncludeWithForbiddenCardsBanlist}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{Card.IncludeWithLimitedCardsBanlist}")

               .Include($"{Decklist.IncludeExtraDeckCards}" +
                        $".{Card.IncludeWithSemiLimitedCardsBanlist}")
              ;
        }

        /// <summary>
        /// Includes the side deck with data about all cards and banlists.
        /// </summary>
        /// <param name="query">The query.</param>
        protected IQueryable<Decklist> includeSideDeckWithAllData(IQueryable<Decklist> query)
        {
            return query
              .Include($"{Decklist.IncludeSideDeckCards}")

              .Include($"{Decklist.IncludeSideDeckCards}.{nameof(Card.Archetype)}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{nameof(Card.MonsterCard)}" +
                       $".{nameof(MonsterCard.PendulumMonsterCard)}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                        $".{nameof(Card.MonsterCard)}" +
                        $".{nameof(MonsterCard.LinkMonsterCard)}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{Card.IncludeWithForbiddenCardsBanlist}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{Card.IncludeWithLimitedCardsBanlist}")

              .Include($"{Decklist.IncludeSideDeckCards}" +
                       $".{Card.IncludeWithSemiLimitedCardsBanlist}")
             ;
        }

        /// <summary>
        /// Includes PlayableOnBanlists.
        /// </summary>
        /// <param name="query">The query.</param>
        protected IQueryable<Decklist> includePlayableOnBanlists(IQueryable<Decklist> query)
        {
            return query
              .Include($"{Decklist.IncludePlayableOnBanlists}");
        }

        /// <summary>
        /// Gets the decklists query with data about cards and banlists explicitly included.
        /// </summary>
        public IQueryable<Decklist> GetDecklistsQueryForBanlistAnalysis(bool shouldBeTracked = true)
        {
            IQueryable<Decklist> query;
            if (shouldBeTracked)
            {
                query = includePlayableOnBanlists(_db.Decklists);
            }
            else
            {
                query = includePlayableOnBanlists(_db.Decklists.AsNoTracking());
            }
            query = includeMainDeckWithAllData(query);
            query = includeExtraDeckWithAllData(query);
            query = includeSideDeckWithAllData(query);

            return query;
        }
    }
}
