﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using YGOProAnalyticsServer.Models;
using YGOProAnalyticsServer.Services.Unzippers.Interfaces;

namespace YGOProAnalyticsServer.Services.Unzippers
{
    /// <summary>
    /// Provide methods required to nzip Files to memory.
    /// </summary>
    public class FileUnzipper : IFileUnzipper
    {
        /// <inheritdoc />
        public string GetDuelLogFromZip(string duelLogZipFilePath)
        {
            using (ZipArchive duelLogZipArchive = ZipFile.OpenRead(duelLogZipFilePath))
            {
                using (Stream duelLogStream = duelLogZipArchive.Entries.First().Open())
                {
                    using (StreamReader sr = new StreamReader(duelLogStream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        /// <inheritdoc />
        public List<DecklistWithName> GetDecksFromZip(string decksZipFilePath)
        {
            List<DecklistWithName> decks = new List<DecklistWithName>();
            using (ZipArchive decksZipArchive = ZipFile.OpenRead(decksZipFilePath))
            {
                foreach (ZipArchiveEntry decksArchiveEntry in decksZipArchive.Entries)
                {
                    using (StreamReader sr = new StreamReader(decksArchiveEntry.Open()))
                    {
                        var deckData = sr.ReadToEnd();
                        if (deckData == "")
                        {
                            continue;
                        }
                        var decklistWithName = new DecklistWithName(decksArchiveEntry.Name, deckData);
                        decks.Add(decklistWithName);
                    }

                }
            }
            return decks;
        }
    }
}
