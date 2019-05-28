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
        public string CardApiURL { get; } = "https://db.ygoprodeck.com/api/v3/cardinfo.php";

        ///<inheritdoc />
        public string DefaultBanlistName { get; } = "2019.04 TCG";

        public string FTPUser { get; } = "";

        public string FTPPassword { get; } = "";

        public string YgoProListOfRoomsUrl => "http://szefoserver.ddns.net:7211/api/getrooms?&pass=";
        
        public string DataFolderLocation { get; } = "DataFromServer";

        public string BanlistApiURL => "https://raw.githubusercontent.com/szefo09/updateYGOPro2/master/lflist.conf";

        public string ServerDataEndpointURL => "";
    }
}
