using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Factories.Interfaces;

namespace YGOProAnalyticsServer.Services.Factories
{
    public class DecksDtosFactory : IDecksDtosFactory
    {
        readonly ICardDtosFactory _cardDtosFactory;

        public DecksDtosFactory(ICardDtosFactory cardDtosFactory)
        {
            _cardDtosFactory = cardDtosFactory ?? throw new ArgumentNullException(nameof(cardDtosFactory));
        }

        public ExtraDeckDTO CreateExtraDeckDto(Decklist decklist)
        {
            throw new NotImplementedException();
        }

        public MainDeckDTO CreateMainDeckDto(Decklist decklist)
        {
            var mainDeckDto = new MainDeckDTO();
            foreach (var card in decklist.MainDeck)
            {
                var cardType = card.Type.ToLower();
                if (cardType.Contains("monster"))
                {
                    if (cardType.Contains("pendulum"))
                    {
                        if (cardType.Contains("effect"))
                        {
                            mainDeckDto.PendulumEffectMonsters.Add(
                                _cardDtosFactory.CreatePendulumMonsterCardDto(card)
                            );
                        }
                        else
                        {
                            mainDeckDto.PendulumNormalMonsters.Add(
                                _cardDtosFactory.CreatePendulumMonsterCardDto(card)
                            );
                        }
                    }
                    else if (cardType.Contains("ritual"))
                    {
                        mainDeckDto.RitualMonsters.Add(
                            _cardDtosFactory.CreateMonsterCardDto(card)
                        );
                    }
                    else if (cardType.Contains("effect"))
                    {
                        mainDeckDto.EffectMonsters.Add(
                            _cardDtosFactory.CreateMonsterCardDto(card)
                        );
                    }
                    else
                    {
                        mainDeckDto.NormalMonsters.Add(
                            _cardDtosFactory.CreateMonsterCardDto(card)
                        );
                    }
                }
                else
                {
                    if (cardType.Contains("spell"))
                    {
                        mainDeckDto.SpellCards.Add(
                            _cardDtosFactory.CreateCardDto(card)
                        );
                    }
                    else if (cardType.Contains("trap"))
                    {
                        mainDeckDto.TrapCards.Add(
                            _cardDtosFactory.CreateCardDto(card)
                        );
                    }
                }
            }

            return mainDeckDto;
        }

        public DeckDTO CreateDeckDto(Decklist decklist)
        {
            throw new NotImplementedException();
        }
    }
}
