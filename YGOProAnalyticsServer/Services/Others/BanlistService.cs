using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    public class BanlistService : IBanlistService
    {
        public bool CanDeckBeUsedOnGivenBanlist(Decklist decklist, Banlist banlist)
        {
            var countedCards = _countCards(decklist);

            bool isForbiddenCardInDecklist = _deckContainsNotAllowedCards(
                banlist.ForbiddenCards,
                countedCards,
                (card, countedCard) => { return true; });

            bool isMoreThanOneCopyOfLimitedCardInDeck = _deckContainsNotAllowedCards(
                banlist.LimitedCards,
                countedCards,
                (card, countedCard) =>
                {
                    return countedCard.NumberOfCopies > 1;
                });

            bool isMoreThanTwoCopiesOfSemiLimitedCardInDeck = _deckContainsNotAllowedCards(
              banlist.SemiLimitedCards,
              countedCards,
              (card, countedCard) =>
              {
                  return countedCard.NumberOfCopies > 2;
              });

            return !isForbiddenCardInDecklist
                   && !isMoreThanOneCopyOfLimitedCardInDeck
                   && !isMoreThanTwoCopiesOfSemiLimitedCardInDeck;
        }

        private bool _deckContainsNotAllowedCards(
            ICollection<Card> cardsOnBanlist,
            IEnumerable<CardWithInfoAboutNumberOfCopiesInDeck> countedCards,
            Func<Card, CardWithInfoAboutNumberOfCopiesInDeck, bool> additionalCondition)
        {
            foreach (var countedCard in countedCards)
            {
                var card = cardsOnBanlist
                    .Where(x => x.Id == countedCard.CardId)
                    .FirstOrDefault();
                if (card != null && additionalCondition(card, countedCard))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<CardWithInfoAboutNumberOfCopiesInDeck> _countCards(Decklist decklist)
        {
            var cardsWithInfoAboutNumberOfCopiesInDeck = new List<CardWithInfoAboutNumberOfCopiesInDeck>();
            _countCardsFromDeck(cardsWithInfoAboutNumberOfCopiesInDeck, decklist.MainDeck);
            _countCardsFromDeck(cardsWithInfoAboutNumberOfCopiesInDeck, decklist.ExtraDeck);
            _countCardsFromDeck(cardsWithInfoAboutNumberOfCopiesInDeck, decklist.SideDeck);

            return cardsWithInfoAboutNumberOfCopiesInDeck;
        }

        private void _countCardsFromDeck(
            List<CardWithInfoAboutNumberOfCopiesInDeck> cardsWithInfoAboutNumberOfCopiesInDeck,
            ICollection<Card> cards)
        {
            foreach (var card in cards)
            {
                var cardWithInfoAboutNumberOfCopiesInDeck = cardsWithInfoAboutNumberOfCopiesInDeck
                    .Where(x => x.CardId == card.Id)
                    .FirstOrDefault();
                if (cardWithInfoAboutNumberOfCopiesInDeck == null)
                {
                    cardWithInfoAboutNumberOfCopiesInDeck = new CardWithInfoAboutNumberOfCopiesInDeck(card.Id);
                    cardsWithInfoAboutNumberOfCopiesInDeck.Add(
                        cardWithInfoAboutNumberOfCopiesInDeck
                    );
                }
                else
                {
                    cardWithInfoAboutNumberOfCopiesInDeck.IncrementNumberOfCopies();
                }
            }
        }

        public class CardWithInfoAboutNumberOfCopiesInDeck
        {
            public CardWithInfoAboutNumberOfCopiesInDeck(int cardId)
            {
                CardId = cardId;
            }

            public int CardId { get; private set; }
            public int NumberOfCopies { get; private set; } = 1;

            public void IncrementNumberOfCopies()
            {
                NumberOfCopies++;
            }
        }
    }
}
