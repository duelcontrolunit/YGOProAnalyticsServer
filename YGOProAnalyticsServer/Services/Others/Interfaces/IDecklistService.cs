﻿using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    public interface IDecklistService
    {
        /// <summary>
        /// Finds all by criteria.
        /// </summary>
        /// <param name="howManyTake">The how many take.</param>
        /// <param name="howManySkip">The how many skip.</param>
        /// <param name="minNumberOfGames">The minimum number of games.</param>
        /// <param name="banlistId">The banlist identifier.</param>
        /// <param name="archetypeName">Name of the archetype.</param>
        /// <param name="statisticsFrom">The statistics from.</param>
        /// <param name="statisticsTo">The statistics to.</param>
        /// <param name="shouldGetDecksFromCache">Should take decks from cache?</param>
        Task<System.Collections.Generic.IEnumerable<Decklist>> FindAll(
            int howManyTake,
            int howManySkip,
            int minNumberOfGames = 10,
            int banlistId = -1,
            string archetypeName = "",
            System.DateTime? statisticsFrom = null,
            System.DateTime? statisticsTo = null,
            bool shouldGetDecksFromCache = true);

        /// <summary>
        /// Decklist with all data included.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Decklist with all data included.</returns>
        Task<Decklist> GetByIdWithAllDataIncluded(int id);

        /// <summary>
        /// Renew cache by key. CacheKey: <see cref="CacheKeys.OrderedDecklistsWithContentIncluded"/>
        /// </summary>
        void UpdateCache();
    }
}