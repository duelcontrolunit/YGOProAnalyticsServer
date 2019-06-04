using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class DecklistWithNumberOfGamesAndWinsDTO
    {
        public DecklistWithNumberOfGamesAndWinsDTO(
            int id,
            string name,
            DateTime whenDecklistWasFirstPlayed,
            int numberOfWins,
            int numberOfGames)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            WhenDecklistWasFirstPlayed = whenDecklistWasFirstPlayed;
            NumberOfWins = numberOfWins;
            NumberOfGames = numberOfGames;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime WhenDecklistWasFirstPlayed { get; set; }
        public int NumberOfWins { get; set; }
        public int NumberOfGames { get; set; }
    }
}
