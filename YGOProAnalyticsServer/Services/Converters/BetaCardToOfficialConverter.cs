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
        private readonly YgoProAnalyticsDatabase _db;
        private readonly IAdminConfig _adminConfig;
        private readonly ICardsDataDownloader _cardsDataDownloader;
        private static List<BetaCardData> betaCardsList;
        public BetaCardToOfficialConverter(YgoProAnalyticsDatabase db, IAdminConfig adminConfig, ICardsDataDownloader cardsDataDownloader)
        {
            _db = db;
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
                    Card officialCard = await _GetCardWithSpecificPasscodeWithJoinsIfItExistsInDatabase(officialPassCode);
                    if (officialCard != null)
                    {
                        Card betaCard = await _GetCardWithSpecificPasscodeWithJoinsIfItExistsInDatabase(betaPassCode);
                        if (betaCard != null)
                        {
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
                            _db.Cards.Remove(betaCard);
                        }
                    }
                    else
                    {
                        Card betaCard = await _GetCardWithSpecificPasscodeWithJoinsIfItExistsInDatabase(betaPassCode);
                        if (betaCard != null)
                        {
                            betaCard.ChangePassCode(officialPassCode);
                        }
                    }
                }
            }

            await _db.SaveChangesAsync();
        }

        private async Task<Card> _GetCardWithSpecificPasscodeWithJoinsIfItExistsInDatabase(int passCode)
        {
            Card card = await _db.Cards.FirstOrDefaultAsync(x => x.PassCode == passCode);
            if(card != null)
            {
                await _db.Entry<Card>(card).Collection(x => x.MainDeckJoin)
                    .EntityEntry.Collection(x => x.ExtraDeckJoin)
                    .EntityEntry.Collection(x => x.SideDeckJoin)
                    .EntityEntry.Collection(x => x.ForbiddenCardsJoin)
                    .EntityEntry.Collection(x => x.LimitedCardsJoin)
                    .EntityEntry.Collection(x => x.SemiLimitedCardsJoin).LoadAsync();
            }

            return card;
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
