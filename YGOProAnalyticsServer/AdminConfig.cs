using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer
{
    [Serializable]
    /// <inheritdoc />
    public class AdminConfig : IAdminConfig
    {
        [JsonProperty(nameof(CardApiURL))]
        ///<inheritdoc />
        public string CardApiURL { get; } = "https://db.ygoprodeck.com/api/v3/cardinfo.php";

        [JsonProperty(nameof(DefaultBanlistName))]
        ///<inheritdoc />
        public string DefaultBanlistName { get; } = "2019.04 TCG";

        [JsonProperty(nameof(FTPUser))]
        ///<inheritdoc />
        public string FTPUser { get; } = "";

        [JsonProperty(nameof(FTPPassword))]
        ///<inheritdoc />
        public string FTPPassword { get; } = "";

        [JsonProperty(nameof(YgoProListOfRoomsUrl))]
        ///<inheritdoc />
        public string YgoProListOfRoomsUrl => "http://szefoserver.ddns.net:7211/api/getrooms?&pass=";

        [JsonProperty(nameof(DataFolderLocation))]
        ///<inheritdoc />
        public string DataFolderLocation { get;} = "DataFromServer";

        [JsonProperty(nameof(BanlistApiURL))]
        ///<inheritdoc />
        public string BanlistApiURL => "https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf";

        [JsonProperty(nameof(ServerDataEndpointURL))]
        ///<inheritdoc />
        public string ServerDataEndpointURL => "";

        [JsonProperty(nameof(DefaultNumberOfResultsPerBrowserPage))]
        ///<inheritdoc />
        public int DefaultNumberOfResultsPerBrowserPage => 100;

        [JsonProperty(nameof(BanlistSlidingCacheExpirationInHours))]
        ///<inheritdoc />
        public int BanlistSlidingCacheExpirationInHours => 2;

        [JsonProperty(nameof(BanlistAbsoluteCacheExpirationInHours))]
        ///<inheritdoc />
        public int BanlistAbsoluteCacheExpirationInHours => 23;

        [JsonProperty(nameof(ArchetypeSlidingCacheExpirationInHours))]
        ///<inheritdoc />
        public int ArchetypeSlidingCacheExpirationInHours => 2;

        [JsonProperty(nameof(ArchetypeAbsoluteCacheExpirationInHours))]
        ///<inheritdoc />
        public int ArchetypeAbsoluteCacheExpirationInHours => 23;

        [JsonProperty(nameof(MaxNumberOfResultsPerBrowserPage))]
        ///<inheritdoc />
        public int MaxNumberOfResultsPerBrowserPage { get; protected set; } = 100;

        /// <summary>
        /// Path to the config file.
        /// Default: "config.json"
        /// </summary>
        public static readonly string Path = "config.json";

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
