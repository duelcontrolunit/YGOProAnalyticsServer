using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    /// <summary>
    /// Provide features related with banlists.
    /// </summary>
    /// <seealso cref="YGOProAnalyticsServer.Services.Others.Interfaces.IBanlistService" />
    public class BanlistService : IBanlistService
    {
        readonly YgoProAnalyticsDatabase _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanlistService"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <exception cref="ArgumentNullException">db</exception>
        public BanlistService(YgoProAnalyticsDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<BanlistIdAndNameDTO>> GetListOfBanlistsNamesAndIdsAsNoTracking()
        {
            return await _db
                .Banlists
                .AsNoTracking()
                .Select(x => new BanlistIdAndNameDTO(x.Id, x.Name))
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Banlist> GetBanlistWithAllCardsIncludedAsync(int banlistId)
        {
            return await _db
                    .Banlists
                    .Where(x => x.Id == banlistId)
                    .Include(Banlist.IncludeWithForbiddenCards)
                    .Include(Banlist.IncludeWithLimitedCards)
                    .Include(Banlist.IncludeWithSemiLimitedCards)
                    .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
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

        /// <summary>
        /// CardId with info about how many copies this card is in deck.
        /// </summary>
        public class CardWithInfoAboutNumberOfCopiesInDeck
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CardWithInfoAboutNumberOfCopiesInDeck"/> class.
            /// </summary>
            /// <param name="cardId">The card identifier.</param>
            public CardWithInfoAboutNumberOfCopiesInDeck(int cardId)
            {
                CardId = cardId;
            }

            /// <summary>
            /// Card identifier.
            /// </summary>
            public int CardId { get; private set; }

            /// <summary>
            /// Number of card copies. One by default.
            /// </summary>
            public int NumberOfCopies { get; private set; } = 1;

            /// <summary>
            /// Increments the number of copies.
            /// </summary>
            public void IncrementNumberOfCopies()
            {
                NumberOfCopies++;
            }
        }
    }
}
