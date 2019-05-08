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
        public string CardApiURL { get; } = "";

        ///<inheritdoc />
        public string DefaultBanlistName { get; } = "";

        public string FTPUser { get; } = "";

        public string FTPPassword { get; } = "";

        public string DataFolderLocation { get; } = "";

        public string BanlistApiURL => "";

        public string ServerDataEndpointURL => "";
    }
}
