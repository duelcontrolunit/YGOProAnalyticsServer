using AutoMapper;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.DTOs.AutomapperProfiles
{
    public class ServerActivityStatisticsToDTOProfile : Profile
    {
        public ServerActivityStatisticsToDTOProfile()
        {
            CreateMap<ServerActivityStatistics, YgoProServerActivityStatisticsDTO>();
        }
    }
}
