using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    /// <summary>
    /// Provide features related with decklists.
    /// </summary>
    public interface IDecklistService
    {
        /// <summary>
        /// Finds all by criteria.
        /// </summary>
        /// <param name="minNumberOfGames">The minimum number of games.</param>
        /// <param name="banlistId">The banlist identifier.</param>
        /// <param name="archetypeName">Name of the archetype.</param>
        /// <param name="statisticsFrom">
        ///     Exclude that decks which have less than minNumberOfGames games from that date.
        /// </param>
        /// <param name="statisticsTo">
        ///     Exclude that decks which have less than minNumberOfGames games to that date.
        /// </param>
        /// <param name="shouldGetDecksFromCache">Should take decks from cache?</param>
        /// <param name="orderByDescendingByNumberOfGames">False mean orderByDescending by number of wins.</param>
        /// <param name="wantedCardsInDeck">Array of passcodes.</param>
        Task<IQueryable<Decklist>> FindAll(
            int minNumberOfGames = 10,
            int banlistId = -1,
            string archetypeName = "",
            System.DateTime? statisticsFrom = null,
            System.DateTime? statisticsTo = null,
            bool shouldGetDecksFromCache = true,
            bool orderByDescendingByNumberOfGames = false,
            int[] wantedCardsInDeck = null);

        /// <summary>
        /// Decklist with all data included.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Decklist with all data included.</returns>
        Task<Decklist> GetByIdWithAllDataIncluded(int id);
        IQueryable<Decklist> GetDecklistsQueryForBanlistAnalysis(bool shouldBeTracked = true);

        /// <summary>
        /// Renew cache by key. CacheKey: <see cref="CacheKeys.OrderedDecklistsWithContentIncluded"/>
        /// </summary>
        Task UpdateCache();
    }
}