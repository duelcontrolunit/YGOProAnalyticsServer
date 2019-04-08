using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer
{
    /// <inheritdoc />
    public class AdminConfig : IAdminConfig
    {
        ///<inheritdoc />
        public string DefaultBanlistName { get; } = "";

        public string FTPUser { get; } = "";

        public string FTPPassword { get; } = "";
    }
}
