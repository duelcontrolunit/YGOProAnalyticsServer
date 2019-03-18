using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;

namespace YGOProAnalyticsServer.Services.Builders.Inferfaces
{
    /// <summary>
    /// It provide methods to properly build different types of cards.
    /// </summary>
    public interface ICardBuilder
    {
        /// <summary>It must be called.</summary>
        /// <param name="passCode">YuGiOh Card Passcode.</param>
        /// <param name="name">Card name.</param>
        /// <param name="description">Card description/effect/flavor text.</param>
        /// <param name="type">
        ///     For example: normal monster, trap card, magic card.
        ///     https://db.ygoprodeck.com/api-guide/
        /// </param>
        /// <param name="race">
        ///     For example:
        ///     <para>1) For monster: aqua, machine warrior</para>
        ///     <para>2) For spell: normal, field, quick-spell</para>
        ///     <para>3) For trap: normal, continuous, counter</para>
        ///     https://db.ygoprodeck.com/api-guide/
        /// </param>
        /// <param name="imageUrl">Link to the image of the card.</param>
        /// <param name="smallImageUrl">Link to the small image of the card.</param>
        /// <param name="archetype">Archetype which card belong.</param>
        /// <returns><see cref="CardBuilder"/>.</returns>
        CardBuilder AddBasicCardElements(int passCode, string name, string description, string type, string race, string imageUrl, string smallImageUrl, Archetype archetype);

        ///<summary>
        /// If card is link monster it must be called.
        ///</summary>
        /// <param name="linkValue">Link value.</param>
        /// <param name="topLeftLinkMarker">Top-Left link marker.</param>
        /// <param name="topLinkMarker">Top link marker.</param>
        /// <param name="topRightLinkMarker">Top-Right link marker.</param>
        /// <param name="middleLeftLinkMarker">Middle-Left link marker.</param>
        /// <param name="middleRightLinkMarker">Middle-Right link marker.</param>
        /// <param name="bottomLeftLinkMarker">Bottom-Left link marker.</param>
        /// <param name="bottomLinkMarker">Bottom link marker.</param>
        /// <param name="bottomRightLinkMarker">Bottom-Right link marker.</param>
        /// <returns><see cref="CardBuilder"/>.</returns>
        CardBuilder AddLinkMonsterCardElements(int linkValue, bool topLeftLinkMarker, bool topLinkMarker, bool topRightLinkMarker, bool middleLeftLinkMarker, bool middleRightLinkMarker, bool bottomLeftLinkMarker, bool bottomLinkMarker, bool bottomRightLinkMarker);

        /// <summary>
        /// If card is monster card it must be called.
        /// </summary>
        /// <param name="attack">Attack of the monster.</param>
        /// <param name="defence">For links it shoud be equal 0.</param>
        /// <param name="levelOrRank">Is Rank only for XYZ monsters.</param>
        /// <param name="attribute">Dark, Light, Earth, Wind etc.</param>
        /// <returns><see cref="CardBuilder"/>.</returns>
        CardBuilder AddMonsterCardElements(string attack,string defence,int levelOrRank,string attribute);

        /// <summary>
        /// If card is pendulum monster it must be called.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        /// <returns><see cref="CardBuilder"/>.</returns>
        CardBuilder AddPendulumMonsterCardElements(int scale);

        /// <summary>
        /// Build card. After build is immediately ready to build next card.
        /// </summary>
        /// <returns>Preconfigured card.</returns>
        /// <exception cref="NotProperlyInitializedException">Is thrown when build failed. Should contain information why build failed in message.</exception>
        Card Build();
    }
}