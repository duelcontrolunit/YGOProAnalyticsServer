using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Converters.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>Service used to convert ydk string to a new deck containing cards.</summary>
    public class YDKToDecklistConverter : IYDKToDecklistConverter
    {
        private readonly IEnumerable<Card> cards;
        private readonly List<BetaCardData> _betaCardDatas;
        /// <summary>Initializes a new instance of the <see cref="YDKToDecklistConverter"/> class.</summary>
        /// <param name="db">The database.</param>
        public YDKToDecklistConverter(YgoProAnalyticsDatabase db, ICardsDataDownloader cardsDataDownloader, IAdminConfig adminConfig)
        {
            cards = db.Cards.ToList();
            _betaCardDatas = BetaCardToOfficialConverter.LoadBetaCardsList(cardsDataDownloader, adminConfig).Result;
        }

        /// <inheritdoc />
        public Decklist Convert(string ydkString)
        {
            List<Card> mainDeck = new List<Card>();
            List<Card> extraDeck = new List<Card>();
            List<Card> sideDeck = new List<Card>();
            ydkString = ydkString.Replace("\r", "");
            string[] lines = ydkString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            bool isSide = false;
            bool decklistStarted = false;

            foreach (string line in lines)
            {
                if (line.Contains("#main"))
                {
                    decklistStarted = true;
                    continue;
                }

                if (!decklistStarted)
                {
                    continue;
                }

                if (line == "!side")
                {
                    isSide = true;
                    continue;
                }
                else
                {
                    if (Int32.TryParse(line, out int passCode))
                    {
                        if (_isCorrectPassCode(passCode))
                        {
                            if (_isBetaPassCode(passCode))
                            {
                                if (_betaCardDatas.Exists(x => x.BetaPassCode == passCode))
                                {
                                    passCode = _betaCardDatas.First(x => x.BetaPassCode == passCode).OfficialPassCode;
                                }
                            }
                            var card = cards.FirstOrDefault(x => x.PassCode == passCode);
                            if (card == null)
                            {
                                continue;
                            }
                            if (!isSide)
                            {
                                if (card.Type.ToLower().Contains("fusion")
                                    || card.Type.ToLower().Contains("xyz")
                                    || card.Type.ToLower().Contains("link")
                                    || card.Type.ToLower().Contains("synchro"))
                                {
                                    extraDeck.Add(card);
                                }
                                else
                                {
                                    mainDeck.Add(card);
                                }
                            }
                            else
                            {
                                sideDeck.Add(card);
                            }
                        }
                    }
                }
            }
            return new Decklist(mainDeck.OrderBy(x => x.PassCode), extraDeck.OrderBy(x => x.PassCode), sideDeck.OrderBy(x => x.PassCode));
        }

        /// <summary>
        /// Determines whether the passcode is correct.
        /// </summary>
        /// <param name="passCode">The pass code.</param>
        /// <returns>
        ///   <c>true</c> if passcode is correct, otherwise <c>false</c>.
        /// </returns>
        private static bool _isCorrectPassCode(int passCode)
        {
            return passCode > 100;
        }
        private static bool _isBetaPassCode(int passCode)
        {
            return passCode.ToString().Length>8;
        }
    }
}
