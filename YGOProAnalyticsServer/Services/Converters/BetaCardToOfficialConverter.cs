using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    /// <summary>
    /// Class used to convert Beta Cards to Official Cards
    /// </summary>
    public class BetaCardToOfficialConverter : IBetaCardToOfficialConverter
    {
        private readonly List<Card> _cards;
        YgoProAnalyticsDatabase _db;
        private readonly IAdminConfig _adminConfig;
        private readonly ICardsDataDownloader _cardsDataDownloader;
        private static List<BetaCardData> betaCardsList;
        public BetaCardToOfficialConverter(YgoProAnalyticsDatabase db, IAdminConfig adminConfig, ICardsDataDownloader cardsDataDownloader)
        {
            _db = db;
            _cards = db.Cards
                .Include(x => x.ExtraDeckJoin)
                .Include(x => x.MainDeckJoin)
                .Include(x => x.SideDeckJoin)
                .Include(x=>x.ForbiddenCardsJoin)
                .Include(x => x.SemiLimitedCardsJoin)
                .Include(x => x.LimitedCardsJoin)
                .ToList();

            _adminConfig = adminConfig;
            _cardsDataDownloader = cardsDataDownloader;
        }

        /// <inheritdoc />
        public async Task UpdateCardsFromBetaToOfficial()
        {
            await LoadBetaCardsList(_cardsDataDownloader, _adminConfig);
            if (_db.Cards.Count() == 0) return;

            foreach (var cardData in betaCardsList)
            {
                int officialPassCode = cardData.OfficialPassCode;
                int betaPassCode = cardData.BetaPassCode;
                if (betaPassCode != 0 || officialPassCode != 0)
                {
                    if (_cardAlreadyExistInOurDatabase(officialPassCode))
                    {
                        if (_cardAlreadyExistInOurDatabase(betaPassCode))
                        {
                            var betaCard = _cards.Find(card => card.PassCode == betaPassCode);
                            var officialCard = _cards.Find(card => card.PassCode == officialPassCode);

                            foreach (var banlist in betaCard.BanlistsWhereThisCardIsForbidden)
                            {
                                if (officialCard.BanlistsWhereThisCardIsForbidden.Contains(banlist)) continue;
                                officialCard.BanlistsWhereThisCardIsForbidden.Add(banlist);
                            }

                            foreach (var banlist in betaCard.BanlistsWhereThisCardIsLimited)
                            {
                                if (officialCard.BanlistsWhereThisCardIsLimited.Contains(banlist)) continue;
                                officialCard.BanlistsWhereThisCardIsLimited.Add(banlist);
                            }

                            foreach (var banlist in betaCard.BanlistsWhereThisCardIsSemiLimited)
                            {
                                if (officialCard.BanlistsWhereThisCardIsSemiLimited.Contains(banlist)) continue;
                                officialCard.BanlistsWhereThisCardIsSemiLimited.Add(banlist);
                            }

                            foreach (var decklist in betaCard.DecksWhereThisCardIsInMainDeck)
                            {
                                if (officialCard.DecksWhereThisCardIsInMainDeck.Contains(decklist)) continue;
                                officialCard.DecksWhereThisCardIsInMainDeck.Add(decklist);
                            }

                            foreach (var decklist in betaCard.DecksWhereThisCardIsInSideDeck)
                            {
                                if (officialCard.DecksWhereThisCardIsInSideDeck.Contains(decklist)) continue;
                                officialCard.DecksWhereThisCardIsInSideDeck.Add(decklist);
                            }

                            foreach (var decklist in betaCard.DecksWhereThisCardIsInExtraDeck)
                            {
                                if (officialCard.DecksWhereThisCardIsInExtraDeck.Contains(decklist)) continue;
                                officialCard.DecksWhereThisCardIsInExtraDeck.Add(decklist);
                            }
                            _cards.Remove(betaCard);
                            _db.Cards.Remove(betaCard);
                        }
                    }
                    else
                    {
                        if (_cardAlreadyExistInOurDatabase(betaPassCode))
                        {
                            _cards.Find(card => card.PassCode == betaPassCode).ChangePassCode(officialPassCode);
                        }
                    }
                }
            }
            await _db.SaveChangesAsync();
        }
        private bool _cardAlreadyExistInOurDatabase(int id)
        {
            return _cards.Find(x => x.PassCode == id) != null;
        }

        public static async Task<List<BetaCardData>> LoadBetaCardsList(ICardsDataDownloader cardsDataDownloader, IAdminConfig adminConfig)
        {
            if (betaCardsList == null)
            {
                string betaToOfficialCardsData = await cardsDataDownloader.DownloadCardsFromWebsite(adminConfig.BetaToOfficialCardApiURL);
                JArray betaCardsListJArray = JsonConvert.DeserializeObject<JArray>(betaToOfficialCardsData);
                betaCardsList = new List<BetaCardData>();
                lock (betaCardsList)
                {
                    foreach (JObject jObject in betaCardsListJArray)
                    {
                        int officialId = jObject.Value<int>("ocode");
                        int betaId = jObject.Value<int>("ucode");
                        string name = jObject.Value<string>("name");
                        if (officialId > 0 && betaId > 0)
                        {
                            betaCardsList.Add(new BetaCardData(name, officialId, betaId));
                        }
                    }
                }

            }
            return betaCardsList;
        }
    }
}
