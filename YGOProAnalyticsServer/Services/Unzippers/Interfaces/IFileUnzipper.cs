using System.Collections.Generic;
using YGOProAnalyticsServer.Models;

namespace YGOProAnalyticsServer.Services.Unzippers.Interfaces
{
    public interface IFileUnzipper
    {
        /// <summary>Gets the decklists from zip.</summary>
        /// <param name="decksZipFilePath">The decks zip file path.</param>
        /// <returns>List of <see cref="DecklistWithName"/></returns>
        List<DecklistWithName> GetDecksFromZip(string decksZipFilePath);

        /// <summary>Gets the duel log from zip.</summary>
        /// <param name="duelLogZipFilePath">The duel log zip file path.</param>
        /// <returns>Duel Log in string form</returns>
        string GetDuelLogFromZip(string duelLogZipFilePath);
    }
}