using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Exceptions;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Analyzers.Interfaces;

namespace YGOProAnalyticsServer.Services.Analyzers
{
    /// <<inheritdoc />
    public class ArchetypeAndDecklistAnalyzer : IArchetypeAndDecklistAnalyzer
    {
        private readonly YgoProAnalyticsDatabase _db;
        private readonly List<Archetype> _archetypes;

        /// <summary>Initializes a new instance of the <see cref="ArchetypeAndDecklistAnalyzer"/> class.</summary>
        /// <param name="db">The database.</param>
        public ArchetypeAndDecklistAnalyzer(YgoProAnalyticsDatabase db)
        {
            _db = db;
            _archetypes = db.Archetypes.Include(x=>x.Statistics).ToList();
        }

        /// <<inheritdoc />
        public Archetype GetArchetypeOfTheDecklistWithStatistics(Decklist decklist, DateTime whenDecklistWasUsed)
        {
            if (decklist.Archetype == null)
            {
                decklist.Archetype = _getTheMostUsedArchetypeInDecklist(decklist);
            }

            var statistics = decklist.Archetype.Statistics.FirstOrDefault(x => x.DateWhenArchetypeWasUsed == whenDecklistWasUsed);
            if (statistics == null)
            {
                statistics = new ArchetypeStatistics(decklist.Archetype, whenDecklistWasUsed);
                decklist.Archetype.Statistics.Add(statistics);
            }

            return decklist.Archetype;
        }

        private Archetype _getTheMostUsedArchetypeInDecklist(Decklist decklist)
        {
            List<Card> fullDeck = new List<Card>();
            foreach (var card in decklist.MainDeck)
            {
                fullDeck.Add(card);
            }

            foreach (var card in decklist.ExtraDeck)
            {
                fullDeck.Add(card);
            }

            foreach (var card in decklist.SideDeck)
            {
                fullDeck.Add(card);
            }

            if (fullDeck.Count == 0)
            {
                throw new EmptyDecklistException("The decklist given in the parameter contains no cards in Main, Extra and Side Deck.");
            }

            List<Archetype> archetypes = fullDeck.ConvertAll<Archetype>(x => x.Archetype);
            if (archetypes.Where(x => x.Name == Archetype.Default).Count() / fullDeck.Count() >= 0.8)
            {
                return _archetypes
                    .FirstOrDefault(x => x.Name == Archetype.Default)
                    ?? new Archetype(Archetype.Default, true);
            }

            archetypes.RemoveAll(x => x.Name == Archetype.Default);
            List<Archetype> uniqueArchetypesInDeck = archetypes.GroupBy(x => x.Name).Select(x => x.First()).ToList();
            Dictionary<Archetype, int> archetypesDictionary = uniqueArchetypesInDeck.ToDictionary(
                p => p, p => archetypes.Where(x => x.Name == p.Name)
                   .Count()).OrderByDescending(x => x.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);


            if (archetypesDictionary.ElementAt(0).Value > (int)(archetypes.Count * 0.5))
            {
                return archetypesDictionary.ElementAt(0).Key;
            }
            else if (archetypesDictionary.Count >= 2)
            {
                string newArchetypeName = new StringBuilder(archetypesDictionary.ElementAt(0).Key.Name).Append(' ')
                    .Append(archetypesDictionary.ElementAt(1).Key.Name).ToString();
                var archetype = _archetypes.Where(x => x.Name == newArchetypeName).FirstOrDefault();
                archetype = archetype ?? new Archetype(newArchetypeName, false);

                if (!_archetypes.Contains(archetype))
                {
                    _archetypes.Add(archetype);
                }

                return archetype;
            }

            return _archetypes
                .FirstOrDefault(x => x.Name == Archetype.Default)
                ?? new Archetype(Archetype.Default, true);
        }

        /// <summary>Removes the duplicate decklists from list of decklists.</summary>
        /// <param name="decklist">The decklist.</param>
        /// <param name="listOfDecks">The list of decks.</param>
        /// <returns>Amount of duplicates removed.</returns>
        public NumberOfDuplicatesWithListOfDecklists RemoveDuplicateDecklistsFromListOfDecklists(
            Decklist decklist,
            IEnumerable<Decklist> listOfDecks)
        {
            int duplicateCount = 0;
            List<Decklist> listWithoutDuplicates = new List<Decklist>();
            foreach (var deck in listOfDecks)
            {
                if (CheckIfDecklistsAreDuplicate(decklist, deck))
                {
                    if (duplicateCount == 0)
                    {
                        listWithoutDuplicates.Add(decklist);
                    }

                    duplicateCount++;
                }
                else
                {
                    listWithoutDuplicates.Add(deck);
                }
            }

            return new NumberOfDuplicatesWithListOfDecklists(duplicateCount, listWithoutDuplicates, decklist);
        }

        /// <summary>
        /// Checks if decklists are duplicate.
        /// </summary>
        /// <param name="decklist1">The decklist1.</param>
        /// <param name="decklist2">The decklist2.</param>
        /// <returns></returns>
        public bool CheckIfDecklistsAreDuplicate(Decklist decklist1, Decklist decklist2)
        {
            if (decklist2.MainDeck.Count != decklist1.MainDeck.Count
                || decklist2.ExtraDeck.Count != decklist1.ExtraDeck.Count
                || decklist2.SideDeck.Count != decklist1.SideDeck.Count)
            {
                return false;
            }

            var mainDeck1 = new List<Card>();
            foreach (var card in decklist1.MainDeck)
            {
                mainDeck1.Add(card);
            }

            var mainDeck2 = new List<Card>();
            foreach (var card in decklist2.MainDeck)
            {
                mainDeck2.Add(card);
            }

            for (int i = 0; i < decklist2.MainDeck.Count; i++)
            {
                if (mainDeck1[i].PassCode != mainDeck2[i].PassCode)
                {
                    return false;
                }
            }

            var extraDeck1 = new List<Card>();
            foreach (var card in decklist1.ExtraDeck)
            {
                extraDeck1.Add(card);
            }

            var extraDeck2 = new List<Card>();
            foreach (var card in decklist2.ExtraDeck)
            {
                extraDeck2.Add(card);
            }

            for (int i = 0; i < decklist2.ExtraDeck.Count; i++)
            {
                if (extraDeck1[i].PassCode != extraDeck2[i].PassCode)
                {
                    return false;
                }
            }

            var sideDeck1 = new List<Card>();
            foreach (var card in decklist1.SideDeck)
            {
                sideDeck1.Add(card);
            }

            var sideDeck2 = new List<Card>();
            foreach (var card in decklist2.SideDeck)
            {
                sideDeck2.Add(card);
            }

            for (int i = 0; i < decklist2.SideDeck.Count; i++)
            {
                if (sideDeck1[i].PassCode != sideDeck2[i].PassCode)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
