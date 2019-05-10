using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Factories;
using YGOProAnalyticsServerTests.TestingHelpers;

namespace YGOProAnalyticsServerTests.Services.Factories
{
    class DecksDtosFactoryTests
    {
        DecksDtosFactory _deckDtosFactory;
        CardDtosFactory _cardDtosFactory;
        CardsAndDecksHelper _helper = new CardsAndDecksHelper();

        [SetUp]
        public void SetUp()
        {
            _cardDtosFactory = new CardDtosFactory();
            _deckDtosFactory = new DecksDtosFactory(_cardDtosFactory);
        }

        [Test]
        public void CreateMainDeckDto_WeGetDTO()
        {
            var decklist = _getDecklistWithCards();
            var dto = _deckDtosFactory.CreateMainDeckDto(decklist);

            Assert.Multiple(()=> {
                Assert.AreEqual(6, dto.EffectMonsters);
            });
            
        }

        private Decklist _getDecklistWithCards()
        {
            var archetype = new Archetype("ValidArchetype", false);
            var decklist = _helper.GetValidDecklistWithStatistics(archetype);

            _addToDecklistSpellAndTrapMonster(archetype, decklist);
            _addToDecklistNonPendulumOrLinkMonsters(archetype, decklist);
            _addToDecklistPendulumMonsters(archetype, decklist);

            return decklist;
        }

        private void _addToDecklistNonPendulumOrLinkMonsters(Archetype archetype, Decklist decklist)
        {
            var nonPendulumOrLinkMonstersTypes = new string[]{ "Effect Monster", "Flip Effect Monster",
                "Flip Tuner Effect Monster", "Gemini Monster", "Normal Monster", "Normal Tuner Monster",
                "Ritual Effect Monster", "Ritual Monster", "Spirit Monster", "Toon Monster", "Tuner Monster",
                "Union Effect Monster", "Union Tuner Effect Monster", "Fusion Monster", "Synchro Monster",
                "Synchro Tuner Monster", "XYZ Monster"
            };
            
            foreach(string type in nonPendulumOrLinkMonstersTypes)
            {
                if(type.ToLower().Contains("synchro") 
                    || type.ToLower().Contains("xyz")
                    || type.ToLower().Contains("fusion"))
                {
                    decklist.ExtraDeck.Add(
                        _getMonsterCard(archetype, type)
                    );
                }
                else
                {
                    decklist.MainDeck.Add(
                        _getMonsterCard(archetype, type)
                    );
                }
            }
        }

        private void _addToDecklistPendulumMonsters(Archetype archetype, Decklist decklist)
        {
            var pendulumMonsters = new string[]{ "Pendulum Effect Monster", "Pendulum Flip Effect Monster",
                "Pendulum Normal Monster", "Pendulum Tuner Effect Monster", "Pendulum Effect Fusion Monster",
                "Synchro Pendulum Effect Monster", "XYZ Pendulum Effect Monster"
            };

            foreach (string type in pendulumMonsters)
            {
                if (type.ToLower().Contains("synchro")
                   || type.ToLower().Contains("xyz")
                   || type.ToLower().Contains("fusion"))
                {
                    decklist.ExtraDeck.Add(
                        _getPendulumMonsterCard(archetype, type)
                    );
                }
                else
                {
                    decklist.MainDeck.Add(
                        _getPendulumMonsterCard(archetype, type)
                    );
                }
            }
        }

        private void _addToDecklistSpellAndTrapMonster(Archetype archetype, Decklist decklist)
        {
            decklist.MainDeck.Add(
                _helper.GetCard(archetype, "Spell Card")
            );

            decklist.MainDeck.Add(
               _helper.GetCard(archetype, "Trap Card")
            );
        }

        private Card _getMonsterCard(Archetype archetype, string type)
        {
            var card = _helper.GetCard(archetype, type);
            card.MonsterCard = _helper.GetMonsterCard(card);
            return card;
        }

        private Card _getPendulumMonsterCard(Archetype archetype, string type)
        {
            var pendulumMonsterCard = _getMonsterCard(archetype, type);
            pendulumMonsterCard.MonsterCard.PendulumMonsterCard = _helper.GetPendulumMonsterCard(pendulumMonsterCard.MonsterCard);

            return pendulumMonsterCard;
        }
    }
}
