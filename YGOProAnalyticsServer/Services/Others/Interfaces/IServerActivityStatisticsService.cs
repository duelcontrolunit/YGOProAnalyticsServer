using System.Collections.Generic;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DTOs;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    public interface IServerActivityStatisticsService
    {
        Task<IEnumerable<YgoProServerActivityStatisticsDTO>> GetAllAsDtos();
    }
}