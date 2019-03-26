using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer
{
    public class AdminConfig : IAdminConfig
    {
        public string DefaultBanlistName { get; } = "";
    }
}
