using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Builders.Inferfaces;
using YGOProAnalyticsServer.Services.Updaters.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Updaters
{
    public class CardsDataToCardsAndArchetypesUpdater : ICardsDataToCardsAndArchetypesUpdater
    {
        private readonly ICardsDataDownloader _cardsDataDownloader;
        private readonly ICardBuilder _CardBuilder;
        private readonly YgoProAnalyticsDatabase _db;
        private readonly List<Archetype> _archetypes;

        public CardsDataToCardsAndArchetypesUpdater(
            ICardsDataDownloader cardsDataDownloader, 
            ICardBuilder cardBuilder,
            YgoProAnalyticsDatabase db)
        {
            _cardsDataDownloader = cardsDataDownloader;
            _CardBuilder = cardBuilder;
            _db = db;
            _archetypes = _db.Archetypes.ToList();
        }

        public async Task ConvertCards(string URL)
        {
            string cardsData = await _cardsDataDownloader.DownloadCardsFromWebsite(URL);
            JToken cardsDictionary = (JsonConvert.DeserializeObject<JArray>(cardsData)).First;
            foreach (JObject item in cardsDictionary.Children<JObject>())
            {
                string type = item.Value<string>("type").ToUpper();
                Archetype archetype;
                if (item.GetValue("archetype").ToString()==string.Empty)
                {
                    archetype = _getArchetype("Neutral");
                }
                else
                {
                    archetype = _getArchetype(item.Value<string>("archetype"));
                }
                
                if (type.Contains("MONSTER"))
                {
                    _addMonsterProperties(type, archetype, item);
                }
                _CardBuilder.AddBasicCardElements(
                    item.Value<int>("id"),
                    item.Value<string>("name"),
                    item.Value<string>("desc"),
                    item.Value<string>("type"),
                    item.Value<string>("race"),
                    item.Value<string>("image_url"),
                    item.Value<string>("image_url_small"),
                    archetype
                );
                _db.Cards.Add(_CardBuilder.Build());
            }

            await _db.SaveChangesAsync();
        }

        private void _addMonsterProperties(string type, Archetype archetype, JObject item)
        {           
            _addBasicMonsterAttributesToCard(item);
            if (type.Contains("PENDULUM"))
            {
                _CardBuilder.AddPendulumMonsterCardElements(item.Value<int>("scale"));
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

        private void _addBasicMonsterAttributesToCard(JObject item)
        {
            var level = item.GetValue("level").ToString();
            if (level == "")
            {
                level = 0.ToString();
            }
            _CardBuilder.AddMonsterCardElements(
                    item.GetValue("atk").ToString(),
                    item.GetValue("def").ToString(),
                    int.Parse(level),
                    item.Value<string>("attribute"));
        }

        private void _addLinkElementsToTheCard(JObject item)
        {
            var linkval = item.GetValue("linkval").ToString();
            int linkvalCount = 0;
            if (linkval == "")
            {
                linkvalCount=item.GetValue("linkmarkers").ToString().Split(',').Count() + 1;
            }
            else
            {
                linkvalCount=int.Parse(linkval);
            }
            _CardBuilder.AddLinkMonsterCardElements(
                    linkvalCount,
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
            return item.GetValue("linkmarkers").ToString().Contains(position);
        }
    }
}
