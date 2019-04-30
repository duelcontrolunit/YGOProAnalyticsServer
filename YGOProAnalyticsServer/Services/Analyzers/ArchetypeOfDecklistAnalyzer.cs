using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServer.Services.Analyzers
{

    /// <<inheritdoc />
    public class ArchetypeOfDecklistAnalyzer : IArchetypeOfDecklistAnalyzer
    {
        private readonly YgoProAnalyticsDatabase _db;

        /// <summary>Initializes a new instance of the <see cref="ArchetypeOfDecklistAnalyzer"/> class.</summary>
        /// <param name="db">The database.</param>
        public ArchetypeOfDecklistAnalyzer(YgoProAnalyticsDatabase db)
        {
            _db = db;
        }

        /// <<inheritdoc />
        public async Task<Decklist> SetDecklistArchetypeFromArchetypeCardsUsedInIt(Decklist decklist)
        {
            decklist.Archetype = await _getNameOfMostUsedArchetypeInDecklist(decklist);
            return decklist;
        }

        private async Task<Archetype> _getNameOfMostUsedArchetypeInDecklist(Decklist decklist)
        {
            List<Card> fullDeck = decklist.MainDeck.Concat(decklist.ExtraDeck).Concat(decklist.SideDeck).ToList();
            if (fullDeck.Count == 0)
            {
                throw new EmptyDecklistException("The decklist given in the parameter contains no cards in Main, Extra and Side Deck.");
            }
            List<Archetype> archetypes = fullDeck.ConvertAll<Archetype>(x => x.Archetype);

            if (archetypes.Where(x => x.Name == Archetype.Default).Count() / fullDeck.Count() >= 0.8)
            {
                return new Archetype(Archetype.Default, true);
            }

            archetypes.RemoveAll(x => x.Name == Archetype.Default);
            List<Archetype> uniqueArchetypesInDeck = archetypes.GroupBy(x => x.Name).Select(x => x.First()).ToList();
            Dictionary<Archetype, int> archetypesDictionary = uniqueArchetypesInDeck.ToDictionary(
                p => p, p => archetypes.Where(x => x.Name == p.Name)
                   .Count()).OrderByDescending(x => x.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            if (archetypesDictionary.Count >= 1)
            {

                if (archetypesDictionary.ElementAt(0).Value > (int)(archetypes.Count / 2))
                {
                    return archetypesDictionary.ElementAt(0).Key;
                }
                else if (archetypesDictionary.Count >= 2)
                {
                    string newArchetypeName = new StringBuilder(archetypesDictionary.ElementAt(0).Key.Name).Append(' ')
                        .Append(archetypesDictionary.ElementAt(1).Key.Name).ToString();
                    var archetype = _db.Archetypes.Where(x => x.Name == newArchetypeName).FirstOrDefault();
                    archetype = archetype ?? new Archetype(newArchetypeName, false);

                    if (!_db.Archetypes.Contains(archetype))
                    {
                        _db.Archetypes.Add(archetype);
                    }
                    await _db.SaveChangesAsync();
                    return archetype;
                }
            }
            return new Archetype(Archetype.Default, true);
        }
    }
}
