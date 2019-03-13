using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Builders.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    public class CardsDataToCardsAndArchetypesConverter
    {
        private readonly ICardsDataDownloader _cardsDataDownloader;
        private readonly IMonsterCardBuilder _monsterCardBuilder;
        private readonly YgoProAnalyticsDatabase _db;
        private readonly List<Archetype> _archetypes;

        public CardsDataToCardsAndArchetypesConverter(
            ICardsDataDownloader cardsDataDownloader, 
            IMonsterCardBuilder monsterCardBuilder,
            YgoProAnalyticsDatabase db)
        {
            _cardsDataDownloader = cardsDataDownloader;
            _monsterCardBuilder = monsterCardBuilder;
            _db = db;
            _archetypes = _db.Archetypes.ToList();
        }

        public async Task ConvertCards(string URL)
        {
            string cardsData = await _cardsDataDownloader.DownloadCardsFromWebsite(URL);
            var cardsDictionary = (JsonConvert.DeserializeObject<JArray>(cardsData)).First;
            foreach (var item in cardsDictionary.Children<JObject>())
            {
                string type = item.Value<string>("type").ToUpper();
                Archetype archetype = _getArchetype(item.Value<string>("archetype"));
                if (type.Contains("MONSTER"))
                {
                    _addMonsterAttributes(type, archetype, item);
                    _db.Cards.Add(_monsterCardBuilder.Build());
                } else
                {
                    var card = new Card(
                        item.Value<int>("id"),
                        item.Value<string>("name"),
                        item.Value<string>("desc"),
                        item.Value<string>("type"),
                        item.Value<string>("race"),
                        item.Value<string>("image_url"),
                        item.Value<string>("image_url_small")
                    );
                    card.Archetype = archetype;
                    _db.Cards.Add(card);
                }
            }

            await _db.SaveChangesAsync();
        }

        private void _addMonsterAttributes(string type, Archetype archetype, JObject item)
        {           
            _addBasicMonsterAttributesToCard(item, archetype);
            if (type.Contains("PENDULUM"))
            {
                _monsterCardBuilder.AddPendulumElements(item.Value<int>("scale"));
            }
            if (type.Contains("LINK"))
            {
                _addLinkElementsToTheCard(item);
            }
        }

        private Archetype _getArchetype(string archetypeNameFromApi)
        {
            var archetype = _archetypes.Where(x => x.Name == archetypeNameFromApi).FirstOrDefault();
            archetype = archetype ?? new Archetype(archetypeNameFromApi, true);

            return archetype;
        }

        private void _addBasicMonsterAttributesToCard(JObject item, Archetype archetype)
        {
            _monsterCardBuilder
                .AddBasicMonsterElements(
                    item.Value<int>("id"),
                    item.Value<string>("name"),
                    item.Value<string>("desc"),
                    item.Value<string>("type"),
                    item.Value<string>("race"),
                    item.Value<string>("image_url"),
                    item.Value<string>("image_url_small"),
                    item.Value<string>("atk"),
                    item.Value<string>("def"),
                    item.Value<int>("level"),
                    item.Value<string>("attribute"),
                    archetype
                );
        }

        private void _addLinkElementsToTheCard(JObject item)
        {
            _monsterCardBuilder.AddLinkElements(
                    item.Value<int>("linkval"),
                    _isLinkMarker("Top-Left", item),
                    _isLinkMarker("Top", item),
                    _isLinkMarker("Top-Right", item),
                    _isLinkMarker("Left", item),
                    _isLinkMarker("Right", item),
                    _isLinkMarker("Bottom-Left", item),
                    _isLinkMarker("Bottom", item),
                    _isLinkMarker("Bottom-Right", item)
                );
        }

        private bool _isLinkMarker(string position, JObject item)
        {
            return item.Value<string>("linkmarkers").Contains(position);
        }
    }
}
