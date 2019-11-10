using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer
{
    /// <inheritdoc />
    public class AdminConfig : IAdminConfig
    {
        [JsonProperty(nameof(CardApiURL))]
        ///<inheritdoc />
        public string CardApiURL { get; protected set; } = "https://db.ygoprodeck.com/api/v3/cardinfo.php";

        [JsonProperty(nameof(BetaToOfficialCardApiURL))]
        ///<inheritdoc />
        public string BetaToOfficialCardApiURL { get; protected set; } = "http://eeriecode.altervista.org/tools/get_beta_cards.php";

        [JsonProperty(nameof(DefaultBanlistNumber))]
        ///<inheritdoc />
        public int DefaultBanlistNumber { get; protected set; } = 1;

        [JsonProperty(nameof(FTPUser))]
        ///<inheritdoc />
        public string FTPUser { get; protected set; } = "";

        [JsonProperty(nameof(FTPPassword))]
        ///<inheritdoc />
        public string FTPPassword { get; protected set; } = "";

        [JsonProperty(nameof(YgoProListOfRoomsUrl))]
        ///<inheritdoc />
        public string YgoProListOfRoomsUrl { get; protected set; } = "http://szefoserver.ddns.net:7211/api/getrooms?&pass=";

        [JsonProperty(nameof(DataFolderLocation))]
        ///<inheritdoc />
        public string DataFolderLocation { get; protected set; } = "DataFromServer";

        [JsonProperty(nameof(BanlistApiURL))]
        ///<inheritdoc />
        public string BanlistApiURL { get; protected set; } = "https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf";

        [JsonProperty(nameof(ServerDataEndpointURL))]
        ///<inheritdoc />
        public string ServerDataEndpointURL { get; protected set; } = "";

        [JsonProperty(nameof(DefaultNumberOfResultsPerBrowserPage))]
        ///<inheritdoc />
        public int DefaultNumberOfResultsPerBrowserPage { get; protected set; } = 100;

        [JsonProperty(nameof(BanlistSlidingCacheExpirationInHours))]
        ///<inheritdoc />
        public int BanlistSlidingCacheExpirationInHours { get; protected set; } = 2;

        [JsonProperty(nameof(BanlistAbsoluteCacheExpirationInHours))]
        ///<inheritdoc />
        public int BanlistAbsoluteCacheExpirationInHours { get; protected set; } = 23;

        [JsonProperty(nameof(ArchetypeSlidingCacheExpirationInHours))]
        ///<inheritdoc />
        public int ArchetypeSlidingCacheExpirationInHours { get; protected set; } = 2;

        [JsonProperty(nameof(ArchetypeAbsoluteCacheExpirationInHours))]
        ///<inheritdoc />
        public int ArchetypeAbsoluteCacheExpirationInHours { get; protected set; } = 23;

        [JsonProperty(nameof(MaxNumberOfResultsPerBrowserPage))]
        ///<inheritdoc />
        public int MaxNumberOfResultsPerBrowserPage { get; protected set; } = 100;

        [JsonProperty(nameof(DBUser))]
        ///<inheritdoc />
        public string DBUser { get; protected set; } = "";

        [JsonProperty(nameof(DBPassword))]
        ///<inheritdoc />
        public string DBPassword { get; protected set; } = "";

        /// <summary>
        /// Path to the config file.
        /// Default: "config.json"
        /// </summary>
        public static readonly string path = "config.json";

        public AdminConfig()
        {
            LoadConfigFromFile(AdminConfig.path).Wait();
        }

        ///<inheritdoc />
        public async Task LoadConfigFromFile(string configPath)
        {
            if (!File.Exists(configPath))
            {
                await createConfigFile(configPath);
            }
            string contentOfConfig = await File.ReadAllTextAsync(configPath);
            var serializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            JsonConvert.PopulateObject(contentOfConfig, this, serializerSettings);
        }

        protected async Task createConfigFile(string configPath)
        {
            await File.WriteAllTextAsync(configPath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
