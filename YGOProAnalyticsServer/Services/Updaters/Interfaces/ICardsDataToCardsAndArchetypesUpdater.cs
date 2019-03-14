using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Updaters.Interfaces
{
    /// <summary>
    /// Provide methods for update cards and archetypes
    /// </summary>
    public interface ICardsDataToCardsAndArchetypesUpdater
    {
        /// <summary>
        /// Updates the cards and archetypes.
        /// </summary>
        /// <param name="URL">The URL.</param>
        Task UpdateCardsAndArchetypes(string URL);
    }
}