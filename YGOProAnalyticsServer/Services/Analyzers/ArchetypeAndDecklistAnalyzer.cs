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
        private const int _minimumNumberOfArchetypeCardsForTypeArchetype = 15;
        private const int _minimumNumberOfArchetypeCardsForArchetype = 9;
        private const int _minimumNumberOfCardsToCheckForArchetype = 5;
        private readonly YgoProAnalyticsDatabase _db;
        private readonly List<Archetype> _archetypes;

        /// <summary>Initializes a new instance of the <see cref="ArchetypeAndDecklistAnalyzer"/> class.</summary>
        /// <param name="db">The database.</param>
        public ArchetypeAndDecklistAnalyzer(YgoProAnalyticsDatabase db)
        {
            _db = db;
            _archetypes = db.Archetypes.Include(x => x.Statistics).ToList();
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

            List<Archetype> archetypes = fullDeck.ConvertAll(x => x.Archetype);
            IEnumerable<Archetype> uniqueArchetypesInDeck = archetypes.GroupBy(x => x.Name).Select(x => x.First());
            Dictionary<Archetype, int> archetypesDictionary = uniqueArchetypesInDeck.ToDictionary(
                p => p, p => archetypes.Where(x => x.Id == p.Id)
                   .Count()).OrderByDescending(x => x.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            if (archetypesDictionary.ElementAt(0).Key.Name == Archetype.Default)
            {
                if (archetypesDictionary.ElementAt(0).Value > (int)(archetypes.Count * 0.8))
                {
                    return _checkForPossibleTypeArchetype(fullDeck);
                }
            }

            if (uniqueArchetypesInDeck.Any(x => x.Name != Archetype.Default))
            {
                archetypes.RemoveAll(x => x.Name == Archetype.Default);
                var defaultArchetype = archetypesDictionary.FirstOrDefault(x => x.Key.Name == Archetype.Default).Key;
                if (defaultArchetype != null)
                {
                    archetypesDictionary.Remove(defaultArchetype);
                }
            }
            //remove archetypes that are lowly represented.
            foreach (var item in archetypesDictionary.Where(x => x.Value <= _minimumNumberOfCardsToCheckForArchetype).ToArray())
            {
                archetypesDictionary.Remove(item.Key);
            }

            if (archetypesDictionary.Count == 0)
            {
                return _checkForPossibleTypeArchetype(fullDeck);
            }

            if (archetypesDictionary.ElementAt(0).Value > (int)(archetypes.Count * 0.5))
            {
                return archetypesDictionary.ElementAt(0).Key;
            }
            else
            {
                if (archetypesDictionary.Count >= 2)
                {
                    int amountOfCardsWith2MainArchetypes = archetypesDictionary.ElementAt(0).Value +
                        archetypesDictionary.ElementAt(1).Value;

                    if (amountOfCardsWith2MainArchetypes >= (int)(archetypes.Count * 0.4))
                    {
                        string newArchetypeName = new StringBuilder(archetypesDictionary
                            .ElementAt(0).Key.Name).Append(' ')
                            .Append(archetypesDictionary.ElementAt(1).Key.Name).ToString();
                        var archetype = _archetypes.Where(x => x.Name == newArchetypeName)
                            .FirstOrDefault();
                        archetype = archetype ?? new Archetype(newArchetypeName, false);

                        if (!_archetypes.Contains(archetype))
                        {
                            _archetypes.Add(archetype);
                        }

                        return archetype;
                    }
                }
                else
                {
                    if (archetypesDictionary.ElementAt(0).Value > _minimumNumberOfArchetypeCardsForArchetype)
                    {
                        return archetypesDictionary.ElementAt(0).Key;
                    }
                }
            }

            return _checkForPossibleTypeArchetype(fullDeck);
        }

        private Archetype _checkForPossibleTypeArchetype(List<Card> fullDeck)
        {
            var potentialArchetype = _checkForMostUsedType(fullDeck);
            potentialArchetype = potentialArchetype
                ?? _archetypes
                .FirstOrDefault(x => x.Name == Archetype.Default)
                ?? new Archetype(Archetype.Default, true);
            if (!_archetypes.Contains(potentialArchetype))
            {
                _archetypes.Add(potentialArchetype);
            }

            return potentialArchetype;
        }

        private Archetype _checkForMostUsedType(List<Card> fullDeck)
        {
            List<string> races = fullDeck.Where(x => _checkIfRaceIsMonster(x.Race))
            .ToList().ConvertAll<string>(x => x.Race);
            IEnumerable<string> uniqueRaces = races.GroupBy(x => x).Select(x => x.First());
            Dictionary<string, int> racesDictionary = uniqueRaces
                .ToDictionary(p => p, p => races.Where(x => x == p)
                .Count()).OrderByDescending(x => x.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var item in racesDictionary.Where(x => x.Value <= _minimumNumberOfCardsToCheckForArchetype).ToArray())
            {
                racesDictionary.Remove(item.Key);
            }

            if (racesDictionary.Count == 0) return null;

            if (racesDictionary.Count >= 2 &&
                racesDictionary.ElementAt(0).Value == racesDictionary.ElementAt(1).Value)
            {
                return null;
            }

            if (racesDictionary.ElementAt(0).Value > _minimumNumberOfArchetypeCardsForTypeArchetype)
            {
                var archetype = _archetypes.Where(x => x.Name == racesDictionary.ElementAt(0).Key).FirstOrDefault();
                return archetype ?? new Archetype(String.Format("{0}-Type", racesDictionary.ElementAt(0).Key), false);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method used to detect if a card is a monster based on it's race. (Checks if it's not any spell or trap race.)
        /// other solution would be checking if Card.MonsterCard != null, but this would require including Cards and thenIncluding MonsterCards.
        /// </summary>
        /// <param name="race">The race.</param>
        /// <returns>True if it's a monster race. Otherwise false</returns>
        private bool _checkIfRaceIsMonster(string race)
        {
            return race != "Normal" && race != "Field" && race != "Equip" && race != "Continuous" && race != "Quick-Play" && race != "Ritual" && race != "Counter";
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
