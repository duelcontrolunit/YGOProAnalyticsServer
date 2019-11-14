using System.Threading.Tasks;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// Provides conversion from beta card to official.
    /// </summary>
    public interface IBetaCardToOfficialConverter
    {
        /// <summary>
        /// Updates the cards from beta to official.
        /// </summary>
        /// <returns>Task</returns>
        Task UpdateCardsFromBetaToOfficial();
    }
}