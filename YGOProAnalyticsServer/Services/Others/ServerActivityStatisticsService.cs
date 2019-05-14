using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DTOs;
using YGOProAnalyticsServer.Services.Others.Interfaces;

namespace YGOProAnalyticsServer.Services.Others
{
    public class ServerActivityStatisticsService : IServerActivityStatisticsService
    {
        YgoProAnalyticsDatabase _db;
        IMapper _mapper;

        public ServerActivityStatisticsService(YgoProAnalyticsDatabase db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<YgoProServerActivityStatisticsDTO>> GetAllAsDtos()
        {
            var statistics = await _db.ServerActivityStatistics.ToListAsync();

            return _mapper.Map<List<YgoProServerActivityStatisticsDTO>>(statistics);
        }
    }
}
