using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Factories.Interfaces
{
    /// <summary>
    /// Provide methods to create different card DTOs
    /// </summary>
    public interface ICardDtosFactory
    {
        /// <summary>
        /// Useful when you must create dto for spell or trap card.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns><see cref="CardDTO"/>.</returns>
        CardDTO CreateCardDto(Card card);

        /// <summary>
        /// Useful when you must create dto for link monster. 
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns><see cref="LinkMonsterCardDTO"/></returns>
        LinkMonsterCardDTO CreateLinkMonsterCardDto(Card card);

        /// <summary>
        /// Useful when you must create dto for monster different than link or pendulum. 
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns><see cref="MonsterCardDTO"/></returns>
        MonsterCardDTO CreateMonsterCardDto(Card card);

        /// <summary>
        /// Useful when you must create dto for pendulum monster (fore example normal pendulum, XYZ Pendulum etc).
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns><see cref="PendulumMonsterCardDTO"/></returns>
        PendulumMonsterCardDTO CreatePendulumMonsterCardDto(Card card);
    }
}