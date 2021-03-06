﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;
using Microsoft.EntityFrameworkCore;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;

namespace YGOProAnalyticsServer.Services.Updaters
{
    /// <summary>
    /// Provide methods which enable banlists update.
    /// </summary>
    public class BanlistDataToBanlistUpdater : IBanlistDataToBanlistUpdater
    {
        readonly YgoProAnalyticsDatabase _db;
        readonly IBanlistDataDownloader _banlistDataDownloader;
        List<Banlist> _banlists;

        bool _areUpdatedForbiddenCardsNow = false;
        bool _areUpdatedLimitedCardsNow = false;
        bool _areUpdatedSemiLimitedCardsNow = false;
        int _banlistNumberInLflist = 0;

        /// <summary>
        /// Create new banlist updater.
        /// </summary>
        /// <param name="db">Acces to database.</param>
        /// <param name="banlistDataDownloader">Is responsible for download banlists as a text.</param>
        public BanlistDataToBanlistUpdater(YgoProAnalyticsDatabase db, IBanlistDataDownloader banlistDataDownloader)
        {
            _db = db;
            _banlistDataDownloader = banlistDataDownloader;
           
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Banlist>> UpdateBanlists(string url)
        {
            _banlists = _db.Banlists.ToList();
            List<Card> cards = _db.Cards.ToList();
            string banlistDataAsString = await _banlistDataDownloader.DownloadBanlistFromWebsite(url);
            var banlistDatas = banlistDataAsString.Replace("\r",String.Empty).Split("\n");
            List<Banlist> newBanlists = new List<Banlist>();
            Banlist banlist = null;
            for (int i = 1; i < banlistDatas.Length; i++)
            {
                var line = banlistDatas[i];
                if (line.Length == 0)
                {
                    continue;
                }

                if (_isInformationAboutBanlistName(line))
                {
                    _ifThereIsAnyBanlistAddItToDbContext(banlist);
                    string banlistName = line.Substring(1);
                    if (_isBanlistAlreadyInDatabase(banlistName))
                    {
                        continue;
                    }

                    banlist = new Banlist(banlistName, _banlistNumberInLflist);
                    newBanlists.Add(banlist);
                    continue;
                }

                _checkIfWeAnalyzeForbiddenOrLimitedOrSemiLimitedCards(line);
                if (banlist != null && !_isInformationAboutCardCountLimitations(line))
                {
                    int cardPassCode = int.Parse(line.Substring(0, line.IndexOf(' ')));
                    var card = cards
                        .FirstOrDefault(x => x.PassCode == cardPassCode);
                    if (card == null)
                    {
                        continue;
                    }

                    _addCardToAppropriateBanlistSection(banlist, card);
                }
            }

            _ifThereIsAnyBanlistAddItToDbContext(banlist);
            await _db.SaveChangesAsync();

            return newBanlists;
        }

        private void _addCardToAppropriateBanlistSection(Banlist banlist, Card card)
        {
            if (_areUpdatedForbiddenCardsNow && !banlist.ForbiddenCards.Contains(card))
            {
                banlist.ForbiddenCards.Add(card);
            }
            else if (_areUpdatedLimitedCardsNow && !banlist.LimitedCards.Contains(card))
            {
                banlist.LimitedCards.Add(card);
            }
            else if (_areUpdatedSemiLimitedCardsNow && !banlist.SemiLimitedCards.Contains(card))
            {
                banlist.SemiLimitedCards.Add(card);
            }
        }

        private bool _isBanlistAlreadyInDatabase(string banlistName)
        {
            return _banlists.Where(x => x.Name == banlistName).FirstOrDefault() != null;
        }

        private void _ifThereIsAnyBanlistAddItToDbContext(Banlist banlist)
        {
            if (banlist != null)
            {
                _db.Banlists.Add(banlist);
            }
        }

        private bool _isInformationAboutCardCountLimitations(string line)
        {
            return line[0] == '#';
        }

        private bool _isInformationAboutBanlistName(string line)
        {
            if (line[0] == '!')
            {
                _banlistNumberInLflist++;
                return true;
            }

            return false;
        }

        private void _checkIfWeAnalyzeForbiddenOrLimitedOrSemiLimitedCards(string line)
        {
            if (!_isInformationAboutCardCountLimitations(line))
            {
                return;
            }

            if (line.ToUpper().Contains("#FORBIDDEN"))
            {
                _areUpdatedForbiddenCardsNow = true;
                _areUpdatedLimitedCardsNow = false;
                _areUpdatedSemiLimitedCardsNow = false;
            }
            else if (line.ToUpper().Contains("#LIMITED"))
            {
                _areUpdatedForbiddenCardsNow = false;
                _areUpdatedLimitedCardsNow = true;
                _areUpdatedSemiLimitedCardsNow = false;
            }
            else if (line.ToUpper().Contains("#SEMI"))
            {
                _areUpdatedForbiddenCardsNow = false;
                _areUpdatedLimitedCardsNow = false;
                _areUpdatedSemiLimitedCardsNow = true;
            }
        }
    }
}
