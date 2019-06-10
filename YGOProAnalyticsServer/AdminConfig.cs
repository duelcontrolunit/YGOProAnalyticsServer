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
        ///<inheritdoc />
        public string CardApiURL { get; } = "https://db.ygoprodeck.com/api/v3/cardinfo.php";

        ///<inheritdoc />
        public string DefaultBanlistName { get; } = "2019.04 TCG";

        ///<inheritdoc />
        public string FTPUser { get; } = "";

        ///<inheritdoc />
        public string FTPPassword { get; } = "";

        ///<inheritdoc />
        public string YgoProListOfRoomsUrl => "http://szefoserver.ddns.net:7211/api/getrooms?&pass=";

        ///<inheritdoc />
        public string DataFolderLocation { get; } = "DataFromServer";

        ///<inheritdoc />
        public string BanlistApiURL => "https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf";

        ///<inheritdoc />
        public string ServerDataEndpointURL => "";

        ///<inheritdoc />
        public int DefaultNumberOfResultsPerBrowserPage => 100;

        ///<inheritdoc />
        public int BanlistSlidingCacheExpirationInHours => 2;

        ///<inheritdoc />
        public int BanlistAbsoluteCacheExpirationInHours => 23;

        ///<inheritdoc />
        public int ArchetypeSlidingCacheExpirationInHours => 2;

        ///<inheritdoc />
        public int ArchetypeAbsoluteCacheExpirationInHours => 23;

        ///<inheritdoc />
        public int MaxNumberOfResultsPerBrowserPage => 100;
    }
}
