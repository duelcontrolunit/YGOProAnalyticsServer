using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    public interface IDecklistService
    {
        /// <summary>
        /// Finds all decklists based on parameters.
        /// </summary>
        /// <param name="howManyTake">The how many decklists take.</param>
        /// <param name="howManySkip">The how many decklists (results from db) skip.</param>
        /// <param name="banlistId">The banlist identifier.</param>
        /// <param name="archetypeName">Name of the archetype.</param>
        /// <returns>Decklists.</returns>
        Task<System.Collections.Generic.IEnumerable<Decklist>> FindAll(
            int howManyTake,
            int howManySkip,
            int banlistId,
            string archetypeName);

        /// <summary>
        /// Decklist with all data included.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Decklist with all data included.</returns>
        Task<Decklist> GetByIdWithAllDataIncluded(int id);
    }
}