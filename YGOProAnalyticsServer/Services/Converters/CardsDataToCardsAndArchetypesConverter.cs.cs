using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Services.Builders.Interfaces;
using YGOProAnalyticsServer.Services.Downloaders.Interfaces;

namespace YGOProAnalyticsServer.Services.Converters
{
    public class CardsDataToCardsAndArchetypesConverter
    {
        private readonly ICardsDataDownloader _cardsDataDownloader;
        private readonly IMonsterCardBuilder _monsterCardBuilder;

        public CardsDataToCardsAndArchetypesConverter(ICardsDataDownloader cardsDataDownloader, IMonsterCardBuilder monsterCardBuilder)
        {
            _cardsDataDownloader = cardsDataDownloader;
            _monsterCardBuilder = monsterCardBuilder;

        }

        //TO_DO: ADD ARCHETYPES

        public async Task ConvertCards(string URL)
        {
            string cardsData = await _cardsDataDownloader.DownloadCardsFromWebsite(URL);
            Dictionary<string, string> cardsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(cardsData);

            string typeOfCard = cardsDictionary["type"].ToUpper();
            if (typeOfCard.Contains("MONSTER"))
            {
                //_monsterCardBuilder.AddBasicMonsterElements();
            }
        }
    }
}
