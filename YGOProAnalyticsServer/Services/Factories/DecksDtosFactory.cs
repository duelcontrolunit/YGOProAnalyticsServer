using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.DTOs.Interfaces;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServer.Services.Factories
{
    /// <seealso cref="YGOProAnalyticsServer.Services.Factories.Interfaces.IDecksDtosFactory" />
    public class DecksDtosFactory : IDecksDtosFactory
    {
        readonly ICardDtosFactory _cardDtosFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecksDtosFactory"/> class.
        /// </summary>
        /// <param name="cardDtosFactory">The card dtos factory.</param>
        /// <exception cref="ArgumentNullException">cardDtosFactory</exception>
        public DecksDtosFactory(ICardDtosFactory cardDtosFactory)
        {
            _cardDtosFactory = cardDtosFactory ?? throw new ArgumentNullException(nameof(cardDtosFactory));
        }

        /// <inheritdoc />
        public MainDeckDTO CreateMainDeckDto(Decklist decklist)
        {
            var mainDeckDto = new MainDeckDTO();
            foreach (var card in decklist.MainDeck)
            {
                var lowerCardType = card.Type.ToLower();
                if (lowerCardType.Contains("monster"))
                {
                    handleMonsterFor(mainDeckDto, card, lowerCardType);
                }
                else
                {
                    handleTraps(mainDeckDto, card, lowerCardType);
                    handleSpells(mainDeckDto, card, lowerCardType);
                }
            }

            return mainDeckDto;
        }

        /// <inheritdoc />
        public ExtraDeckDTO CreateExtraDeckDto(Decklist decklist)
        {
            var extraDeckDto = new ExtraDeckDTO();
            foreach (var card in decklist.ExtraDeck)
            {
                var lowerCardType = card.Type.ToLower();
                handleMonsterFor(extraDeckDto, card, lowerCardType);
            }

            return extraDeckDto;
        }

        /// <inheritdoc />
        public DeckDTO CreateDeckDto(Decklist decklist)
        {
            var deck = new DeckDTO();
            foreach (var card in decklist.SideDeck)
            {
                var lowerCardType = card.Type.ToLower();
                if (lowerCardType.Contains("monster"))
                {
                    handleMonsterFor(deck, card, lowerCardType);
                }
                else
                {
                    handleTraps(deck, card, lowerCardType);
                    handleSpells(deck, card, lowerCardType);
                }
            }

            return deck;
        }

        public DeckDTO CreateDeckDto(IEnumerable<Card> cards)
        {
            var deck = new DeckDTO();
            foreach (var card in cards)
            {
                var lowerCardType = card.Type.ToLower();
                if (lowerCardType.Contains("monster"))
                {
                    handleMonsterFor(deck, card, lowerCardType);
                }
                else
                {
                    handleTraps(deck, card, lowerCardType);
                    handleSpells(deck, card, lowerCardType);
                }
            }

            return deck;
        }

        protected void handleMonsterFor(DeckDTO deck, Card card, string lowerCardType)
        {
            handleXYZPendulumMonster(deck, card, lowerCardType);
            handlePendulumSynchroMonster(deck, card, lowerCardType);
            handlePendulumFusionMonster(deck, card, lowerCardType);
            handleXYZMonster(deck, card, lowerCardType);
            handleSynchroMonster(deck, card, lowerCardType);
            handleFusionMonster(deck, card, lowerCardType);
            handleLinkMonster(deck, card, lowerCardType);

            handleNonExtraPendulumNormalMonster(deck, card, lowerCardType);
            handleNonExtraPendulumEffectMonster(deck, card, lowerCardType);
            handleRitualMonster(deck, card, lowerCardType);
            handleEffectMonster(deck, card, lowerCardType);
            handleNormalMonster(deck, card, lowerCardType);
        }

        protected virtual void handleMonsterFor(MainDeckDTO mainDeckDto, Card card, string lowerCardType)
        {
            handleNonExtraPendulumNormalMonster(mainDeckDto, card, lowerCardType);
            handleNonExtraPendulumEffectMonster(mainDeckDto, card, lowerCardType);
            handleRitualMonster(mainDeckDto, card, lowerCardType);
            handleEffectMonster(mainDeckDto, card, lowerCardType);
            handleNormalMonster(mainDeckDto, card, lowerCardType);
        }

        protected virtual void handleMonsterFor(ExtraDeckDTO extraDeckDTO, Card card, string lowerCardType)
        {
            handleXYZPendulumMonster(extraDeckDTO, card, lowerCardType);
            handlePendulumSynchroMonster(extraDeckDTO, card, lowerCardType);
            handlePendulumFusionMonster(extraDeckDTO, card, lowerCardType);
            handleXYZMonster(extraDeckDTO, card, lowerCardType);
            handleSynchroMonster(extraDeckDTO, card, lowerCardType);
            handleFusionMonster(extraDeckDTO, card, lowerCardType);
            handleLinkMonster(extraDeckDTO, card, lowerCardType);
        }

        protected void handleXYZPendulumMonster<TXYZPendulumContainer>(
            TXYZPendulumContainer container,
            Card card,
            string lowerCardType)
            where TXYZPendulumContainer : IXYZPendulumContainer
        {
            if(lowerCardType.Contains("xyz") && lowerCardType.Contains("pendulum"))
            {
                container.XYZPendulumMonsters.Add(
                    _cardDtosFactory.CreatePendulumMonsterCardDto(card)
                );
            }
        }

        protected void handlePendulumSynchroMonster<TSynchroPendulumContainer>(
           TSynchroPendulumContainer container,
           Card card,
           string lowerCardType)
           where TSynchroPendulumContainer : ISynchroPendulumMonsterContainer
        {
            if (lowerCardType.Contains("synchro") && lowerCardType.Contains("pendulum"))
            {
                container.SynchroPendulumMonsters.Add(
                    _cardDtosFactory.CreatePendulumMonsterCardDto(card)
                );
            }
        }

        protected void handlePendulumFusionMonster<TFusionPendulumContainer>(
          TFusionPendulumContainer container,
          Card card,
          string lowerCardType)
          where TFusionPendulumContainer : IFusionPendulumMonsterContainer
        {
            if (lowerCardType.Contains("fusion") && lowerCardType.Contains("pendulum"))
            {
                container.FusionPendulumMonsters.Add(
                    _cardDtosFactory.CreatePendulumMonsterCardDto(card)
                );
            }
        }

        protected void handleXYZMonster<TXYZMonstersContainer>(
          TXYZMonstersContainer container,
          Card card,
          string lowerCardType)
          where TXYZMonstersContainer : IXYZMonstersContainer
        {
            if (lowerCardType.Contains("xyz") && !lowerCardType.Contains("pendulum"))
            {
                container.XYZMonsters.Add(
                    _cardDtosFactory.CreateMonsterCardDto(card)
                );
            }
        }

        protected void handleSynchroMonster<TSynchroMonstersContainer>(
         TSynchroMonstersContainer container,
         Card card,
         string lowerCardType)
         where TSynchroMonstersContainer : ISynchroMonstersContainer
        {
            if (lowerCardType.Contains("synchro") && !lowerCardType.Contains("pendulum"))
            {
                container.SynchroMonsters.Add(
                    _cardDtosFactory.CreateMonsterCardDto(card)
                );
            }
        }

        protected void handleFusionMonster<TFusionMonstersContainer>(
         TFusionMonstersContainer container,
         Card card,
         string lowerCardType)
         where TFusionMonstersContainer : IFusionMonstersContainer
        {
            if (lowerCardType.Contains("fusion") && !lowerCardType.Contains("pendulum"))
            {
                container.FusionMonsters.Add(
                    _cardDtosFactory.CreateMonsterCardDto(card)
                );
            }
        }
        protected void handleLinkMonster<TLinkMonstersContainer>(
         TLinkMonstersContainer container,
         Card card,
         string lowerCardType)
         where TLinkMonstersContainer : ILinkMonstersContainer
        {
            if (lowerCardType.Contains("link") && !lowerCardType.Contains("pendulum"))
            {
                container.LinkMonsters.Add(
                    _cardDtosFactory.CreateLinkMonsterCardDto(card)
                );
            }
        }


        protected virtual void handleEffectMonster<TEffectMonstersContainer>(
            TEffectMonstersContainer container,
            Card card,
            string lowerCardType)
            where TEffectMonstersContainer : IEffectMonsterContainer
        {
            if ((!lowerCardType.Contains("normal"))
                && isNotAnyExtraMonsterType(lowerCardType)
                && !lowerCardType.Contains("pendulum"))
            {
                container.EffectMonsters.Add(
                    _cardDtosFactory.CreateMonsterCardDto(card)
                );
            }
        }

        protected virtual void handleNormalMonster<TNormalMonstersContainer>(
            TNormalMonstersContainer container,
            Card card,
            string lowerCardType)
            where TNormalMonstersContainer : INormalMonstersContainer
        {
            if (!lowerCardType.Contains("normal") || lowerCardType.Contains("pendulum"))
            {
                return;
            }

            container.NormalMonsters.Add(
                _cardDtosFactory.CreateMonsterCardDto(card)
            );
        }

        protected virtual void handleRitualMonster<TRitualMonstersContainer>(
            TRitualMonstersContainer container,
            Card card,
            string lowerCardType)
            where TRitualMonstersContainer : IRitualMonstersContainer
        {
            if (lowerCardType.Contains("ritual") 
                && isNotAnyExtraMonsterType(lowerCardType)
                && !lowerCardType.Contains("pendulum"))
            {
                container.RitualMonsters.Add(
                    _cardDtosFactory.CreateMonsterCardDto(card)
                );
            }
        }

        protected virtual void handleNonExtraPendulumNormalMonster<T>(T container, Card card, string lowerCardType)
            where T : INonExtraPendulumNormalMonster
        {
            if (lowerCardType.Contains("pendulum") && isNotAnyExtraMonsterType(lowerCardType) && lowerCardType.Contains("normal"))
            {
                container.PendulumNormalMonsters.Add(
                    _cardDtosFactory.CreatePendulumMonsterCardDto(card)
                );
            }
        }

        protected virtual void handleNonExtraPendulumEffectMonster<T>(T container, Card card, string lowerCardType)
            where T : INonExtraPendulumEffectMonster
        {
            if (lowerCardType.Contains("pendulum") && isNotAnyExtraMonsterType(lowerCardType) && !lowerCardType.Contains("normal"))
            {
                container.PendulumEffectMonsters.Add(
                    _cardDtosFactory.CreatePendulumMonsterCardDto(card)
                );
            }
        }

        private bool isNotAnyExtraMonsterType(string lowerCardType)
        {
            return !lowerCardType.Contains("xyz")
                && !lowerCardType.Contains("synchro")
                && !lowerCardType.Contains("fusion")
                && !lowerCardType.Contains("link");
        }

        protected virtual void handleSpells<TSpecificCardsContainer>(
            TSpecificCardsContainer container,
            Card card,
            string lowerCardType)
            where TSpecificCardsContainer : ISpellCardsContainer
        {
            if (lowerCardType == "spell card")
            {
                container.SpellCards.Add(
                    _cardDtosFactory.CreateCardDto(card)
                );
            }
        }

        protected virtual void handleTraps<TSpecificCardsContainer>(
            TSpecificCardsContainer container,
            Card card,
            string lowerCardType)
           where TSpecificCardsContainer : ITrapCardsContainer
        {
            if (lowerCardType == "trap card")
            {
                container.TrapCards.Add(
                    _cardDtosFactory.CreateCardDto(card)
                );
            }
        }
    }
}
