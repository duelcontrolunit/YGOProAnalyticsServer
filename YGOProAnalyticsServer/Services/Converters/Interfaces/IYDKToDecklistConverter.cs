using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Converters.Interfaces
{
    /// <summary>
    /// It convert YDK String into a Decklist containing Main Deck, Side Deck and Extra Deck.
    /// </summary>
    public interface IYDKToDecklistConverter
    {
        /// <summary>
        /// Converts the specified ydk string to a new Deck containing Lists of Cards in it.
        /// </summary>
        /// <param name="ydkString">The ydk string.</param>
        /// <returns>A new Deck containing Lists of Cards in it.</returns>
        Decklist Convert(string ydkString);
    }
}